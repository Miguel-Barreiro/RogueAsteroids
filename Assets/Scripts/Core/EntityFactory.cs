using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using View;

namespace Core
{
	public sealed class EntityFactory<T> : DependencyManager.IDependencyRequired where T : IEntity, new()
	{
		private readonly List<T> _entitiesSpawned = new List<T>();
		private readonly List<T> _entityPool = new List<T>();

		private readonly static List<Action> _destroyedEntities = new List<Action>();

		private EntityCycleEvent<T> _cycleEvent;
		public void SetupDependencies(DependencyManager manager)
		{
			_cycleEvent = manager.Get<EntityCycleEvent<T>>();
			_prefabFactory = manager.Get<PrefabFactory>();
		}


		/// <summary>
		/// warning: this function creates a new array every call 
		/// </summary>
		/// <returns></returns>
		public T[] GetAll()
		{
			T[] result = new T[_entitiesSpawned.Count];
			_entitiesSpawned.CopyTo(result);
			return result;
		}
		
		
		public T CreateNew(Action<T> setupEntity)
		{
			T newEntity;
			if (_entityPool.Count > 0)
			{
				newEntity = _entityPool[0];
				_entityPool.Remove(newEntity);
			} else
			{
				newEntity = new T();
			}
			newEntity.RegisterComponents();
			setupEntity?.Invoke(newEntity);
			
			_entitiesSpawned.Add(newEntity);
			_cycleEvent.TriggerCreated(newEntity);
			return newEntity;
		}

		public void DestroyEntity(T entity)
		{
			if (_entitiesSpawned.Contains(entity))
			{
				_entitiesSpawned.Remove(entity);
				_destroyedEntities.Add(() => {
					_cycleEvent.TriggerDestroyed(entity);
					entity.Destroy(_prefabFactory);
					_entityPool.Add(entity);
				});
			}
		}

		private static readonly Action[] _tempArray = new Action[100];
		private PrefabFactory _prefabFactory;

		public static void TriggerEntitiesDestroyed()
		{
			_destroyedEntities.CopyTo(_tempArray);
			for (int i = 0; i < _tempArray.Length; i++)
				if (_tempArray[i] != null)
				{
					_destroyedEntities.Remove(_tempArray[i]);
					_tempArray[i].Invoke();
					_tempArray[i] = null;
				}
			
		}
	}
}