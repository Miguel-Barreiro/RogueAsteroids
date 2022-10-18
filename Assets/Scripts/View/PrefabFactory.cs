using System.Collections.Generic;
using Core;
using Entities;
using Systems;
using UnityEngine;

namespace View
{
	// here we can add an object pool to reuse gameobjects rather than instantiate them everytime
	public sealed class PrefabFactory : DependencyManager.IDependencyRequired
	{
		private readonly Dictionary<GameObject, List<GameObject>> _gameObjectPool = new Dictionary<GameObject, List<GameObject>>();
		private readonly Dictionary<GameObject, GameObject> _prefabsByGameObject = new Dictionary<GameObject, GameObject>();
		private DependencyManager _dependencyManager;

		public void SetupDependencies(DependencyManager manager)
		{
			_dependencyManager = manager;
		}

		public void Destroy(GameObject gameObject)
		{
			if (!_prefabsByGameObject.ContainsKey(gameObject))
			{
				GameObject.Destroy(gameObject);
				return;
			}
			GameObject prefab = _prefabsByGameObject[gameObject];
			if (!_gameObjectPool[prefab].Contains(gameObject))
				_gameObjectPool[prefab].Add(gameObject);
			
			gameObject.SetActive(false);
		}

		public GameObject CreateNew(GameObject prefab, Transform parent)
		{
			if (!_gameObjectPool.ContainsKey(prefab))
				_gameObjectPool.Add(prefab, new List<GameObject>());
			
			List<GameObject> pool = _gameObjectPool[prefab];
			if (pool.Count > 0)
			{
				GameObject pooledGameobject = pool[0];
				pool.Remove(pooledGameobject);
				pooledGameobject.SetActive(true);
				if (parent != null)
					pooledGameobject.transform.SetParent(parent, true);
				return pooledGameobject;
			}
			
			GameObject result = GameObject.Instantiate(prefab, parent);
			_prefabsByGameObject.Add(result, prefab);
			_dependencyManager.HandleDependencies(result);

			return result;
		}
	}
}