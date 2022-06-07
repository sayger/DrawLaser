using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lazerTest : MonoBehaviour
{
    [SerializeField] private bool active=true;
    [SerializeField] private LayerMask layerMask;
    [Range(0, 1)] [SerializeField]
    private float IndicatorSphereSize=0.25f;
    
    
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (active)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            Gizmos.DrawSphere(hit.point,IndicatorSphereSize);
            
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            
        }
      
         
        

         


    }
}
