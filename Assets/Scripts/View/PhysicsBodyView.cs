using System;
using UnityEngine;

namespace View
{
	public class PhysicsBodyView : MonoBehaviour
	{
		public event Action<Collision> OnCollision;
		
		private void OnCollisionEnter(Collision collision)
		{
			OnCollision?.Invoke(collision);
		}

	}
}