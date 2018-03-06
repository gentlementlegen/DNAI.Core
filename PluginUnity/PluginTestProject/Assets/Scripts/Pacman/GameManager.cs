using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Pacman
{
    public class GameStateEvent : EventArgs
    {
        public GameManager.GameState CurrentState;
        public GameManager.GameState PreviousState;
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState { Play, Pause, End, Menu }

        public GameState State { get; private set; } = GameState.Menu;

        public event EventHandler OnGameStateChange;

        public int Score { get; private set; }

        [Header("UI")]
        [SerializeField]
        private Text _textScore;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _playerPrefab;
        private GameObject _playerInstance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetGameState(GameState.Menu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (State == GameState.Menu)
                {
                    SetGameState(GameState.Play);
                    _playerInstance = Instantiate(_playerPrefab, TerrainManager.Instance.GetWorldPosition(1, 1), Quaternion.identity);
                }
                else if (State == GameState.End)
                {
                    SetGameState(GameState.Menu);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            if (State == GameState.Menu || State == GameState.End)
                return;
            if (State == GameState.Play)
            {
                Time.timeScale = 0;
                SetGameState(GameState.Pause);
            }
            else if (State == GameState.Pause)
            {
                Time.timeScale = 1;
                SetGameState(GameState.Play);
            }
        }

        public void AddScore(int score)
        {
            Score += score;
            _textScore.text = Score.ToString();
        }

        public void OnPlayerWin()
        {
            SetGameState(GameState.End);
            Destroy(_playerInstance);
            _playerInstance = null;
            Score = 0;
        }

        private void SetGameState(GameState state)
        {
            OnGameStateChange?.Invoke(this, new GameStateEvent { CurrentState = state, PreviousState = State });
            State = state;
        }
    }
}