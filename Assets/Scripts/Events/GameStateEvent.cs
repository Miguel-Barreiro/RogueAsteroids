using System;

namespace Events
{
	public sealed class GameStateEvent
	{
		public event Action OnGameLoad;
		public event Action OnGameStart;
		public event Action OnGameEnd;

		public void TriggerOnGameLoad() { OnGameLoad?.Invoke(); }
		public void TriggerGameStart() { OnGameStart?.Invoke(); }
		public void TriggerGameEnd() { OnGameEnd?.Invoke(); }
	}
}