using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLaserHit : MonoBehaviour
{
    [SerializeField] private bool Working=true;
    public Camera SaidCamera;
    public RectTransform SaidCursor;
    public Transform SubjectObject;
    [SerializeField] private LayerMask layerMask;
    private FindLazerHit lazerHit;
    
    void Start()
    {
        lazerHit = FindObjectOfType<FindLazerHit>();
    }

    void FixedUpdate()
    {
        if (Working && !lazerHit.drawing)
        {
            Ray ray = SaidCamera.ScreenPointToRay(SaidCursor.position);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(SaidCamera.transform.position, ray.direction * hit.distance, Color.yellow);
              //  Debug.Log("Did Hit");
                SubjectObject.position = hit.point;
            }
        }
    }

    
}
