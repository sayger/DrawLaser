using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDetector : MonoBehaviour
{
    public bool OnCollisionGround;
    public bool OnCollisionChar;
    [SerializeField] private LayerMask detectionLayerGround;
    [SerializeField] private LayerMask detectionLayerPlayer;
    public Transform detectedObjectChar;
    
    void Start()
    {
        
    }

     
    private void OnTriggerEnter(Collider other)
    {
        if ( detectionLayerGround == (detectionLayerGround | (1 << other.gameObject.layer)) )
        {
            OnCollisionGround = true;
        }
        

        if ( detectionLayerPlayer == (detectionLayerPlayer | (1 << other.gameObject.layer)) )
        {
            OnCollisionChar = true;
            detectedObjectChar = other.gameObject.transform;
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if ( detectionLayerGround == (detectionLayerGround | (1 << other.gameObject.layer)) )
        {
            OnCollisionGround = false;
        }
        if ( detectionLayerPlayer == (detectionLayerPlayer | (1 << other.gameObject.layer)) )
        {
            OnCollisionChar = false;
            detectedObjectChar = null;
        }
    }
}
