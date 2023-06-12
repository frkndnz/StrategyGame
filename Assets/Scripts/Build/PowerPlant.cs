using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.HealthSystem;
using FD.ProductSystem;
using FD.PathSystem;
public class PowerPlant : Build
{
    [SerializeField]
    private int _initialHealthValue = 100;

    [SerializeField]
    private Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
    }
    private void Start()
    {
        PrepareProductData();
        health.InitializeHealth(_initialHealthValue);
        myFieldPathNodes = PathManager.scr.GetPathNodeOfObject(transform.position, productPattern.CellSize);
        myBoundsPathNodes= PathManager.scr.GetBoundsOfObject(transform.position, productPattern.CellSize);
        SetPathNodeList(false);
    }
    public override void PrepareProductData()
    {
        myProductDataList = productPattern.MyProductList;
    }

    public override void Production(int productIndex)
    {
        if (!CanProduce)
            return;
    }

    public override void Dead()
    {
        print("power plant is dead");
    }

    public override void GetHitFeedBack()
    {
        print("power plant feedback");
    }
    
    
    
}
