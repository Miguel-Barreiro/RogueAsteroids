using System;
using UnityEngine;

namespace View
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsBodyView : MonoBehaviour
	{
		public event Action<Collision> OnCollision;
		
		public Rigidbody2D RigidBody;

		private void Awake()
		{
			RigidBody = GetComponent<Rigidbody2D>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			OnCollision?.Invoke(collision);
		}

	}
}