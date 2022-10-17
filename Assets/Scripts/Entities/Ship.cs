using Components;
using Core;
using Systems;
using View;

namespace Entities
{
	public class Ship : IEntity
	{
		public readonly BindableProperty<int> Lifes = 0;
		
		public readonly PhysicalBody PhysicalBody = new PhysicalBody();
		public readonly Components.View View = new Components.View();

		public void RegisterComponents()
		{
			ComponentFactory<Components.View>.Add(View);
			ComponentFactory<PhysicalBody>.Add(PhysicalBody);
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