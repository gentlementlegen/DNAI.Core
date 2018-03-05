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
                SetGameState(GameState.Play);
                Instantiate(_playerPrefab, TerrainManager.Instance.GetWorldPosition(1, 1), Quaternion.identity);
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
        }

        private void SetGameState(GameState state)
        {
            OnGameStateChange?.Invoke(this, new GameStateEvent { CurrentState = state, PreviousState = State });
            State = state;
        }
    }
}