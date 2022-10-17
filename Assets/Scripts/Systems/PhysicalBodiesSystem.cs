using Components;
using Core;
using UnityEngine;
using View;

namespace Systems
{
	public class PhysicalBodiesSystem : System, IExecuteSystem
	{

		public void Execute(float elapsedTime)
		{
			foreach (PhysicalBody physicalBody in ComponentFactory<PhysicalBody>.All)
			{
				PhysicsBodyView physicsBodyView = physicalBody.BodyView.Value;
				if (physicsBodyView != null)
				{
					Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * physicalBody.Direction;
					Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
					
					Transform rigidBodyTransform = physicsBodyView.RigidBody.transform;

					Quaternion newRotation = Quaternion.RotateTowards(rigidBodyTransform.rotation, targetRotation,
																		physicalBody.TurnSpeed * elapsedTime);
					rigidBodyTransform.rotation = newRotation;
					
					float breakingSpeed = physicalBody.BreakSpeed * elapsedTime;
					physicsBodyView.RigidBody.velocity = Vector2.Lerp(physicsBodyView.RigidBody.velocity, physicalBody.Velocity, breakingSpeed);
				}
			}

		}

		public override void Disable() {}
		public override void Enable() {}
	}

}