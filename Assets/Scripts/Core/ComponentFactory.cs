using System;
using System.Collections.Generic;

namespace Core
{
	
	public static class ComponentFactory<T>
	{
		public static event Action<T> OnComponentCreated;
		public static event Action<T> OnComponentDestroyed;

		public readonly static List<T> All = new List<T>();

		public static void Add(T newComponent)
		{
			All.Add(newComponent);
			OnComponentCreated?.Invoke(newComponent);
		}

		public static void DestroyComponent(T component)
		{
			All.Remove(component);
			OnComponentDestroyed?.Invoke(component);
		}
	}
}