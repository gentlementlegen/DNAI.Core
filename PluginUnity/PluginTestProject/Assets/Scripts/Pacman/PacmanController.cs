using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pacman
{
    public class PacmanController : MonoBehaviour
    {
        [SerializeField]
        private float _distanceDelta = 0.1f;

        private Vector3 _target = new Vector3(1, 1, 0);

        private readonly Queue<TerrainManager.Direction> _dir = new Queue<TerrainManager.Direction>();
        private TerrainManager.Direction _currentDir = TerrainManager.Direction.Right;

        private int x = 1;
        private int y = 1;

        private void Start()
        {
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
            transform.position = Vector3.MoveTowards(transform.position, _target, _distanceDelta);
            if (Vector3.Distance(transform.position, _target) < 0.01f)
                UpdateTarget();
        }

        private void AddDirection(TerrainManager.Direction direction)
        {
            if (_dir.Count > 0)
                _dir.Dequeue();
            _dir.Enqueue(direction);
        }

        private void UpdateTarget()
        {
            if (_dir.Count <= 0)
            {
                _target = TerrainManager.Instance.GetNextAvailableNode(x, y, _currentDir, out x, out y);
                return;
            }

            var currX = x; var currY = y;
            var newTarget = TerrainManager.Instance.GetNextAvailableNode(x, y, _dir.Peek(), out x, out y);
            if (currX == x && currY == y)
            {
                newTarget = TerrainManager.Instance.GetNextAvailableNode(x, y, _currentDir, out x, out y);
            }
            else
            {
                _currentDir = _dir.Dequeue();
            }
            _target = newTarget;
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
    }
}