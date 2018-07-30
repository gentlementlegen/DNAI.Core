using Assets.Scripts.Pacman;
using System.Collections.Generic;
using UnityEngine;
using static DNAI.Astar2.Astar2;

public class AstarTest : MonoBehaviour
{
	private readonly PosGraph _graph = new PosGraph();
    private Dictionary<int, int> _idx = new Dictionary<int, int>();
    private List<Position> _nodes = new List<Position>();
    private readonly List<int> _tList = new List<int>();

    /*[SerializeField]
    private PacmanController pacman;

    [SerializeField]
    private Transform target;*/

    private List<int> path;

    private Position currPosition = null;

    private int GetNodeIndex(Position node)
    {
        return _idx[(int)node.Y * TerrainManager.Terrain[(int)node.Y].Length + (int)node.X];
    }

    private void Start()
    {
	  	_graph.links = new List<List<int>> ();
		_graph.nodes = new List<Position> ();

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

                    //Debug.Log("Adding node (" + x.ToString() + ", " + y.ToString() + "): index(" + i.ToString() + ") => LinksLength: " + ((List<List<int>>)_graph.links).Count);

                    _idx.Add(y * TerrainManager.Terrain[y].Length + x, i);
                    _nodes.Add(posNode);

                    if (pos == 'T')
                        _tList.Add(i);
                    
                    // top
                    if (pos != 'T' && TerrainManager.Terrain [y - 1] [x] != 'X') {

                        var j = _idx[(y - 1) * TerrainManager.Terrain[y].Length + x];
                                                
						_graph.linkNodes (i, j, true, _graph);
                    }

                    // left
					if (pos != 'T' && TerrainManager.Terrain [y] [x - 1] != 'X') {

                        var j = _idx[y * TerrainManager.Terrain[y].Length + x - 1];

                        _graph.linkNodes (i, j, true, _graph);
                    }
                }
            }
        }
        _graph.linkNodes(_tList[0], _tList[1], true, _graph);
        
        //path = (List<int>)_graph.pathFindAStar(0, 256, _graph);

        //currPosition = _nodes[path[0]];

        //foreach (int curr in path)
        //{
        //    Debug.Log("=> " + _nodes[curr].Display());
        //}

        /*var l = (List<List<int>>)_graph.links;
        var n = (List<Position>)_graph.nodes;

        for (int i = 0; i < l.Count; i++)
        {
            List<int> link = l[i];
            foreach (var li in link)
            {
                Debug.Log("[Idx => " + i + " li => " + li + "]");
            }
        }

        for (int i = 0; i < n.Count; i++)
        {
            Position node = n[i];
            Debug.Log("[idx => " + i + " node => " + node.Display() + "]");
        }*/
    }

    public List<int> GetDestinationPath(Position from, Position to)
    {
        return GetDestinationPath(GetNodeIndex(from), GetNodeIndex(to));
    }

    public List<int> GetDestinationPath(int from, int to)
    {
        return _graph.pathFindAStar(from, to, _graph) as List<int>;
    }

    public Position GetNode(int idx)
    {
        return _graph.getNode(idx, _graph);
    }

    /*public void Update()
    {
        if (curr)
    }*/

    private IEnumerable<Position> GetNextPosition()
    {
        foreach (int currPos in path)
        {
            yield return _nodes[currPos];
        }
    }
}