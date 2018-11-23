using UnityEngine;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Controller for the Pacman Ghosts.
    /// </summary>
    public class GhostController : MonoBehaviour
    {
        [SerializeField]
        private AstarTest _astar;

        /// <summary>
        /// The distance that the ghost can move every frame.
        /// </summary>
        [SerializeField]
        private float _distanceDelta = 0.1f;

        /// <summary>
        /// The ghost target (the player)
        /// </summary>
        private GameObject _target;

        /// <summary>
        /// The ghost position target.
        /// </summary>
        private DNAI.Astar2.Astar2.Position _targetPos = new DNAI.Astar2.Astar2.Position();

        /// <summary>
        /// The ghost's current position.
        /// </summary>
        private DNAI.Astar2.Astar2.Position _currPos = new DNAI.Astar2.Astar2.Position();

        /// <summary>
        /// Sets the ghost starting positon.
        /// </summary>
        private void Start()
        {
            _currPos.X = 1;
            _currPos.Y = 1;
            _targetPos.X = 1;
            _targetPos.Y = 1;
        }

        /// <summary>
        /// Updates the ghost's position.
        /// </summary>
        private void Update()
        {
            if (_target == null)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
                if (_target != null)
                {
                    _target.GetComponent<PacmanController>().PositionChanged += GhostController_PositionChanged;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position,
                TerrainManager.Instance.GetWorldPosition((int)_targetPos.X, (int)_targetPos.Y),
                _distanceDelta * Time.deltaTime);
        }

        /// <summary>
        /// Triggered every time the ghost changes position. Gets a new destination with the astar method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="position"></param>
        private void GhostController_PositionChanged(object sender, Vector2 position)
        {
            _currPos = TerrainManager.Instance.GetGridPosition(transform.position.x, transform.position.y).AsPosition();
            var targetGridPos = TerrainManager.Instance.GetGridPosition(_target.transform.position.x, _target.transform.position.y).AsPosition();
            var nodes = _astar.GetDestinationPath(_currPos, targetGridPos);

            if (nodes.Count > 1)
                _targetPos = _astar.GetNode(nodes[1]);
        }
    }
}