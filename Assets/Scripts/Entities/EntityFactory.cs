using System.Collections.Generic;
using Events;
using Systems.Core;

namespace Entities
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

		public List<T> GetAll()
		{
			return _entitiesSpawned;
		}

		public T CreateNew()
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