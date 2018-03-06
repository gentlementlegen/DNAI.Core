using System;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class CanvasManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _panelMenu;
        [SerializeField] private GameObject _panelGame;
        [SerializeField] private GameObject _panelGameOver;
        [SerializeField] private GameObject _panelPause;

        private void Start()
        {
            GameManager.Instance.OnGameStateChange += GameStateChanged;
        }

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
                    break;

                case GameManager.GameState.Menu:
                    _panelMenu.SetActive(true);
                    _panelGame.SetActive(false);
                    _panelGameOver.SetActive(false);
                    _panelPause.SetActive(false);
                    break;
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChange -= GameStateChanged;
        }
    }
}