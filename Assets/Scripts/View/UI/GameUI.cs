using Core;
using TMPro;
using UnityEngine;

namespace View.UI
{
	public class GameUI : MonoBehaviour, DependencyManager.IDependencyRequired
	{
		public TMP_Text Score;
		public TMP_Text Lifes;

		public void SetupDependencies(DependencyManager manager)
		{
			
		}
	}
}