using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.HealthSystem;
using FD.PathSystem;
using FD.ProductSystem;
public class Tower : Build
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
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public override void Dead()
    {
        gameObject.SetActive(false);
    }

    public override void GetHitFeedBack()
    {
    }

    public override void PrepareProductData()
    {
    }

    public override void Production(int productIndex)
    {
    }
    public List<Animator> effectList;
    bool _isAttack;
    CircleCollider2D circleCollider;
    private IEnumerator Attack()
    {
        _isAttack = true;
        circleCollider.enabled = true;
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].Play("electric", 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetHit(5, gameObject);
        }
        circleCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        _isAttack = false;
    }
    public List<Health> enemyList;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            if (!enemyList.Contains(collision.GetComponent<Health>()))
            {
                enemyList.Add(collision.GetComponent<Health>());
            }
           
        }
        if(!_isAttack)
            StartCoroutine(Attack());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyList.Contains(collision.GetComponent<Health>()))
        {
            enemyList.Remove(collision.GetComponent<Health>());
        }
    }
}
