using Components;
using Systems;

namespace Entities
{
	public sealed class Asteroid : IEntity
	{
		
		public readonly PhysicalBody PhysicalBody;
		public readonly Components.View View;

		public Asteroid()
		{
			View = ComponentFactory<Components.View>.Add();
			PhysicalBody = ComponentFactory<PhysicalBody>.Add();
		}

		public void Destroy()
		{
			ComponentFactory<Components.View>.DestroyComponent(View);
			ComponentFactory<PhysicalBody>.DestroyComponent(PhysicalBody);
		}
	}
}