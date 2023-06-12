using UnityEngine;
using FD.ProductSystem;
using FD.PathSystem;
using System.Collections.Generic;

public class BaseModel:MonoBehaviour
{
    public static BaseModel scr;
    private void Awake()
    {
        scr = this;
    }
    //GET DATA
    public enum ClickedType { Empty,Menu,Build,Soldier,pathNode}
    public ClickedType leftClickedType;
    public ClickedType rightClickedType;

    [Header("ProductionMenuData"),Space(10)]
    public GameObject ProductionObj;
    [Header("BuildData"), Space(10)]
    public Build currentBuild;
    public Build playerBase, enemyBase;
    [Header("SoldierData"), Space(10)]
    public GameObject soldier;
    public Soldier currentSoldier;
    public Soldier currentEnemy;
    [Header("PathNodeData"), Space(10)]
    public PathNode currentPathnode;
    public PathNode targetPathNode;

}

