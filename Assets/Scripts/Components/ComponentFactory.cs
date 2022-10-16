using System;
using System.Collections.Generic;

namespace Components
{
	
	public static class ComponentFactory<T> where T : new()
	{
		public static event Action<T> OnComponentCreated;
		public static event Action<T> OnComponentDestroyed;

		public readonly static List<T> All = new List<T>();

		public static T Add()
		{
			T newComponent= new T();
			OnComponentCreated?.Invoke(newComponent);
			All.Add(newComponent);
			return newComponent;
		}

		public static void DestroyComponent(T component)
		{
			All.Remove(component);
			OnComponentDestroyed?.Invoke(component);
		}
	}
}