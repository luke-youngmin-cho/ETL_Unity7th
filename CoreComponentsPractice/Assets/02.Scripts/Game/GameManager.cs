using DiceGame.Data;
using DiceGame.Data.Mock;
using DiceGame.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiceGame.Game
{
    public enum GameState
    {
        None,
        Login,
        WaitUntilLoggedIn,
        LoadResources,
        WaitUntilResourcesLoaded,
        InGame,
    }


    public class GameManager : SingletonMonoBase<GameManager>
    {
        [field: SerializeField] public bool isTesting { get; private set; }
        public IUnitOfWork unitOfWork { get; private set; }

        public GameState state
        {
            get => _state;
            set
            {
                if (value == _state)
                    return;

                _state = value;
            }
        }
        [SerializeField] GameState _state;

        override protected void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Workflow();   
        }

        private void Workflow()
        {
            switch (_state)
            {
                case GameState.None:
                    break;
                case GameState.Login:
                    {
                        SceneManager.LoadScene("Login");
                        _state++;
                    }
                    break;
                case GameState.WaitUntilLoggedIn:
                    {
                        if (LoginInformation.loggedIn)
                            _state++;
                    }
                    break;
                case GameState.LoadResources:
                    {
                        if (isTesting)
                            unitOfWork = new MockUnitOfWork();
                        else
                            unitOfWork = new UnitOfWork();

                        _state++;
                    }
                    break;
                case GameState.WaitUntilResourcesLoaded:
                    {
                        SceneManager.LoadScene("DicePlay");
                        _state++;
                    }
                    break;
                case GameState.InGame:
                    break;
                default:
                    break;
            }
        }
    }
}