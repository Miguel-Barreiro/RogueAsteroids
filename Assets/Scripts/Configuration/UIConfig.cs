using UnityEngine;

namespace Configuration
{
	[CreateAssetMenu(menuName = "Game Configurations/UIConfig", order = 1)]
	public class UIConfig : ScriptableObject
	{

		public GameObject MainMenuPrefab;
		public GameObject GameUIPrefab;
	}
}