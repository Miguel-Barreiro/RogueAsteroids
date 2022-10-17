using Components;
using Core;
using Systems;
using View;

namespace Entities
{
	public sealed class Asteroid : IEntity
	{
		public readonly BindableProperty<int> Life = 100;
		public int Size = 0;
		
		public readonly PhysicalBody PhysicalBody;
		public readonly Components.View View;

		public Asteroid()
		{
			View = ComponentFactory<Components.View>.Add();
			PhysicalBody = ComponentFactory<PhysicalBody>.Add();
		}


		public void Destroy(PrefabFactory prefabFactory)
		{
			if (View.GameObject.Value != null)
				prefabFactory.Destroy(View.GameObject.Value);
			ComponentFactory<Components.View>.DestroyComponent(View);
			ComponentFactory<PhysicalBody>.DestroyComponent(PhysicalBody);
		}
	}
}