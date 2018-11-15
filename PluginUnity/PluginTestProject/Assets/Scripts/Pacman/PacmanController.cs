using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public delegate void OnPositionChanged(object sender, Vector2Int position);

    /// <summary>
    /// Controller for the pacman (player)
    /// </summary>
    public class PacmanController : MonoBehaviour
    {
        /// <summary>
        /// Triggered when pacman's position changed
        /// </summary>
        public event OnPositionChanged PositionChanged;

        /// <summary>
        /// The distance that pacman moves every tick
        /// </summary>
        [SerializeField]
        private float _distanceDelta = 0.1f;

        private Vector3 _target = new Vector3(1, 1, 0);

        private readonly Queue<TerrainManager.Direction> _dir = new Queue<TerrainManager.Direction>();
        private TerrainManager.Direction _currentDir = TerrainManager.Direction.Left;

        private int x = 14;
        private int y = 23;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            UpdateTarget();
        }

        /// <summary>
        /// Every frame we check for a keyboard input.
        /// If an arrow is pressed, it is added to a stack.
        /// Then, we move the player accordingly.
        /// </summary>
        private void Update()
        {
            if (GameManager.Instance.State != GameManager.GameState.Play)
                return;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AddDirection(TerrainManager.Direction.Down);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AddDirection(TerrainManager.Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AddDirection(TerrainManager.Direction.Up);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AddDirection(TerrainManager.Direction.Right);
            }
            transform.position = Vector3.MoveTowards(transform.position, _target, _distanceDelta * Time.deltaTime);
            if (Vector3.Distance(transform.position, _target) < 0.01f)
                UpdateTarget();
        }

        /// <summary>
        /// Kills the player and notifies the GameManager.
        /// </summary>
        private void KillPacman()
        {
            _currentDir = TerrainManager.Direction.Right;
            RotatePacman();
            _animator.SetTrigger("Die");
            GameManager.Instance.KillPlayer();
        }

        /// <summary>
        /// If we already queued a direction, remove it so we only remember the last one.
        /// Otherwise, add it.
        /// </summary>
        /// <param name="direction"></param>
        public void AddDirection(TerrainManager.Direction direction)
        {
            if (_dir.Count > 0)
                _dir.Dequeue();
            _dir.Enqueue(direction);
        }

        /// <summary>
        /// Updates the pacman positon.
        /// If we have no direction (ak no key pressed), just keep going the same as before.
        /// Otherwise check if we can use the new direction, and move. Triggers the event.
        /// isJump represents the "teleporting" spots on the sides of the terrain.
        /// </summary>
        private void UpdateTarget()
        {
            bool isJump;

            if (_dir.Count <= 0)
            {
                _target = TerrainManager.Instance.GetNextAvailableNode(x, y, _currentDir, out x, out y, out isJump);
                PositionChanged?.Invoke(this, new Vector2Int(x, y));
            }
            else
            {
                var currX = x; var currY = y;
                var newTarget = TerrainManager.Instance.GetNextAvailableNode(x, y, _dir.Peek(), out x, out y, out isJump);
                if (currX == x && currY == y)
                {
                    newTarget = TerrainManager.Instance.GetNextAvailableNode(x, y, _currentDir, out x, out y, out isJump);
                    PositionChanged?.Invoke(this, new Vector2Int(currX, currY));
                }
                else
                {
                    _currentDir = _dir.Dequeue();
                }
                _target = newTarget;
            }

            if (isJump)
            {
                transform.position = _target;
            }
            RotatePacman();
        }

        /// <summary>
        /// Rotates pacman face to match its current direction.
        /// </summary>
        private void RotatePacman()
        {
            switch (_currentDir)
            {
                case TerrainManager.Direction.Up:
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;

                case TerrainManager.Direction.Down:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;

                case TerrainManager.Direction.Left:
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;

                case TerrainManager.Direction.Right:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
        }

        /// <summary>
        /// Kills the poor pacman if he touches an enemy.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Enemy")
            {
                KillPacman();
            }
        }

        /// <summary>
        /// Destroys this game object.
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}