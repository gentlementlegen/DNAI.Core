using System.Collections.Generic;
using UnityEngine;
using static DNAI.Astar2.Astar2;

namespace Assets.Scripts.Pacman
{
    /// <summary>
    /// Represents an astar behaviour using the DNAI script.
    /// </summary>
    public class AstarTest : MonoBehaviour
    {
        /// <summary>
        /// Position graph representing the terrain.
        /// </summary>
        private readonly PosGraph _graph = new PosGraph();

        /// <summary>
        /// Ids of the graph.
        /// </summary>
        private readonly Dictionary<int, int> _idx = new Dictionary<int, int>();

        /// <summary>
        /// List of the nodes.
        /// </summary>
        private readonly List<Position> _nodes = new List<Position>();

        /// <summary>
        /// List of teleporting spots.
        /// </summary>
        private readonly List<int> _tList = new List<int>();

        /// <summary>
        /// Gets the node index correcponding to a position.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int GetNodeIndex(Position node)
        {
            return _idx[(int)node.Y * TerrainManager.Terrain[(int)node.Y].Length + (int)node.X];
        }

        /// <summary>
        /// Builds the graph.
        /// Basically just reads the array in the TerrainManager and looks for walkable spaces.
        /// If it is walkable, adds it to the graph and links it to the other nodes.
        /// There is on special case which is the teleporting spot, handled at the end.
        /// </summary>
        private void Start()
        {
            _graph.links = new List<List<int>>();
            _graph.nodes = new List<Position>();

            for (int y = 0; y < TerrainManager.Terrain.Length; y++)
            {
                for (int x = 0; x < TerrainManager.Terrain[y].Length; x++)
                {
                    char pos = TerrainManager.Terrain[y][x];
                    if (pos == '.' || pos == 'O' || pos == ' ' || pos == 'T')
                    {
                        var posNode = new Position()
                        {
                            X = x,
                            Y = y,
                            Z = 0
                        };
                        var i = _graph.appendNode(posNode, _graph);

                        _idx.Add(y * TerrainManager.Terrain[y].Length + x, i);
                        _nodes.Add(posNode);

                        if (pos == 'T')
                            _tList.Add(i);

                        // top
                        if (pos != 'T' && TerrainManager.Terrain[y - 1][x] != 'X')
                        {

                            var j = _idx[(y - 1) * TerrainManager.Terrain[y].Length + x];

                            _graph.linkNodes(i, j, true, _graph);
                        }

                        // left
                        if (pos != 'T' && TerrainManager.Terrain[y][x - 1] != 'X')
                        {

                            var j = _idx[y * TerrainManager.Terrain[y].Length + x - 1];

                            _graph.linkNodes(i, j, true, _graph);
                        }
                    }
                }
            }
            _graph.linkNodes(_tList[0], _tList[1], true, _graph);
        }

        /// <summary>
        /// Gets the path between point from and to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<int> GetDestinationPath(Position from, Position to)
        {
            return GetDestinationPath(GetNodeIndex(from), GetNodeIndex(to));
        }

        /// <summary>
        /// Gets the path between point from and to.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<int> GetDestinationPath(int from, int to)
        {
            return _graph.pathFindAStar(from, to, _graph) as List<int>;
        }

        /// <summary>
        /// Returns a node in the graph.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Position GetNode(int idx)
        {
            return _graph.getNode(idx, _graph);
        }
    }
}