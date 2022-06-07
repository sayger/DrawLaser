using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimicRotation : MonoBehaviour
{
    [SerializeField] private bool effectOnlyWİthTouch;
    [SerializeField] private bool touchActive;
    [SerializeField] private GameObject referenceObject ;
    [SerializeField] private GameObject subjectObject ;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private Vector3 currentTargetRotation;
    [SerializeField] private bool useLerpElseMoveTowards ;
    [SerializeField] private bool copyX ;
    [SerializeField] private bool copyY ;
    [SerializeField] private bool copyZ ;
    [SerializeField] private bool copyLocalData;
    [SerializeField] private bool pasteToLocalData;

    [SerializeField] private bool test;
     


  //  [SerializeField] private float packageOfTheA;

  //  [SerializeField] private float lastGate ;
    
    
    
    
    void Start()
    {
        if (subjectObject==null)
        {
            subjectObject = gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (effectOnlyWİthTouch)
        {
            if (Input.GetMouseButton(0))
            {
                touchActive = true;
                 
                AllProcess();
            }
            else
            {
                touchActive =  false;
            }
        }
        else
        {
            AllProcess();
        }
         
    }

    private void AllProcess()
    {
        /*
       // subjectObject.transform.localRotation = referenceObject.transform.localRotation  ;
        Quaternion testUseless = referenceObject.transform.localRotation;
        testUseless = new Quaternion(0, testUseless.y, 0, testUseless.w);
        subjectObject.transform.localRotation = referenceObject.transform.localRotation  ;
        /**/
        targetRotation = copyLocalData ? referenceObject.transform.localRotation.eulerAngles : referenceObject.transform.rotation.eulerAngles;
        Vector3 currentRotation = pasteToLocalData ?  subjectObject.transform.localRotation.eulerAngles : subjectObject.transform.rotation.eulerAngles;

        if (!copyX)
        {
            targetRotation.x = currentRotation.x;
        }
        if (!copyY)
        {
            targetRotation.y = currentRotation.y;
        }
        if (!copyZ)
        {
            targetRotation.z = currentRotation.z;
        }
        
        
        currentTargetRotation = useLerpElseMoveTowards
            ? Vector3.Lerp(currentRotation, targetRotation, transitionSpeed/10)
            : Vector3.MoveTowards(currentRotation, targetRotation, transitionSpeed);

      /*  if (Math.Abs(currentTargetRotation.y-lastGate) >359.8)
        {
            Debug.Log(" 360 passed");
            if (currentTargetRotation.y<lastGate)
            {
                packageOfTheA += 360;
            }
            else if (currentTargetRotation.y>lastGate)
            {
                
            }
            {
                packageOfTheA -= 360;
            }
        }

        lastGate = currentTargetRotation.y;*/
        if (test)
        {
           // currentTargetRotation.y += packageOfTheA;
           if (Math.Abs(currentTargetRotation.y -targetRotation.y)>10)
           {
               currentTargetRotation.y = targetRotation.y;
           }
        }
        if (pasteToLocalData)
        {
            subjectObject.transform.localRotation = Quaternion.Euler(currentTargetRotation);
        }
        else
        {
            subjectObject.transform.rotation = Quaternion.Euler(currentTargetRotation);
        }

    }
}
