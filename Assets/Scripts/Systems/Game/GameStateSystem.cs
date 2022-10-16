using Core;
using Entities;
using Events;
using Events.UI;

namespace Systems.Game
{
	public class GameStateSystem : System, DependencyManager.IDependencyRequired, ISetupSystem
	{
		private PlayButtonEvent _playButtonEvent;
		private EntityFactory<Entities.Game> _entityFactory;
		private Entities.Game _game;
		private GameStateEvent _gameStateEvent;


		public void SetupDependencies(DependencyManager manager)
		{
			_playButtonEvent = manager.Get<PlayButtonEvent>();
			_entityFactory = manager.Get<EntityFactory<Entities.Game>>();
			_gameStateEvent = manager.Get<GameStateEvent>();
		}

		public void Setup()
		{
			_game = _entityFactory.CreateNew();
			_playButtonEvent.OnPlayButton += OnPlayButton;
		}

		private void OnPlayButton()
		{
			_gameStateEvent.TriggerGameStart();
		}
	}
}