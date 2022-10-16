using UnityEngine;

namespace Configuration
{
	[CreateAssetMenu(menuName = "ScriptableObjects/ShipConfig", order = 1)]
	public class ShipConfig : ScriptableObject
	{
		public int StartingLifes = 3;
		public GameObject ShipPrefab;
	}

}