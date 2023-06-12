using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public LayerMask layer;
    public bool areaFull;
    public BoxCollider2D myCollider;
    public SpriteRenderer objRenderer;
    public GameObject highlightRed;
    
    private void Start()
    {
        highlightRed.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Simple;
        
    }
    private void Update()
    {
        areaFull = myCollider.IsTouchingLayers(layer) ;
        highlightRed.SetActive(areaFull);
    }
}
