using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager scr;
    private void Awake()
    {
        scr = this;
    }
    public bool placementActive;
    public Tilemap gameTileMap;
    public GameObject obj;
    public Vector2 objSize;
    public Scanner scanner;
    private void Update()
    {
        if (!placementActive)
            return;
        PlacementControl();
    }
    private FD.ProductSystem.DefaultBuildSO.BuildType buildType;
    public void GetSprite(Sprite sprite,FD.ProductSystem.DefaultBuildSO.BuildType type,Vector2 size) 
    {
        scanner.objRenderer.sprite = sprite;
        placementActive = true;
        StartCoroutine(SetMousePosition());
        buildType = type;
        objSize = size;
        scanner.myCollider.size = new Vector2(objSize.x * 0.32f, objSize.y * 0.32f);
        scanner.highlightRed.transform.localScale = new Vector3(size.x,size.y,1);
    }
    private IEnumerator SetMousePosition()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(0.01f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void PlacementControl() 
    {
        scanner.gameObject.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = gameTileMap.WorldToCell(mousePos);

        Vector3 sizeCalculate;
        sizeCalculate.x= objSize.x %2==0 ? 0.16f:0;
        sizeCalculate.y = objSize.y % 2 == 0 ? 0.16f : 0;
        sizeCalculate.z = 0;
        Vector3 position = gameTileMap.GetCellCenterWorld(cellPosition)+sizeCalculate;
        position.z = -2;
        obj.transform.position = position;
        if (!scanner.areaFull)
        {
            if (_isPlaced)
                PlaceBuild(position, buildType);

        }

    }
    private bool _isPlaced;
    public void ClickedPlace()
    {
        if (!placementActive)
            return;
        _isPlaced = true;
    }
    public void ClosedPlace()
    {
        if (!placementActive)
            return;
        placementActive = false;
        scanner.gameObject.SetActive(false);
    }
    private void PlaceBuild(Vector3 pos, FD.ProductSystem.DefaultBuildSO.BuildType type) 
    {
        switch (type)
        {
            case FD.ProductSystem.DefaultBuildSO.BuildType.barrack:
                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.barrack).transform.position=pos;
                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.barrack).SetActive(true);
               
                break;
            case FD.ProductSystem.DefaultBuildSO.BuildType.powerplant:
               
                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.powerplant).transform.position = pos;
                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.powerplant).SetActive(true);
                break;
            case FD.ProductSystem.DefaultBuildSO.BuildType.tower:

                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.tower).transform.position = pos;
                PoolManager.scr.GetObjectFromPool(PoolManager.Pool.Type.tower).SetActive(true);
                break;
            default:
                break;
        }
        scanner.gameObject.SetActive(false);
        placementActive = false;
        _isPlaced = false;
    }
}
