using System;
using UnityEngine;

namespace View
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicsBodyView : MonoBehaviour
	{
		public event Action<Collision2D, PhysicsBodyView> OnCollision;
		
		public Rigidbody2D RigidBody;

		private void Awake()
		{
			RigidBody = GetComponent<Rigidbody2D>();
		}
		

		private void OnCollisionEnter2D(Collision2D collision)
		{
			OnCollision?.Invoke(collision, this);
		}

	}
}