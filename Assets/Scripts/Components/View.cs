using Core;
using UnityEngine;

namespace Components
{
	public class View
	{
		public readonly BindableProperty<GameObject> GameObject = new BindableProperty<GameObject>(null);
	}
}