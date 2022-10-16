using UnityEngine;

namespace Systems
{
	public class System
	{
		public virtual void Disable() {}

		public virtual void Enable() {}
	}
	
	public interface ISetupSystem
	{
		void Setup();
	}

	public interface IExecuteSystem
	{
		void Execute(float elapsedTime);
	}

}