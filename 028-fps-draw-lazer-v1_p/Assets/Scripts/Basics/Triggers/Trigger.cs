using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;
     
    public int callAddress = 0;
    public bool InTouch;
    public bool useOnTriggerStay=false;
    public Transform subject;
    public LayerMask triggeredBy;
    public float checkingDistance;
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!useOnTriggerStay)
        {
            InTouch = checkCollisionWithLayerMask(subject,triggeredBy,checkingDistance);
        }
        
        if (InTouch)  triggerIt();
        
    }
    private bool checkCollisionWithLayerMask(Transform position,LayerMask layerMask,float distance)//
    { 
        return Physics.CheckSphere(position.position, distance, layerMask);
 
    }

    private void OnTriggerStay(Collider other)
    {
        if (useOnTriggerStay)
        {
           // Debug.Log(other.gameObject);
          //  Debug.Log((int)triggeredBy);
            if (  triggeredBy == (triggeredBy | (1 << other.gameObject.layer )))
            {
                triggerIt();
                InTouch = true;
            }
            else InTouch = false;
            
        }
    }

    private void adjustments()
    {
        if (checkingDistance<=0)
        {
            checkingDistance = transform.lossyScale.x;
        }

        if (subject==null)
        {
            subject = gameObject.transform;
        }
    }

    private void triggerIt()
    {
        if (callAddress==0)
        {
        //    LevelEvents.Instance.PanelJumpRequest(transform.position);
        }

        if (callAddress==1)
        {
         //   LevelEvents.Instance.JumpLimitationRequest();
        }
        if (callAddress==2)
        {
        //    LevelEvents.Instance.WinningJumpRequest();
        }

        if (callAddress==3)
        {
             
        }
        
    }
}
