using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserDetector : MonoBehaviour
{
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;
    public Transform subject;
    public bool ContactBeenMade;
    public bool reUseAble=true;
    public LayerMask triggeredBy;
    
     
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void adjustments()
    {
        if (subject==null)
        {
            subject = gameObject.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
       // LevelEvents.Instance.PlayerDamageRequest(  ProjectileDirection);
    }
    
}
