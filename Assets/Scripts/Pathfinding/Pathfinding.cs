using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FD.PathSystem
{
    public class Pathfinding : MonoBehaviour
    {
        public Tilemap tileMap;
        public List<PathNode> Path;
        public List<TileBase> tileList = new List<TileBase>();
        public List<GameObject> nodeList;

        public PathNode seeker, target;
        void Start()
        {
            foreach (var pos in tileMap.cellBounds.allPositionsWithin)
            {
                var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

                if (!tileMap.HasTile(localPlace)) continue;
                var tile = tileMap.GetTile(localPlace);
                tileMap.GetInstantiatedObject(localPlace);
                GameObject obj = tileMap.GetInstantiatedObject(localPlace); 
                nodeList.Add(obj);
                tileList.Add(tile);
            }

            SetLogic();
        }
        [SerializeField]
        public PathNode[,] grid;
        private void SetLogic()
        {
            grid = new PathNode[tileMap.size.x, tileMap.size.y];
            int index=0;
            for (int y = 0; y < tileMap.size.y; y++)
            {
                for (int x = 0; x < tileMap.size.x; x++)
                {
                    PathNode currentNode = nodeList[index].GetComponent<PathNode>();
                    currentNode.Initialize(x, y);
                    grid[x, y] = currentNode;
                    index++;
                }
            }
           
        }
        public bool pathfindingActive;
        private void Update()
        {
            if (pathfindingActive)
            {
                FindPath(seeker, target);
            }
        }
        public List<PathNode> FindPath(PathNode _startNode,PathNode _endNode)
        {
            
            PathNode startNode = _startNode;
            PathNode targetNode = _endNode;

            List<PathNode> openSet = new List<PathNode>();
            HashSet<PathNode> closedSet = new HashSet<PathNode>();

            openSet.Add(startNode);
            
            while (openSet.Count>0)
            {
                PathNode currentPathNode = openSet[0];
                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentPathNode.fCost || openSet[i].fCost==currentPathNode.fCost && openSet[i].hCost <currentPathNode.hCost)
                    {
                        currentPathNode = openSet[i];
                    }
                }
                openSet.Remove(currentPathNode);
                closedSet.Add(currentPathNode);
               

                if (currentPathNode == targetNode)
                {
                    
                    return RetracePath(startNode, targetNode);
                    
                }

                testList = GetNeighbours(currentPathNode);
                foreach (PathNode neighbour in GetNeighbours(currentPathNode))
                {
                    if (closedSet.Contains(neighbour) || !neighbour.walkable)
                    {
                        continue;

                    }
                    
                    int newMovementCostToNeighbour = currentPathNode.gCost + GetDistance(currentPathNode, neighbour);
                    if(newMovementCostToNeighbour<neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parentPathNode = currentPathNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        
                    }
                }
            }
            return null;
        }
        public List<PathNode> testList;
        public List<PathNode> GetNeighbours(PathNode node)
        {
            List<PathNode> neighbours = new List<PathNode>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.x + x;
                    int checkY = node.y + y;
                    if (checkX >= 0 && checkX < tileMap.size.x && checkY >= 0 && checkY < tileMap.size.y)
                    {
                        neighbours.Add(grid[checkX,checkY]);
                    }
                }
            }
            return  neighbours;
        }

        List<PathNode> RetracePath(PathNode startNode,PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            PathNode currentNode = endNode;
            while (currentNode!=startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parentPathNode;
            }
            path.Reverse();
            Path = path;
            return path;
            DrawPath();
        }
        int GetDistance(PathNode nodeA,PathNode nodeB)
        {
            int dstX = Mathf.Abs(nodeA.x - nodeB.x);
            int dstY = Mathf.Abs(nodeA.y - nodeB.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }

        private void DrawPath()
        {
            for (int i = 0; i < Path.Count; i++)
            {
                Path[i].SetColor(Color.black);
            }

        }
    }
}