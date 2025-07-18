
namespace Assets.Scripts.GameAppControl
{
    public class GameTrigger
    {
        private static GameTrigger _instance;
        public static GameTrigger Instance => _instance ??= new GameTrigger();

        private GameManager _gameManager;

        public void Init(GameManager manager)
        {
            _gameManager = manager;
        }

        public void SetTrigger(AppTriger trigger)
        {
            if (_gameManager != null)
            {
                _gameManager.Trigger(trigger);
            }
        }
    }
}
