using FD.ProductSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FD.HealthSystem;
using FD.PathSystem;

public class Barrack : Build
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
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern.CellSize);
        SetPathNodeList(false);
    }
    public override void PrepareProductData()
    {
        myProductDataList = productPattern.MyProductList;
    }

    public override void Production(int productIndex)
    {
        GameObject obj = PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.soldier);
        obj.transform.position = productPosition.position;
        obj.GetComponent<Soldier>().GetPatternData(productIndex);
        obj.GetComponent<Soldier>()._isDead = false;
        obj.SetActive(true);
    }

    
    
    


    public override void Dead()
    {
        SetPathNodeList(true);
        gameObject.SetActive(false);
    }
    public override void GetHitFeedBack()
    {
       
    }
   
}
