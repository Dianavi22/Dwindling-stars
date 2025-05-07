using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabFlower : MonoBehaviour
{
    public Image crosshair; // Image UI servant de ligne de mire
    private Camera mainCamera;
    public float maxDistance = 100f;
    private LayerMask targetLayer = ~0;
    private int count = 0;
    public int maxFlowers = 2;
    public GameObject porte;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }



    
    void Update()
    {
        checkNbFlowers();
        CheckTargetInSight();
    }

    void checkNbFlowers()
    {
        if(count == maxFlowers)
        {
            porte.SetActive(true);
        }
    }


    void CheckTargetInSight()
    {
        
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
       
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
        {
           
            if (hit.collider.gameObject.CompareTag("Flower"))
            {
                
                crosshair.color = Color.red;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    count++;

                    Destroy(hit.collider.gameObject);
                    crosshair.color = Color.white;
                }
            }
            else
            {
                crosshair.color = Color.white;
            }

        }
        
    }
}
