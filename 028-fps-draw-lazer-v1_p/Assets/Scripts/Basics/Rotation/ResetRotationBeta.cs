using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotationBeta : MonoBehaviour
{
    [SerializeField] private bool work=true;
     
    
    [SerializeField] private Transform subject;
    
    [SerializeField] private Transform resetReference;
    
    
    [SerializeField] private Transform angleReference;
    
    [SerializeField] private float maxAngle=100;
    [SerializeField] private float currentAngle;

    void Start()
    {
        if (subject==null)
        {
            subject = transform;
        }

        if (resetReference==null||angleReference==null)
        {
            work = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (work)
        {
            currentAngle = Vector3.Angle(subject.up, angleReference.up);
            
            
            
            if (currentAngle>maxAngle)
            {
                subject.rotation = resetReference.rotation;
            }
        }
    }
}
