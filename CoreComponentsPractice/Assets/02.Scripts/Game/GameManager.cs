using DiceGame.Data;
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
                    break;
                case GameState.WaitUntilResourcesLoaded:
                    break;
                case GameState.InGame:
                    break;
                default:
                    break;
            }
        }
    }
}