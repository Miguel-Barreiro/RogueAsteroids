
using Components;

namespace Entities
{
	public class Game : IEntity
	{
		public readonly BindableProperty<int> Score = 0;

		public void Destroy() {  }
	}
}