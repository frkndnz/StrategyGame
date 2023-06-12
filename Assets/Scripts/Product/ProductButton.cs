using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FD.ProductSystem;

public class ProductButton : MonoBehaviour
{
    public enum Type { build,product}
    public Type type;
    public Image productImage;
    public TMPro.TMP_Text productText;

    public DefaultBuildSO.BuildType myBuild;
    public Build build;
    public ProductPatternSO productData;
    public int productIndex;
    public void GetData(ProductPatternSO buildData,DefaultBuildSO.BuildType buildType,Type buttonType) // get data mainproduction
    {
        type = buttonType;
        productImage.sprite = buildData.ProductSprite;
        productText.text = buildData.ProductName;
        myBuild = buildType;
        productData = buildData;
    }
    public void GetData(DefaultSoldier productData, Build _build, Type buttonType) // get data products
    {
        type = buttonType;
        productImage.sprite = productData.ProductSprite;
        productText.text = productData.ProductName;
        build = _build;
        productIndex = productData.type.GetHashCode();
    }
    public void ButtonClick()
    {
        switch (type)
        {
            case Type.build:
               
                PlacementManager.scr.GetSprite(productImage.sprite,myBuild,productData.CellSize);
                break;
            case Type.product:
                build.Production(productIndex);
                break;
            default:
                break;
        }
        //myBuild.Production()
    }
}
