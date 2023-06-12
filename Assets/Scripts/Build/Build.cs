using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.ProductSystem
{
    public abstract class Build : MonoBehaviour
    {
        [SerializeField]
        public ProductPatternSO productPattern;
        [SerializeField]
        protected BoxCollider2D buildCollider;
        [SerializeField]
        protected SpriteRenderer buildRenderer;
        public enum Type { player,enemy}
        public Type type;
        public List<GameObject> myProductList;
        public List<DefaultSoldier> myProductDataList;
        public bool CanProduce;
        public Transform productPosition;
        public List<PathSystem.PathNode> myFieldPathNodes,myBoundsPathNodes;

#if UNITY_EDITOR
        private void OnValidate()
        {
            GetBuildData();
        }
#endif
        protected void GetBuildData()
        {
            buildRenderer.sprite = productPattern.ProductSprite;
            buildCollider.size = productPattern.CellSize*0.32f;
        }

        public abstract void PrepareProductData();
        public abstract void Production(int productIndex);

        public abstract void Dead();

        public abstract void GetHitFeedBack();
        public void SetPathNodeList(bool walkable)
        {
            for (int i = 0; i < myFieldPathNodes.Count; i++)
            {
                myFieldPathNodes[i].walkable = walkable;
            }
        }

    }

}