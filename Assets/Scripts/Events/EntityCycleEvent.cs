using System;

namespace Events
{
	public class EntityCycleEvent<T>
	{
		public event Action<T> OnDestroyed;
		public event Action<T> OnCreated;

		public void TriggerCreated(T entity)
		{
			OnCreated?.Invoke(entity);
		}

		public void TriggerDestroyed(T entity)
		{
			OnDestroyed?.Invoke(entity);
		}
	}
	
}