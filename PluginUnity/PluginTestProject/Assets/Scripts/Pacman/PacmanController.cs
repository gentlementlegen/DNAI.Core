using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public delegate void OnPositionChanged(object sender, Vector2Int position);

    public class PacmanController : MonoBehaviour
    {
        public event OnPositionChanged PositionChanged;

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

            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    KillPacman();
            //}
        }

        private void KillPacman()
        {
            _currentDir = TerrainManager.Direction.Right;
            RotatePacman();
            _animator.SetTrigger("Die");
            GameManager.Instance.KillPlayer();
        }

        public void AddDirection(TerrainManager.Direction direction)
        {
            if (_dir.Count > 0)
                _dir.Dequeue();
            _dir.Enqueue(direction);
        }

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Enemy")
            {
                KillPacman();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}