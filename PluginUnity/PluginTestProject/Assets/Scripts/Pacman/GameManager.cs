using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Event info givng the current game state and the previous one.
    /// </summary>
    public class GameStateEvent : EventArgs
    {
        public GameManager.GameState CurrentState;
        public GameManager.GameState PreviousState;
    }

    /// <summary>
    /// Maanger for the Pacman game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton for the manager.
        /// </summary>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Represent the different possible states of the game.
        /// </summary>
        public enum GameState { Play, Pause, End, Menu, Warmup, Death }

        /// <summary>
        /// Holds the current state of the game.
        /// </summary>
        public GameState State { get; private set; } = GameState.Menu;

        /// <summary>
        /// Triggered when the game state changes.
        /// </summary>
        public event EventHandler OnGameStateChange;

        /// <summary>
        /// The player's score.
        /// </summary>
        public int Score { get; private set; }

        [Header("UI")]
        [SerializeField]
        private Text _textScore;

        [Header("Prefabs")]
        [SerializeField]
        private GameObject _playerPrefab;

        private GameObject _playerInstance;

        /// <summary>
        /// Singleton assignment.
        /// </summary>
        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            SetGameState(GameState.Menu);
        }

        /// <summary>
        /// Reacts the the enter button to start / end the game,
        /// and escape for pause.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (State == GameState.Menu)
                {
                    StartCoroutine(StartRoundRoutine());
                    _playerInstance = Instantiate(_playerPrefab, TerrainManager.Instance.GetWorldPosition(14, 23), Quaternion.identity);
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

        /// <summary>
        /// Warmup time, with the music.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartRoundRoutine()
        {
            SetGameState(GameState.Warmup);
            yield return new WaitForSeconds(4.5f);
            SetGameState(GameState.Play);
        }

        /// <summary>
        /// Pauses / unpauses the game.
        /// </summary>
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

        /// <summary>
        /// Adds score.
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            Score += score;
            _textScore.text = Score.ToString();
        }

        /// <summary>
        /// Sets the game to end state, destroys the player and saves the score.
        /// </summary>
        public void OnPlayerWin()
        {
            SoundManager.Instance.StopSounds();
            SetGameState(GameState.End);
            Destroy(_playerInstance);
            SaveBestScore();
            _playerInstance = null;
            Score = 0;
        }

        /// <summary>
        /// Sets the new game state and triggers the event.
        /// </summary>
        /// <param name="state"></param>
        private void SetGameState(GameState state)
        {
            OnGameStateChange?.Invoke(this, new GameStateEvent { CurrentState = state, PreviousState = State });
            State = state;
        }

        public int GetBestScore()
        {
            return PlayerPrefs.GetInt("BestScore", 0);
        }

        private void SaveBestScore()
        {
            if (Score > GetBestScore())
                PlayerPrefs.SetInt("BestScore", Score);
        }

        internal void KillPlayer()
        {
            SetGameState(GameState.Death);
            SoundManager.Instance.PlayDeathSound();
        }
    }
}