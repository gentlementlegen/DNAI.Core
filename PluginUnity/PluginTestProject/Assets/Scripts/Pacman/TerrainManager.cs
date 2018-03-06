using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class TerrainManager : MonoBehaviour
    {
        public static TerrainManager Instance { get; private set; }

        public static readonly string[] Terrain = new string[31]
        {
            "XXXXXXXXXXXXXXXXXXXXXXXXXXXX",
            "X............XX............X",
            "X.XXXX.XXXXX.XX.XXXXX.XXXX.X",
            "XOXXXX.XXXXX.XX.XXXXX.XXXXOX",
            "X.XXXX.XXXXX.XX.XXXXX.XXXX.X",
            "X..........................X",
            "X.XXXX.XX.XXXXXXXX.XX.XXXX.X",
            "X.XXXX.XX.XXXXXXXX.XX.XXXX.X",
            "X......XX....XX....XX......X",
            "XXXXXX.XXXXX XX XXXXX.XXXXXX",
            "     X.XXXXX XX XXXXX.X     ",
            "     X.XX          XX.X     ",
            "     X.XX XXXXXXXX XX.X     ",
            "XXXXXX.XX X      X XX.XXXXXX",
            "      .   X      X   .      ",
            "XXXXXX.XX X      X XX.XXXXXX",
            "     X.XX XXXXXXXX XX.X     ",
            "     X.XX          XX.X     ",
            "     X.XX XXXXXXXX XX.X     ",
            "XXXXXX.XX XXXXXXXX XX.XXXXXX",
            "X............XX............X",
            "X.XXXX.XXXXX.XX.XXXXX.XXXX.X",
            "X.XXXX.XXXXX.XX.XXXXX.XXXX.X",
            "XO..XX.......  .......XX..OX",
            "XXX.XX.XX.XXXXXXXX.XX.XX.XXX",
            "XXX.XX.XX.XXXXXXXX.XX.XX.XXX",
            "X......XX....XX....XX......X",
            "X.XXXXXXXXXX.XX.XXXXXXXXXX.X",
            "X.XXXXXXXXXX.XX.XXXXXXXXXX.X",
            "X..........................X",
            "XXXXXXXXXXXXXXXXXXXXXXXXXXXX"
        };

        public enum Direction
        {
            Up, Down, Left, Right
        }

        [SerializeField]
        private float _scaleFactor = 1;

        [SerializeField]
        private GameObject _prefabPacgum;

        [SerializeField]
        private GameObject _prefabPowerUp;

        private float _remainingGums;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.OnGameStateChange += GameStateChanged;
        }

        private void GameStateChanged(object sender, System.EventArgs e)
        {
            var ev = e as GameStateEvent;

            if (ev.CurrentState == GameManager.GameState.Play && ev.PreviousState == GameManager.GameState.Menu)
            {
                for (int y = 0; y < Terrain.Length; y++)
                {
                    for (int x = 0; x < Terrain[y].Length; x++)
                    {
                        switch (Terrain[y][x])
                        {
                            case '.':
                                Instantiate(_prefabPacgum, new Vector3(x * _scaleFactor, -y * _scaleFactor), Quaternion.identity);
                                ++_remainingGums;
                                break;

                            case 'O':
                                Instantiate(_prefabPowerUp, new Vector3(x * _scaleFactor, -y * _scaleFactor), Quaternion.identity);
                                ++_remainingGums;
                                break;
                        }
                    }
                }
            }
        }

        public Vector3 GetNextAvailableNode(int x, int y, Direction direction, out int newX, out int newY)
        {
            var res = new Vector3(x, y, 0);
            newX = x;
            newY = y;
            if (x <= 0 || y <= 0 || y >= Terrain.Length || x >= Terrain[y].Length)
                return res * _scaleFactor;
            switch (direction)
            {
                case Direction.Up:
                    if (Terrain[y - 1][x] != 'X')
                        newY--;
                    break;
                case Direction.Down:
                    if (Terrain[y + 1][x] != 'X')
                        newY++;
                    break;
                case Direction.Left:
                    if (Terrain[y][x - 1] != 'X')
                        newX--;
                    break;
                case Direction.Right:
                    if (Terrain[y][x + 1] != 'X')
                        newX++;
                    break;
            }
            res.x = newX;
            res.y = -newY;
            return res * _scaleFactor;
        }

        public Vector2Int GetGridPosition(float x, float y)
        {
            return new Vector2Int((int)(x / _scaleFactor), (int)(y / _scaleFactor));
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, -y) * _scaleFactor;
        }

        public void OnGumEaten()
        {
            --_remainingGums;
            if (_remainingGums <= 0)
            {
                GameManager.Instance.OnPlayerWin();
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChange -= GameStateChanged;
        }
    }
}