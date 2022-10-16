using UnityEngine;

namespace Configuration
{
	[CreateAssetMenu(menuName = "ScriptableObjects/UIConfig", order = 1)]
	public class UIConfig : ScriptableObject
	{

		public GameObject MainMenuPrefab;
		public GameObject GameUIPrefab;
	}
}