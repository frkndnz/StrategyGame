using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.HealthSystem;
using FD.PathSystem;
using FD.ProductSystem;
using UnityEngine.UI;

public class Base : Build
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
        yield return new WaitForSeconds(0.5f);
        PrepareProductData();
        health.InitializeHealth(_initialHealthValue);
        myFieldPathNodes = PathManager.scr.GetPathNodeOfObject(transform.position, productPattern.CellSize);
        myBoundsPathNodes = PathManager.scr.GetBoundsOfObject(transform.position, productPattern.CellSize);
        SetPathNodeList(false);
    }
    public override void Dead()
    {
        gameObject.SetActive(false);
        if (type == Type.player)
        {
            GameManager.scr.WinControl(false);
        }
        else if(type==Type.enemy)
        {
            GameManager.scr.WinControl(true);
        }
    }

    public override void GetHitFeedBack()
    {
        StopAllCoroutines();
        StartCoroutine(HealthRoutione());
    }
    public Slider healthSlider;
    
    private IEnumerator HealthRoutione()
    {
        float targetVal =(float) health.currentHealth / (float)_initialHealthValue;
        if (healthSlider.value > targetVal)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetVal-0.01f , 4f * Time.deltaTime);
            yield return null;
        }
        //print(targetVal);
        healthSlider.value = targetVal;
    }
    public override void PrepareProductData()
    {
       
    }

    public override void Production(int productIndex)
    {
        
    }
}
