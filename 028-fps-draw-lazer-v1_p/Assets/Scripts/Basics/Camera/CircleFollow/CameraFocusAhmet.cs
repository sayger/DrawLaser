using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraFocusAhmet : MonoBehaviour
{
    public Transform trackObject;

    public bool trackingRun;
    
    public float TrackStartDistance;
    public float TrackStopDistance;//detectorde
    public float currentDistance;
    
    public bool trackX;
    public bool trackY;
    public bool trackZ;

    public Vector3 offSet;
    public float speed;
    public float initialSpeed;

    
    private void Start()
    {
        Vector3 vec = transform.position;
        if (trackX == true)
        {
            vec.x = trackObject.position.x;
        }
        if (trackY == true)
        {
            vec.y = trackObject.position.y;
        }
        if (trackZ == true)
        {
            vec.z = trackObject.position.z;
        }
        transform.position = vec;
        initialSpeed = speed;
    }


    void FixedUpdate()
    {
        currentDistance = CalculateDistance(transform,trackObject,trackX,trackY,trackZ);
        if (currentDistance > TrackStartDistance && trackingRun == false)
        {
            trackingRun = true;
            Vector3 vec = transform.position;
            if (trackX == true)
            {
                vec.x = trackObject.position.x;
            }
            if (trackY == true)
            {
                vec.y = trackObject.position.y;
            }
            if (trackZ == true)
            {
                vec.z = trackObject.position.z;
            }
            offSet = transform.position-vec ;
            
            
        }

        if (currentDistance < TrackStopDistance && trackingRun == true)
        {
            trackingRun = false;
        }
        
        if (trackingRun == true)
        {
            Vector3 vec = transform.position;
            if (trackX == true)
            {
                vec.x = trackObject.position.x;
            }
            if (trackY == true)
            {
                vec.y = trackObject.position.y;
            }
            if (trackZ == true)
            {
                vec.z = trackObject.position.z;
            }

            offSet =Vector3.MoveTowards(offSet, Vector3.zero, speed * Time.deltaTime);


          //  transform.position = vec + offSet;

          if (currentDistance > TrackStartDistance)
          {
              speed = initialSpeed;
          }
          else
          {
              speed = initialSpeed * ((currentDistance - TrackStopDistance) / (TrackStartDistance - TrackStopDistance));
          }
          transform.position=Vector3.MoveTowards( transform.position, vec, speed * Time.deltaTime);
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
