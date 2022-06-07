using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimicPosition : MonoBehaviour
{
    public GameObject referenceObject ;
    [SerializeField] private GameObject subjectObject ;
    
    public float transitionSpeed;
    [SerializeField] private bool useLerpElseMoveTowards ;
    
    [SerializeField] private Vector3 finalTargetPosition;
    [SerializeField] private Vector3 currentTargetPosition;
    
    [SerializeField] private bool copyX ;
    [SerializeField] private bool copyY ;
    [SerializeField] private bool copyZ ;
    
    [SerializeField] private bool copyLocalData;
    [SerializeField] private bool pasteToLocalData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AllProcess();
    }
     private void AllProcess()
    { 
        finalTargetPosition = copyLocalData ? referenceObject.transform.localPosition : referenceObject.transform.position;
        Vector3 currentRotation = pasteToLocalData ?  subjectObject.transform.localPosition : subjectObject.transform.position;

        if (!copyX)
        {
            finalTargetPosition.x = currentRotation.x;
        }
        if (!copyY)
        {
            finalTargetPosition.y = currentRotation.y;
        }
        if (!copyZ)
        {
            finalTargetPosition.z = currentRotation.z;
        }
        
        
        currentTargetPosition = useLerpElseMoveTowards
            ? Vector3.Lerp(currentRotation, finalTargetPosition, (transitionSpeed/10)*Time.deltaTime)
            : Vector3.MoveTowards(currentRotation, finalTargetPosition, transitionSpeed*Time.deltaTime);
  
        if (pasteToLocalData)
        {
            subjectObject.transform.localPosition =  currentTargetPosition ;
        }
        else
        {
            subjectObject.transform.position =  currentTargetPosition ;
        }

    }
}
