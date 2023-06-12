using UnityEngine;
using FD.ProductSystem;
using FD.PathSystem;

public class BaseController:MonoBehaviour
{
    public static BaseController scr;
    private void Awake() { scr = this; }

    

    public void SendBuildToInfoPanel() //SendBuildToInfoPanel
    {
        if (!BaseModel.scr.currentBuild.CanProduce)
            return;
        BaseView.scr.informationMenu.UpdateInformationMenu(true);
        ProductionManager.scr.currentBuildProducts = BaseModel.scr.currentBuild.myProductDataList;
        ProductionManager.scr.LoadMenu(ProductionManager.ProductType.buildProduction);
    }

    private BaseModel.ClickedType _lastClickedType=BaseModel.ClickedType.Empty;
    public void SelectedLeftClickControl(BaseModel.ClickedType type,  Component component=null)
    {
        switch (type)
        {
            case BaseModel.ClickedType.Empty:

                ResetModelData(BaseModel.ClickedType.Build);
                ResetModelData(BaseModel.ClickedType.Soldier);
                PlacementManager.scr.ClickedPlace();
                break;
            case BaseModel.ClickedType.Menu:

                break;
            case BaseModel.ClickedType.Build:

                BaseModel.scr.currentBuild = (Build)component;
                if(BaseModel.scr.currentBuild.type==Build.Type.player)
                    SendBuildToInfoPanel();
                break;
            case BaseModel.ClickedType.Soldier:
                Soldier currentSoldier = (Soldier)component;
                if (currentSoldier.type == Soldier.Type.player)
                {
                    ResetModelData(BaseModel.ClickedType.Build);
                    BaseView.scr.informationMenu.UpdateInformationMenu(false);
                    ProductionManager.scr.LoadMenu(ProductionManager.ProductType.mainProduction);
                    if (BaseModel.scr.currentSoldier!=null && BaseModel.scr.currentSoldier != (Soldier)component)
                    {
                        BaseModel.scr.currentSoldier.Sethighlight(false);
                    }
                    BaseModel.scr.currentSoldier = (Soldier)component;
                    BaseModel.scr.currentSoldier.Sethighlight(true);
                    SendNodeToSoldier();

                }

                break;
            case BaseModel.ClickedType.pathNode:
                ResetModelData(BaseModel.ClickedType.Build);
                ResetModelData(BaseModel.ClickedType.Soldier);
                PlacementManager.scr.ClickedPlace();
                break;
            default:
                break;
        }
        _lastClickedType = type;
        BaseModel.scr.leftClickedType = type;
    }
    public void SelectedRightClickControl(BaseModel.ClickedType type, Component component = null)
    {
        PlacementManager.scr.ClosedPlace();
        switch (type)
        {
            case BaseModel.ClickedType.Empty:

                break;
            case BaseModel.ClickedType.Menu:

                break;
            case BaseModel.ClickedType.Build:
                BaseModel.scr.currentBuild =(Build)component;
                if (BaseModel.scr.leftClickedType == BaseModel.ClickedType.Soldier && BaseModel.scr.currentBuild.type==Build.Type.enemy && !BaseModel.scr.currentSoldier._isDead)
                    BaseModel.scr.currentSoldier.FindNearestBound(BaseModel.scr.currentBuild.myBoundsPathNodes);
                break;
            case BaseModel.ClickedType.Soldier:
                BaseModel.scr.currentEnemy = (Soldier)component;
                if (BaseModel.scr.leftClickedType == BaseModel.ClickedType.Soldier && BaseModel.scr.currentEnemy.type == Soldier.Type.enemy)
                {
                    if (!BaseModel.scr.currentEnemy._isMove && !BaseModel.scr.currentSoldier._isDead)
                    {
                        BaseModel.scr.currentSoldier.FindNearestBound(BaseModel.scr.currentEnemy.myBoundsPathNodes);

                    }
                    else
                    {

                    }
                   
                }
                    break;
            case BaseModel.ClickedType.pathNode:
                if(BaseModel.scr.leftClickedType==BaseModel.ClickedType.Soldier && !BaseModel.scr.currentSoldier._isDead)
                    BaseModel.scr.currentSoldier.GetPath();
                break;
            default:
                break;
        }
        _lastClickedType = type;
        BaseModel.scr.rightClickedType = type;
    }
    private void ResetModelData(BaseModel.ClickedType type)
    {
        switch (type)
        {
            case BaseModel.ClickedType.Empty:
                break;
            case BaseModel.ClickedType.Menu:
                break;
            case BaseModel.ClickedType.Build:
                if(BaseModel.scr.currentBuild)
                    BaseModel.scr.currentBuild = null;
                BaseView.scr.informationMenu.UpdateInformationMenu(false);
                ProductionManager.scr.LoadMenu(ProductionManager.ProductType.mainProduction);
                break;
            case BaseModel.ClickedType.Soldier:
                if (BaseModel.scr.currentSoldier)
                {
                    BaseModel.scr.currentSoldier.Sethighlight(false);
                    BaseModel.scr.currentSoldier = null;

                }
                break;
            default:
                break;
        }
    }

    private void SendNodeToSoldier()
    {
        BaseModel.scr.currentPathnode = PathManager.scr.SelectPathNode();
        BaseModel.scr.currentSoldier.myCurrentNode = BaseModel.scr.currentPathnode;
    }
}

