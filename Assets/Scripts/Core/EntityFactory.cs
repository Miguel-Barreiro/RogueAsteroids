using System;
using System.Collections.Generic;
using Events;

namespace Core
{
	public class EntityFactory<T> : DependencyManager.IDependencyRequired where T : new()
	{
		private readonly List<T> _entitiesSpawned = new List<T>();
		private readonly List<T> _entityPool = new List<T>();

		private EntityCycleEvent<T> _cycleEvent;
		public void SetupDependencies(DependencyManager manager)
		{
			_cycleEvent = manager.Get<EntityCycleEvent<T>>();
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

			setupEntity?.Invoke(newEntity);
			
			_entitiesSpawned.Add(newEntity);
			_cycleEvent.TriggerCreated(newEntity);
			return newEntity;
		}

		public void DestroyEntity(T entity)
		{
			_entitiesSpawned.Remove(entity);
			_entityPool.Add(entity);
			_cycleEvent.TriggerDestroyed(entity);
		}
	}
}