using Core;
using View;

namespace Entities
{
	public class Game : IEntity
	{
		public readonly BindableProperty<int> Score = 0;

		public void Destroy(PrefabFactory prefabFactory) {  }
	}
}