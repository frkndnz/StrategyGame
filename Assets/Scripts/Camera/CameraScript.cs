using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{
    public static CameraScript scr;
    public Camera mainCamera;
    private void Awake() { scr = this; }
    public float zoomSpeed = 1f;
    public float minZoom = 1f;
    public float maxZoom = 10f;

    private void Update()
    {
        if (!GameManager.scr.isGame)
            return;
        Zoom();
        MoveController();
    }
    
    private void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput == 0)
            return;
        float newZoom = Camera.main.orthographicSize - scrollInput * zoomSpeed;
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        Camera.main.orthographicSize = newZoom;

        
    }
    public float cameraSpeed;
    public Vector2 cameraBoundsWidth,cameraBoundsHeight;
    private bool _isMoving=false;
    private Vector3 _lastMousePosition;
  private void MoveController()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = Input.mousePosition;
            _isMoving = true;
        }
        if(_isMoving && Input.GetMouseButton(0))
        {
            Vector2 delta = Input.mousePosition - _lastMousePosition;
            float newX = transform.position.x-delta.x * cameraSpeed * Time.deltaTime;
            float newY =transform.position.y -delta.y * cameraSpeed * Time.deltaTime;

            newX = Mathf.Clamp(newX, cameraBoundsWidth.x, cameraBoundsWidth.y);
            newY = Mathf.Clamp(newY, cameraBoundsHeight.x, cameraBoundsHeight.y);
            transform.position = new Vector3(newX, newY, transform.position.z);

            _lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isMoving = false;
        }
        
    }


    
}
