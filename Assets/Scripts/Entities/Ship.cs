using Components;
using Systems;

namespace Entities
{
	public class Ship : IEntity
	{
		public readonly PhysicalBody PhysicalBody;
		public readonly Components.View View;

		public Ship()
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