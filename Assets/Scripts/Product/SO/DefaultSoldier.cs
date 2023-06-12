using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FD.ProductSystem
{
    [CreateAssetMenu(menuName = "Products/Soldier")]
    public class DefaultSoldier : ProductPatternSO
    {
        public RuntimeAnimatorController animation_controller;
        public int damage;
        public int health;
        public float movementSpeed;
        public enum Type { type1, type2,type3,type4,type5 };
        public Type type;
    }
}