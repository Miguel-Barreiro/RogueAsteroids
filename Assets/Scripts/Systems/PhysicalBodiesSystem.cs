using Components;

namespace Systems
{
	public class PhysicalBodiesSystem : System, IExecuteSystem
	{

		public void Execute(float elapsedTime)
		{
			foreach (PhysicalBody physicalBody in ComponentFactory<PhysicalBody>.All)
			{
				// physicalBody.
			}

		}

		public override void Disable() {}
		public override void Enable() {}
	}

}