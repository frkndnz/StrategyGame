using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FD.PathSystem
{
    public class PathNode : MonoBehaviour
    {
        [SerializeField]
        public int x;
        [SerializeField]
        public int y;
        public PathNode parentPathNode;
        public bool walkable=true;
        public int gCost;
        public int hCost;
        public int fCost
        {
            get { return gCost + hCost; }
        }

        public void Initialize(int x, int y)
        {
            this.x = x;
            this.y = y;

        }
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}