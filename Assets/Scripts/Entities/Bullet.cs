using Components;
using Systems;

namespace Entities
{
	public class Bullet : IEntity
	{
		public readonly PhysicalBody PhysicalBody;
		public readonly Components.View View;

		public Bullet()
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