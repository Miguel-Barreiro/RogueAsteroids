using UnityEngine;

namespace Configuration
{
	[CreateAssetMenu(menuName = "Game Configurations/ShipConfig", order = 1)]
	public class ShipConfig : ScriptableObject
	{
		[Range(0, 10)]
		public float BreakingSpeed = 1;
		[Range(0, 10)]
		public float TurningSpeed = 1;
		
		public int StartingLifes = 3;
		public GameObject ShipPrefab;
		
		[Range(0, 10)]
		public float Velocity = 1;
	}

}