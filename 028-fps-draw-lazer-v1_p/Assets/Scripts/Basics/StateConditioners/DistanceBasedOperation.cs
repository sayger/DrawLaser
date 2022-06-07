using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DistanceBasedOperation : MonoBehaviour
{
     
    
    
    [SerializeField] private float _currentPercentage;
    [SerializeField] private float _delayedPercentage;
    [SerializeField] private Transform objectA;
    [SerializeField] private Transform objectB;
    [SerializeField] private float currentDistance;
    [SerializeField] private bool takeMinAuto;
    
    [SerializeField] private float minDistanceForEffect=50;
    [SerializeField] private float effectHappenInDistance=60;
    [SerializeField] private float delayCatchSpeed = 1;
    [SerializeField] private bool useLerpElseMoveTowards;

    [SerializeField] private bool thisGameOnly;
    
    
    [SerializeField] private bool takeDistanceX;
    [SerializeField] private bool takeDistanceY;
    [SerializeField] private bool takeDistanceZ;

    [Header(" Lock Settings  ")]
    public bool lockPercentage ;

    [SerializeField] private float lockedPercentage;
    
    [SerializeField] private bool useTransitionAfterLock ;
    [SerializeField] private bool transitionActive ;
    [SerializeField] private float transitionSpeed ;
    [SerializeField] private bool lockStart;


    [SerializeField] private bool maxDistanceRegisterLimit;
    [SerializeField] private float maxDistance;
    
    

    
   // [SerializeField] private TYPE _type;

   [SerializeField] private bool findOperationDistance ;
   [SerializeField] private Transform  operationEndTarget;
   void Start()
    {
        adjustments();
    }

    private void adjustments()
    {
        if (takeMinAuto)
        {
            Vector3 target = objectA.position;
            if (takeDistanceX) target.x = objectB.position.x;
            if (takeDistanceY) target.y = objectB.position.y;
            if (takeDistanceZ) target.z = objectB.position.z;

            currentDistance = (Vector3.Distance(objectA.position, target));

            minDistanceForEffect = currentDistance;
        }
        //-----------------------------------------------
        if (findOperationDistance)
        {
            Vector3 target = objectB.position;
            if (takeDistanceX) target.x = operationEndTarget.position.x;
            if (takeDistanceY) target.y = operationEndTarget.position.y;
            if (takeDistanceZ) target.z = operationEndTarget.position.z;

            effectHappenInDistance = (Vector3.Distance(objectB.position, target));

             
            
              
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        variableUpdate();
        calculateDelayed();
    }

    private void variableUpdate()
    {
        Vector3 target = objectA.position;
        if (takeDistanceX) target.x = objectB.position.x;
        if (takeDistanceY) target.y = objectB.position.y;
        if (takeDistanceZ) target.z = objectB.position.z;

         
        if (!lockPercentage)
        {
            currentDistance = (Vector3.Distance(objectA.position, target));
            if (thisGameOnly)
            {
                currentDistance = objectB.position.z - objectA.position.z;;
                if (currentDistance<0)
                {
                    currentDistance = 0;
                }
            
            }

            if (lockStart&&useTransitionAfterLock)
            {
                transitionActive = true;
                Vector3 targetDistance = new Vector3(currentDistance, 0, 0);
                Vector3 lockedDistance = new Vector3(lockedPercentage, 0, 0);
                Vector3 now = Vector3.MoveTowards(lockedDistance, targetDistance, (transitionSpeed * Time.deltaTime));
                lockedPercentage = now.x;
                
                
                if (Math.Abs(lockedPercentage - currentDistance) < 0.1)
                {
                    lockStart = false;
                    transitionActive = false;
                }
                currentDistance = lockedPercentage;
            }
            
        }
        else
        {
            if (useTransitionAfterLock)
            {
                if (!lockStart)
                {
                    lockedPercentage = currentDistance;
                }

                lockStart = true;

            }
           

        }

        if (maxDistanceRegisterLimit)
        {
            if (currentDistance>maxDistance)
            {
                currentDistance = maxDistance;
            }
        }

        

        _currentPercentage = ((currentDistance - minDistanceForEffect) / effectHappenInDistance) * 100;
            
            
        if (currentDistance>minDistanceForEffect+effectHappenInDistance)
        {
            _currentPercentage = 100;
        }
        else  if (currentDistance<minDistanceForEffect)
        {
            _currentPercentage = 0;
        }
    }

    private void calculateDelayed()
    {
        Vector3 subject = new Vector3(_delayedPercentage, 0, 0);
        Vector3 currentTarget = new Vector3(_currentPercentage, 0, 0);
        
        
        
        Vector3 currentEffective = useLerpElseMoveTowards
            ? Vector3.Lerp(subject, currentTarget, delayCatchSpeed/10)
            : Vector3.MoveTowards(subject, currentTarget, delayCatchSpeed);

        _delayedPercentage = currentEffective.x;
    }
    public float currentPercentage()
    {
        return _currentPercentage;
    }
    public float delayedPercentage()
    {
        return _delayedPercentage;
    }
}
