using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour // TODO name as Trigger
{
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;

    public int effectId;
    public bool detectionActive;
  //  public Transform subject;
    public bool ContactBeenMade;
    public bool ContactActive;
    public bool reUseAble=true;
    public LayerMask triggeredBy;
    //------------------------------------
    [SerializeField] private bool onTriggerEnter;
    [SerializeField] private bool onTriggerStay;
    [SerializeField] private bool onTriggerExit;
    [SerializeField] private float checkDistance  ;
    
     
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ContactActive = Physics.CheckSphere(transform.position, checkDistance,triggeredBy );
    }

    private void adjustments()
    {
         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!detectionActive) return;
        
        if (!onTriggerEnter) return;

        if (!reUseAble&&ContactBeenMade) return;

        if (  triggeredBy == (triggeredBy | (1 << other.gameObject.layer )))
        {
            ContactBeenMade = true;
            Vector3 temp=other.GetComponent<Rigidbody>().velocity;
            React(  temp);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!detectionActive) return;
        
        if (!onTriggerStay) return;
        
        if (!reUseAble&&ContactBeenMade) return;

        if (  triggeredBy == (triggeredBy | (1 << other.gameObject.layer )))
        {
            ContactBeenMade = true;
            Vector3 temp=other.GetComponent<Rigidbody>().velocity;
            React(  temp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!detectionActive) return;
        
        if (!onTriggerExit) return;
        
        if (!reUseAble&&ContactBeenMade) return;

        if (  triggeredBy == (triggeredBy | (1 << other.gameObject.layer )))
        {
            ContactBeenMade = true;
            Vector3 temp=other.GetComponent<Rigidbody>().velocity;
            React(  temp);
        }
    }

    private void React(Vector3 ProjectileDirection)
    {
        if (effectId==1)
        {
            
        }
        if (effectId==2)
        {
            
        }
        if (effectId==3)
        {
            
        }
        if (effectId==4)
        {
            
        }
        //TODO INSERT ACTION HERE
    }
    
}
