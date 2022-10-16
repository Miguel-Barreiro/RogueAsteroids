using Core;
using UnityEngine;
using View;

namespace Components
{
	public sealed class PhysicalBody
	{
		public Vector2 Velocity = Vector2.zero;
		public Vector2 Direction = Vector2.up;
		public float TurnSpeed = 1;
		public float BreakSpeed = 1;
		
		public readonly BindableProperty<PhysicsBodyView> BodyView = new BindableProperty<PhysicsBodyView>(null);
	}
}