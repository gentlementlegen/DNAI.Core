using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState { Play, Pause, End, Menu }

        public GameState State { get; private set; } = GameState.Menu;

        [SerializeField]
        private GameObject _playerPrefab;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                State = GameState.Play;
                Instantiate(_playerPrefab);
            }
        }
    }
}