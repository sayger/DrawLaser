using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class CameraFocusPositioner : MonoBehaviour//TODO MAKE IT BETTER THEN UPDATE 000 REPO
{
    [SerializeField] private Transform FollowObject;
    [SerializeField] private Vector3 TargetPos;
    [SerializeField] private bool followingNow;
    
    [SerializeField] private bool lerpElseMoveTowards;
    [SerializeField] private float followSpeed=1;
    [Range(0, 100)]
    [SerializeField] private float minFollowSpeedPercentage=10;
    private float minFollowSpeed;
     private float followSpeedOriginalSpeed;
    
    [SerializeField] private bool xAxisFollow=true; //TODO add custom axis
    [SerializeField] private bool yAxisFollow;
    [SerializeField] private bool zAxisFollow=true;
    [SerializeField] private bool reCenterIt=true;
        
    [SerializeField] private float offset ;
    [SerializeField] private bool getSceneOffset=false;
    

    [SerializeField] private bool autoSetMinFollowDistance =true ;
    [SerializeField] private Transform followDistanceReference ;
    
    [SerializeField] private float minFollowDistance;
    [SerializeField] private float resultDistance;
    [SerializeField] private float currentDistance;
    [SerializeField] private float speedMaxReachDistance;
   // [SerializeField] private float speedChangeSpeed;
    
  
    [SerializeField] private bool useExtraLimiter; //TODO THIS IS HALF ASS SHIT
    [SerializeField] private position_DirectionBasedLimiter limiter;
     
    [Header(" characterSpeed Based Follow Speed  ")]
    [SerializeField] private bool playerInputBasedSpeed;

    [SerializeField] private bool ConsiderTargetCCSpeed; //TODO DELETE LATER
    [SerializeField] private CCMovement TargetCCMovement; //TODO DELETE LATER
    [SerializeField] private bool ConsiderTargetNavmeshSpeed; //TODO DELETE LATER
    [SerializeField] private NavMeshAgent targetAgent;
    
    
   // [SerializeField] private bool GetAwayFromObject; TODO MAKE IT GIVE PRIORITY BETWEEN FOLLOW AND GET AWAY
  

 
 //  [SerializeField] private bool onlyForThisGame3; //TODO DELETE LATER
 //  [SerializeField] private RailManager onlyForThisGame4; //TODO DELETE LATER
  // [SerializeField] private Vector3 offsetRecord; //TODO DELETE LATER
  // [SerializeField] private float offsetExtra; //TODO DELETE LATER
 
  
    void Start()
    {
         
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        variableUpdate();
        if (followingNow)
        {
            followMove();
        }

     /*   if (onlyForThisGame3)
        {
            Vector3 temp = new Vector3(0, 0, offsetExtra);
            if (onlyForThisGame4.placingMode)
            {
                offset = offsetRecord + temp;
            }
            else
            {
                offset = offsetRecord ;
            }

            
        }*/
    }

    private void adjustments()
    {
        if (reCenterIt)
        {
            Vector3 startPos = transform.position;
            if (xAxisFollow)
            {
                startPos.x = FollowObject.position.x;
            }
            if (yAxisFollow)
            {
                startPos.y = FollowObject.position.y;
            }
            if (zAxisFollow)
            {
                startPos.z = FollowObject.position.z;
            }

            transform.position = startPos;
        }
        if (getSceneOffset)
        {
            offset = distanceCalculation(transform ,FollowObject ,xAxisFollow,yAxisFollow,zAxisFollow);  
             
        }
        
        if (autoSetMinFollowDistance)
        {
            if (followDistanceReference==null)
            {
                followDistanceReference = transform.GetChild(0);
//                Debug.Log(" first child taken as reference");
            }
            if (followDistanceReference==null)
            {
                autoSetMinFollowDistance = false;
                Debug.Log(" couldn't find reference  ");
                 
            }
            else
            {
                minFollowDistance = followDistanceReference.localScale.x/2; 
            }
            
        }

        followSpeedOriginalSpeed = followSpeed;

        if (TargetCCMovement==null)
        {
            TargetCCMovement = FollowObject.GetComponent<CCMovement>();
        }

        if (TargetCCMovement==null)
        {
            ConsiderTargetCCSpeed = false;
        }

        if (targetAgent==null)
        {
            ConsiderTargetNavmeshSpeed = false;
        }

        if (ConsiderTargetCCSpeed)
        {
            ConsiderTargetNavmeshSpeed = false;
        }

        if (playerInputBasedSpeed)
        {
            if (InputEvents.Instance == null)
            { 
                playerInputBasedSpeed = false;
                Debug.Log(" NO INPUT EVENT FIND  ");
                /* InputEvents temp =gameObject.AddComponent<InputEvents>();
                 temp.awakeFunctions();*/
            }
        }

        
        
        minFollowSpeed = (followSpeed / 100) * minFollowSpeedPercentage;

        if (speedMaxReachDistance<minFollowDistance)
        {
            speedMaxReachDistance = minFollowDistance * 2;
        }
    }

    private float distanceCalculation(Transform first, Transform Second,
        bool calculateX = true, bool calculateY = true, bool calculateZ=true)
    {
         
        Vector3 target = first.position;
        if (calculateX)
        {
             target.x = Second.position.x;
        }
        if (calculateY)
        {
            target.y = Second.position.y;
        }
        if (calculateZ)
        {
            target.z = Second.position.z;
        }

        return Vector3.Distance(first.position, target);
    }
    private void variableUpdate()
    {
        
 
        currentDistance =  distanceCalculation(transform ,FollowObject ,xAxisFollow,yAxisFollow,zAxisFollow);
        resultDistance = currentDistance - offset;
        resultDistance = resultDistance < 0 ? 0 : resultDistance;
        
        if ((resultDistance > minFollowDistance))
        {
            
            followingNow = true;
            
        }
        if ((resultDistance < (minFollowDistance)-(getPercentage(minFollowDistance, 20) ) )) // important for prevent glitch
        {
            followingNow = false;
             
        }

        if (useExtraLimiter&followingNow&&!limiter.ValidateAcceptance())
        {
            followingNow = false;
        }
 
        
    }
    private void followMove()
    {
        var position = transform.position;
        TargetPos = position;
        if (xAxisFollow)
        {
            TargetPos.x = FollowObject.position.x;
        }
        if (yAxisFollow)
        {
            TargetPos.y = FollowObject.position.y;
        }
        if (zAxisFollow)
        {
            TargetPos.z = FollowObject.position.z;
        }

        float remainingSpeed = getPercentage(followSpeedOriginalSpeed, minFollowSpeedPercentage, true);
        if (playerInputBasedSpeed)
        {
            followSpeed =minFollowSpeed + (remainingSpeed) *( InputEvents.Instance.getCurrentPercentage()/100);
            
        }
        else if (ConsiderTargetCCSpeed || ConsiderTargetNavmeshSpeed)
        {
           // followSpeed = minFollowSpeed+ ( remainingSpeed)*( Math.Abs(TargetCCMovement._TheVelocity.magnitude/100));
          //  followSpeed =   TargetCCMovement._TheVelocity.magnitude  ;

          Vector3 tempMagnitude = new Vector3();

          Vector3 movementDirection =new Vector3() ;
          float movementMagnitude  =0;
          
          if (ConsiderTargetCCSpeed)
          {
              Vector3 receivedVelocity = TargetCCMovement._TheVelocity;
              if (!xAxisFollow)
              {
                  receivedVelocity.x = 0;
              }
              if (!yAxisFollow)
              {
                  receivedVelocity.y = 0;
              }
              if (!zAxisFollow)
              {
                  receivedVelocity.z = 0;
              }
                movementDirection = receivedVelocity.normalized;
               movementMagnitude = receivedVelocity.magnitude;
          }
         
          if (ConsiderTargetNavmeshSpeed)
          {
              var desiredVelocity = targetAgent.desiredVelocity;
              movementDirection = desiredVelocity.normalized;
              movementMagnitude = desiredVelocity.magnitude;
          }
          
          
          
          Vector3 directionHeading = FollowObject.position - transform.position;
          float angle = Vector3.Angle(movementDirection, directionHeading.normalized);
          float speedTemp = movementMagnitude -
                            (movementMagnitude * (angle / 180));

         // speedTemp /= 100;
        //  speedTemp *= 110;

         
          if (speedTemp<minFollowSpeed)
          {
              
              if ((resultDistance > speedMaxReachDistance ))
              {
                  speedTemp = followSpeedOriginalSpeed;
              }
              else if((resultDistance < (minFollowDistance)  ))
              {
                  speedTemp = minFollowSpeed;
              }
              else
              {
                  float temp = resultDistance - minFollowDistance;
                  temp = temp / speedMaxReachDistance;
                  speedTemp = minFollowSpeed + ((followSpeedOriginalSpeed - minFollowSpeed) * temp);
              }
          }
          
          followSpeed =  speedTemp   ;
         
        }
        else
        {
            if ((resultDistance > speedMaxReachDistance ))
            {
                followSpeed = followSpeedOriginalSpeed;
            }
            else if((resultDistance < (minFollowDistance)  ))
            {
                followSpeed = minFollowSpeed;
            }
            else
            {
                float temp = resultDistance - minFollowDistance;
                temp = temp / speedMaxReachDistance;
                followSpeed = minFollowSpeed + ((followSpeedOriginalSpeed - minFollowSpeed) * temp);
            }
        }

         
        
        float activeSpeed=followSpeed *  Time.deltaTime;
       
       
        Vector3 currentTarget = lerpElseMoveTowards
            ? Vector3.Lerp(position, TargetPos, (activeSpeed/10)  )
            : Vector3.MoveTowards(position, TargetPos, (activeSpeed)  );
        
        /*TODO This is the one supposed to word wtf
           Vector3 currentTarget = lerpElseMoveTowards
            ? Vector3.Lerp(position, TargetPos, (followSpeed/10)*Time.deltaTime )
            : Vector3.MoveTowards(position, TargetPos, (followSpeed)*Time.deltaTime );
         */
        
        position = currentTarget;
        transform.position = position;
         
    }

    private float getPercentage(float value, float percentage , bool reverseOver100=false)
    {
        float result = (value / 100) * percentage;
        if (reverseOver100)
        {
            result = (value / 100) * (100-percentage);
        }
        return result;
    }
}
