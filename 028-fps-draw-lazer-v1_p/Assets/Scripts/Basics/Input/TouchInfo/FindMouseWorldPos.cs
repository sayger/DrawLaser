using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMouseWorldPos : MonoBehaviour
{
    [SerializeField] private Vector3 HitPos;
    [SerializeField] private Camera camera  ;
    [SerializeField] private float searchDistance=1000;
    [SerializeField] private Vector3 cameraDirectedFinalPoint ;
    [SerializeField] private Vector3 Direction;
    
    [SerializeField] private Vector2 mouseInput;
    [SerializeField] private LayerMask HitLayers;
    
    [SerializeField] private bool useIndicator;
    [SerializeField] private Transform indicator ;
     
    void Start()
    {

        if (camera==null)
        {
            camera = Camera.main;
        }
        if (useIndicator&&indicator==null)
        {
            indicator = transform;
            
        }
    }

   
    void FixedUpdate()
    {
        mouseInput.x = Input.mousePosition.x;
        mouseInput.y = Input.mousePosition.y;

        cameraDirectedFinalPoint = camera.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, searchDistance));

        Direction = ( cameraDirectedFinalPoint - camera.transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, Direction * searchDistance, out hit, Mathf.Infinity, HitLayers))
        {
            Debug.DrawRay(transform.position, Direction * searchDistance, Color.yellow);
            if (useIndicator)
            {
                indicator.position = hit.point;
                HitPos = hit.point;
            }
            HitPos = hit.point;
        }
        else
        {
            Debug.DrawRay(transform.position, Direction * searchDistance, Color.red);
        }
    }



}
