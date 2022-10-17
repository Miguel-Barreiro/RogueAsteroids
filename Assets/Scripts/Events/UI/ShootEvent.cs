using System;
using Entities;

namespace Events.UI
{
	public sealed class ShootEvent
	{
		public event Action<Ship> OnShoot;

		public void TriggerShoot(Ship obj) { OnShoot?.Invoke(obj); }
	}
}