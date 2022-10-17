using View;

namespace Core
{
	public interface IEntity
	{
		public void RegisterComponents();
		public void Destroy(PrefabFactory prefabFactory);
	}
}