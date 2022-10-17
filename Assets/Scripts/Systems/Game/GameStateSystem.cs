using Core;
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
		private MainController _mainController;
		private AsteroidSpawnerSystem _asteroidSpawnerSystem;


		public void SetupDependencies(DependencyManager manager)
		{
			_playButtonEvent = manager.Get<PlayButtonEvent>();
			_entityFactory = manager.Get<EntityFactory<Entities.Game>>();
			_gameStateEvent = manager.Get<GameStateEvent>();
			_mainController = manager.Get<MainController>();
			_asteroidSpawnerSystem = manager.Get<AsteroidSpawnerSystem>();
		}

		public void Setup()
		{
			_mainController.DisableSystem(_asteroidSpawnerSystem);
			
			_game = _entityFactory.CreateNew(null);
			_playButtonEvent.OnPlayButton += OnPlayButton;
			
			_gameStateEvent.OnGameEnd += () => { _mainController.DisableSystem(_asteroidSpawnerSystem); };
			_gameStateEvent.OnGameStart += () => { _mainController.EnableSystem(_asteroidSpawnerSystem); };
		}

		private void OnPlayButton()
		{
			_gameStateEvent.TriggerGameStart();
		}
	}
}