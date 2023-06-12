using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FD.ProductSystem
{

    [CreateAssetMenu( menuName = "Products/Build")]
    public class DefaultBuildSO : ProductPatternSO
    {
       public enum BuildType { barrack,powerplant,tower}
       public BuildType type;
    }
}