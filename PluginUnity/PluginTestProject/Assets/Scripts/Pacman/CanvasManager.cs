using System;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class CanvasManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private GameObject _panelMenu;

        [SerializeField] private GameObject _panelGame;
        [SerializeField] private GameObject _panelGameOver;

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
                    break;

                case GameManager.GameState.Pause:
                    break;

                case GameManager.GameState.End:
                    _panelGameOver.SetActive(true);
                    _panelGame.SetActive(false);
                    break;

                case GameManager.GameState.Menu:
                    _panelGame.SetActive(false);
                    _panelMenu.SetActive(true);
                    _panelGameOver.SetActive(false);
                    break;
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChange -= GameStateChanged;
        }
    }
}