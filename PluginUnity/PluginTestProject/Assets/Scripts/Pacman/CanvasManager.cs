using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Handler for the UI canvas.
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _panelMenu;
        [SerializeField] private GameObject _panelGame;
        [SerializeField] private GameObject _panelGameOver;
        [SerializeField] private GameObject _panelPause;

        [Header("Text")]
        [SerializeField] private Text _textBestScore;
        [SerializeField] private Text _textScoreGameOver;

        /// <summary>
        /// Subscribe to the game state event.
        /// </summary>
        private void Start()
        {
            GameManager.Instance.OnGameStateChange += GameStateChanged;
        }

        /// <summary>
        /// Updates the UI according to the current game state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameStateChanged(object sender, EventArgs e)
        {
            var ev = e as GameStateEvent;
            switch (ev.CurrentState)
            {
                case GameManager.GameState.Play:
                    _panelGame.SetActive(true);
                    _panelMenu.SetActive(false);
                    _panelGameOver.SetActive(false);
                    _panelPause.SetActive(false);
                    break;

                case GameManager.GameState.Pause:
                    _panelPause.SetActive(true);
                    _panelGame.SetActive(false);
                    break;

                case GameManager.GameState.End:
                    _panelGameOver.SetActive(true);
                    _panelGame.SetActive(false);
                    _textScoreGameOver.text = GameManager.Instance.Score.ToString();
                    break;

                case GameManager.GameState.Menu:
                    _panelMenu.SetActive(true);
                    _panelGame.SetActive(false);
                    _panelGameOver.SetActive(false);
                    _panelPause.SetActive(false);
                    _textBestScore.text = GameManager.Instance.GetBestScore().ToString();
                    break;

                case GameManager.GameState.Warmup:
                    _panelGame.SetActive(true);
                    _panelMenu.SetActive(false);
                    _panelGameOver.SetActive(false);
                    _panelPause.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// Unsubscribe to the game state event to prevent leaks.
        /// </summary>
        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChange -= GameStateChanged;
        }
    }
}