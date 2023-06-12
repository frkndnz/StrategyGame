using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FD.ProductSystem;
using FD.PathSystem;
using UnityEngine.EventSystems;

public class BaseView : MonoBehaviour
{

    public static BaseView scr;
    public InformationMenu informationMenu;
    public EndPanel endPanel;
    private void Awake()
    {
        scr = this;
    }
    

    [System.Serializable]
    public class InformationMenu
    {
        public GameObject informationPanel;
        public Image buildImage;
        public TMP_Text buildName;
        public Image productImage;
        public void UpdateInformationMenu(bool active)
        {
            informationPanel.SetActive(active);
            if (!active)
                return;
            buildImage.sprite = BaseModel.scr.currentBuild.productPattern.ProductSprite;
            buildName.text = BaseModel.scr.currentBuild.productPattern.ProductName;
            if (BaseModel.scr.currentBuild.CanProduce)
                productImage.sprite = BaseModel.scr.currentBuild.myProductDataList[0].ProductSprite;
        }

    }
    [System.Serializable]
    public class EndPanel
    {
        public GameObject endPanel;
        public TMP_Text winText;

        public void EndPanelActive(bool win)
        {
            winText.text = win ? "VICTORY" : "LOSE";
            endPanel.SetActive(true);
        }
    }
    [System.Serializable]
    public class ProductionMenu
    {
        //production menu properties
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //mouse left clicked
        {
            RaycastControl(true);
        }
        else if (Input.GetMouseButtonDown(1)) // mouse right clicked
        {
            RaycastControl(false);
        }

    }

    public LayerMask raycastLayer;
    private void RaycastControl(bool left)
    {
        Component _component=new Component();
        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction,10,raycastLayer);
        BaseModel.ClickedType curClickType=BaseModel.ClickedType.Empty;

        if (EventSystem.current.IsPointerOverGameObject()) // clicked Menu
        {
            curClickType = BaseModel.ClickedType.Menu;  

        }
        else if (hit.collider != null ) // clicked gameBoard object
        {
            if (hit.collider.GetComponent<Build>()) // Click Build
            {
                _component = hit.collider.GetComponent<Build>();
                curClickType = BaseModel.ClickedType.Build;
            }
            else if(hit.collider.GetComponent<Soldier>())
            {
                _component = hit.collider.GetComponent<Soldier>();
                curClickType = BaseModel.ClickedType.Soldier;
               
            }
            else if (hit.collider.GetComponent<PathNode>())
            {
                _component = hit.collider.GetComponent<PathNode>();
                curClickType = BaseModel.ClickedType.pathNode;
            }
            
            
        }
        else // clicked Empty
        {
            curClickType = BaseModel.ClickedType.Empty;
        }

        if (left)
        {
            BaseController.scr.SelectedLeftClickControl(curClickType,_component);
        }
        else
        {
            BaseController.scr.SelectedRightClickControl(curClickType,_component);
        }

    }
}
