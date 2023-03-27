using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwap : MonoBehaviour
{
    public GameObject newGamObj;
    public LayerMask ground;
    private GameObject hitObj;
    public int numOfTiles;
    public int moveCounter;
    public GameObject tarObj;
    public Tilemap tilemap;


    private void Start()
    {
        
       
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, 30f, ground))
        {
            if(hit.transform.gameObject != hitObj )
            {
                if(hit.transform.gameObject.name != "groundBlockRounded")
                {
                    moveCounter--;
                }
                
                hitObj = hit.transform.gameObject;
                if (hit.transform.gameObject.name == "groundBlockRounded")
                {
                    Instantiate(newGamObj, hitObj.transform.position, hitObj.transform.rotation);
                    hitObj.SetActive(false);
                }
            }
            
            
            
        }
    }
    
}
