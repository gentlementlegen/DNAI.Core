using System.Collections.Generic;
using UnityEngine;
using static DNAI.AstarMabit.AstarMabit;

namespace Assets.Scripts.Pacman
{
    public class AstarPacman : MonoBehaviour
    {
        private readonly PosGraph _graph = new PosGraph();
        private Dictionary<int, int> _idx = new Dictionary<int, int>();
        private readonly List<int> _tList = new List<int>();

        private void Start()
        {
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
                        if (pos == 'T')
                            _tList.Add(i);
                        // top
                        if (pos != 'T' && TerrainManager.Terrain[y - 1][x] != 'X')
                            _graph.linkNodes(i, _idx[(y - 1) * TerrainManager.Terrain[y].Length + x], true, _graph);
                        // left
                        if (pos != 'T' && TerrainManager.Terrain[y][x - 1] != 'X')
                            _graph.linkNodes(i, _idx[y * TerrainManager.Terrain[y].Length + x - 1], true, _graph);
                    }
                }
            }
            _graph.linkNodes(_tList[0], _tList[1], true, _graph);
            var l = (List<List<int>>)_graph.links;
            var n = (List<Position>)_graph.nodes;

            for (int i = 0; i < l.Count; i++)
            {
                List<int> link = l[i];
                foreach (var li in link)
                {
                    Debug.Log("[Idx => " + i + " li => " + li + "]");
                }
                Debug.Log("\n");
            }

            for (int i = 0; i < n.Count; i++)
            {
                Position node = n[i];
                Debug.Log("[idx => " + i + " node => " + node.Display() + "]");
            }
        }
    }
}