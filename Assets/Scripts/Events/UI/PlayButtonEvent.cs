using System;

namespace Events.UI
{
	public sealed class PlayButtonEvent
	{
		public event Action OnPlayButton;
		
		public void TriggerPlayButton() { OnPlayButton?.Invoke(); }
	}
}