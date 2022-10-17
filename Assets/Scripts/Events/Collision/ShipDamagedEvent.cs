using System;

namespace Events.Collision
{
	public sealed class ShipDamagedEvent
	{
		public event Action<int> OnDamaged;
		public void TriggerOnDamaged(int deltaLife) { OnDamaged?.Invoke(deltaLife); }
	}
}