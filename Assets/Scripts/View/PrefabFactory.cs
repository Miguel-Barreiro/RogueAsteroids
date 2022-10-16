using Systems;
using Systems.Core;
using UnityEngine;

namespace View
{
	// here we can add an object pool to reuse gameobjects rather than instantiate them everytime
	public class PrefabFactory : DependencyManager.IDependencyRequired
	{
		private DependencyManager _dependencyManager;

		public void SetupDependencies(DependencyManager manager)
		{
			_dependencyManager = manager;
		}

		public void Destroy(GameObject gameObject)
		{
			GameObject.Destroy(gameObject);
		}

		public GameObject CreateNew(GameObject prefab, Transform parent)
		{
			GameObject result = GameObject.Instantiate(prefab, parent);

			_dependencyManager.HandleDependencies(result);

			return result;
		}
	}
}