using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.PathSystem
{
    public class PathManager : MonoBehaviour
    {
        public static PathManager scr;
        public Pathfinding pathfinding;
        private void Awake()
        {
            scr = this;
        }
        public PathNode SelectPathNode()
        {
            PathNode pathnode;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = PlacementManager.scr.gameTileMap.WorldToCell(mousePos);

            //var tile = PlacementManager.scr.gameTileMap.GetTile(cellPosition);
            if (PlacementManager.scr.gameTileMap.GetInstantiatedObject(cellPosition))
            {
               pathnode= PlacementManager.scr.gameTileMap.GetInstantiatedObject(cellPosition).GetComponent<PathNode>();

            }
            else
            {
                pathnode = null;
                print("pathnode can not found");
            }
            return pathnode;
        }
        public PathNode GetPathNode(Vector3 pos)
        {
            PathNode pathnode;

           // Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = PlacementManager.scr.gameTileMap.WorldToCell(pos);

            //var tile = PlacementManager.scr.gameTileMap.GetTile(cellPosition);
            if (PlacementManager.scr.gameTileMap.GetInstantiatedObject(cellPosition))
            {
                pathnode = PlacementManager.scr.gameTileMap.GetInstantiatedObject(cellPosition).GetComponent<PathNode>();

            }
            else
            {
                pathnode = null;
                print("pathnode can not found");
            }
            return pathnode;
        }
        public LayerMask raycastLayer;
        public List<PathNode> GetPathNodeOfObject(Vector3 objPosition,Vector2 gridSize)
        {
            Vector2 direction = Vector2.zero;
            List<PathNode> nodeList = new List<PathNode>();
            int gridWidth = (int)gridSize.x; 
            int gridHeight = (int)gridSize.y; ; 
            float cellSize = 0.32f; 
            Vector3 objectCenter = objPosition;
            float startX = objectCenter.x - ((gridWidth - 1) * cellSize) / 2f;
            float startY = objectCenter.y - ((gridHeight - 1) * cellSize) / 2f;
            float startZ = objectCenter.z;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 cellCenter = new Vector3(
                        startX + x * cellSize,
                        startY + y * cellSize,
                        startZ);
                    RaycastHit2D hit = Physics2D.Raycast(cellCenter, direction,10f, raycastLayer);
                    if (hit.collider.GetComponent<PathNode>())
                    {
                        if (!nodeList.Contains(hit.collider.GetComponent<PathNode>()))
                        {
                            nodeList.Add(hit.collider.GetComponent<PathNode>());
                        }
                    }
                }
            }
            return nodeList;
        }
        public List<PathNode> GetBoundsOfObject(Vector3 objPosition, Vector2 gridSize)
        {
            Vector2 direction = Vector2.zero;
            List<PathNode> nodeList = new List<PathNode>();
            gridSize += Vector2.one * 2;
            int gridWidth = (int)gridSize.x;
            int gridHeight = (int)gridSize.y; ;
            float cellSize = 0.32f;
            Vector3 objectCenter = objPosition;
            float startX = objectCenter.x - ((gridWidth - 1) * cellSize) / 2f;
            float startY = objectCenter.y - ((gridHeight - 1) * cellSize) / 2f;
            float startZ = objectCenter.z;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 cellCenter = new Vector3(
                        startX + x * cellSize,
                        startY + y * cellSize,
                        startZ);
                    RaycastHit2D hit = Physics2D.Raycast(cellCenter, direction, 10f, raycastLayer);
                    if(x==0 || y==0 || y==gridHeight-1 || x == gridWidth - 1)
                    {
                        if (hit.collider.GetComponent<PathNode>())
                        {
                            if (!nodeList.Contains(hit.collider.GetComponent<PathNode>()))
                            {
                                nodeList.Add(hit.collider.GetComponent<PathNode>());
                            }
                        }

                    }
                }
            }
            return nodeList;
        }
    }
}