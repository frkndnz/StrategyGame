using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.ProductSystem;
using FD.HealthSystem;
using FD.PathSystem;

public class EnemyBarrack : Build
{
    [SerializeField]
    private int _initialHealthValue = 100;

    [SerializeField]
    private Health health;
    private void Awake()
    {
        health = GetComponent<Health>();
    }
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        PrepareProductData();
        health.InitializeHealth(_initialHealthValue);
        myFieldPathNodes = PathManager.scr.GetPathNodeOfObject(transform.position, productPattern.CellSize);
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern.CellSize);
        SetPathNodeList(false);
        StartCoroutine(ProductionRoutine());
    }
    public override void Dead()
    {
        SetPathNodeList(true);
        buildRenderer.enabled = false;
        buildCollider.enabled = false;
        gameObject.SetActive(false);
    }

    public override void GetHitFeedBack()
    {
        
    }

    public override void PrepareProductData()
    {
        
    }
    IEnumerator ProductionRoutine()
    {
        while (health.currentHealth>0)
        {
            Production(Random.Range(0, 2));
            yield return new WaitForSeconds(Random.Range(5f,15f));
        }
    }
    public override void Production(int productIndex)
    {
        GameObject obj = PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.enemysoldier);
        obj.transform.position = productPosition.position;
        obj.GetComponent<Soldier>().GetPatternData(productIndex);
        obj.GetComponent<Soldier>()._isDead = false;
        obj.SetActive(true);
        obj.GetComponent<Soldier>().myCurrentNode = PathManager.scr.GetPathNode(obj.transform.position);
        obj.GetComponent<Soldier>().FindNearestBound(BaseModel.scr.playerBase.myBoundsPathNodes);
    }
}
