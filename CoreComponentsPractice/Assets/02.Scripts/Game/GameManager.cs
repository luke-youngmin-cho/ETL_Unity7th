using DiceGame.Data;
using DiceGame.Data.Mock;
using DiceGame.Network;
using DiceGame.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiceGame.Game
{
    public enum GameState
    {
        None,
        WaitUntilInternetConnected,
        Login,
        WaitUntilLoggedIn,
        LoadResources,
        WaitUntilResourcesLoaded,
        InLobby,
        InGameReady,
        InGamePlay,
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
                case GameState.WaitUntilInternetConnected:
                    {
                        if (InternetConnection.IsGoogleWebsiteReachable())
                        {
                            _state++;
                        }
                    }
                    break;
                case GameState.Login:
                    {
                        SceneManager.LoadScene("Login");
                        _state++;
                    }
                    break;
                case GameState.WaitUntilLoggedIn:
                    {
                        if (LoginInformation.loggedIn &&
                            LoginInformation.profile != null)
                        {
                            if (PhotonManager.instance)
                                _state++;
                        }
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
                        if (unitOfWork.isReady)
                        {
                            SceneManager.LoadScene("Lobby");
                            _state++;
                        }
                    }
                    break;
                case GameState.InLobby:
                    break;
                case GameState.InGameReady:
                    break;
                case GameState.InGamePlay:
                    break;
                default:
                    break;
            }
        }
    }
}