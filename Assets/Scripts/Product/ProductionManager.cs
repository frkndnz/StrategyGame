using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FD.ProductSystem;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager scr;
    private void Awake()
    {
        scr = this;
    }
    public enum ProductType { mainProduction,buildProduction}
    public List<DefaultBuildSO> buildPatternSO;
    public List<DefaultSoldier> currentBuildProducts=new List<DefaultSoldier>();
    public List<ProductButton> buttonList;
    private void Start()
    {
        LoadMenu(ProductType.mainProduction);
    }
    public void LoadMenu(ProductType type)
    {
        int counter = 0;
        if (type == ProductType.mainProduction)
        {
            for (int i = 0; i < 10; i++)
            {
                buttonList[i].GetData(buildPatternSO[counter], buildPatternSO[counter].type, ProductButton.Type.build);
                counter++;
                counter = counter % buildPatternSO.Count == 0 ? 0 : counter;

            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                buttonList[i].GetData(currentBuildProducts[counter],BaseModel.scr.currentBuild,ProductButton.Type.product);
                counter++;
                counter = counter % currentBuildProducts.Count == 0 ? 0 : counter;
            }
        }
    }

}

