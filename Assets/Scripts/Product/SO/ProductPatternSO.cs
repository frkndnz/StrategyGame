using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.ProductSystem
{
    public abstract class ProductPatternSO : ScriptableObject
    {
        #region DATA

        [SerializeField]
        protected string productName;
        public string ProductName => productName;
        [SerializeField]
        protected Sprite productSprite;
        public Sprite ProductSprite => productSprite;
        [SerializeField]
        protected Vector2 cellSize;
        public Vector2 CellSize => cellSize;

        [SerializeField]
        protected List<DefaultSoldier> myProductList;
        public List<DefaultSoldier> MyProductList => myProductList;

        #endregion
        
    }
}