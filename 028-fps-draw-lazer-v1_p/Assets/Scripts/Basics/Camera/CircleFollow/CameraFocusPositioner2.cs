using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraFocusPositioner2 : MonoBehaviour
{
    [SerializeField] private bool followingNow;
    [SerializeField] private Vector3 TargetPos;
    [SerializeField] private Transform FollowObject;
    [SerializeField] private bool lerpElseMoveTowards;
    [Range(0, 100)]
    [SerializeField] private float followSpeed=1;
    [SerializeField] private bool xAxisFollow=true; //TODO add custom axis
    [SerializeField] private bool yAxisFollow;
    [SerializeField] private bool zAxisFollow=true;
    [SerializeField] private bool reCenterIt=true;
    [SerializeField] private float currentDistance;
    [SerializeField] private float minFollowDistance;
    [SerializeField] private float followStopDistance;
    [SerializeField] private NavMeshAgent targetAgent;
    [SerializeField] private bool autoSetMinFollowDistance =true ;
    [SerializeField] private Transform followDistanceReference ;
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentDistance = CalculateDistanceNoY(transform, FollowObject);

        if (currentDistance>minFollowDistance)
        {
            followingNow = true;
        }

        if (currentDistance<followStopDistance)
        {
            followingNow = false;
        }

        if (followingNow)
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
            
            Vector3 movementDirection =new Vector3() ;
            float movementMagnitude  =0;
            
            var desiredVelocity = targetAgent.desiredVelocity;
            
            movementDirection = desiredVelocity.normalized;
            movementMagnitude = desiredVelocity.magnitude;

            Vector3 moveTo = transform.position ;
            moveTo += movementDirection * movementMagnitude *  Time.deltaTime;
            
            float activeSpeed=followSpeed *  Time.deltaTime;
            
            Vector3 currentTarget = lerpElseMoveTowards
                ? Vector3.Lerp(position, TargetPos, (activeSpeed/10)  )
                : Vector3.MoveTowards(position, TargetPos, (activeSpeed)  );
            currentTarget -= transform.position;
            moveTo += currentTarget;
            
             
            transform.position = moveTo;
            
        }
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
       
        if (autoSetMinFollowDistance)
        {
            if (followDistanceReference==null)
            {
                followDistanceReference = transform.GetChild(0);
                Debug.Log(" first child taken as reference");
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
 
    }
    private float CalculateDistance(Transform objA,Transform objB,bool acceptX=true,bool acceptY=true,bool acceptZ=true)
    {
        var position1 = objA.position;
        Vector3 objBTemp = position1;
        var position = objB.position;
        if (acceptX)
        {
            objBTemp.x = position.x;
        }
        if (acceptY)
        {
            objBTemp.y = position.y;
        }
        if (acceptZ)
        {
            objBTemp.z = position.z;
        }
 
        return Vector3.Distance(position1, objBTemp);

    }
    private float CalculateDistanceNoY(Transform objA,Transform objB )
    {
        return   CalculateDistance(objA, objB, true, false);

    }
}
