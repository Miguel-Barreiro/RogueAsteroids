using UnityEngine;

namespace Configuration
{
	[CreateAssetMenu(fileName = "Level", menuName = "Game Configurations/level configuration ", order = 0)]
	public class LevelConfiguration : ScriptableObject
	{
		public GameObject BackgroundPrefab;
	}
}