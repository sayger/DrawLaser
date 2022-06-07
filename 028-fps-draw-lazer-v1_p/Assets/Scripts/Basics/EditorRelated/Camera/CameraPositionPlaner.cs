using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionPlaner : MonoBehaviour
{
    [SerializeField] private bool takeItBack;
    
    [SerializeField] private bool work;
    
    [SerializeField] private bool savePosition;
    [SerializeField] private bool changePositionNow ;
    [SerializeField] private int changePosTo ;
    [SerializeField] private bool resetPositions;
    [SerializeField] private int totalPosCount;
     
      
        
    // TODO MAYBE ADD MAKE LOCAL VERSION
        
    [SerializeField] List<Vector3> Positions = new List<Vector3>();
    [SerializeField] List<Quaternion> Rotations = new List<Quaternion>();

    [SerializeField] private Vector3 lastPos;

    [SerializeField] private Quaternion lastRot;
    
    void Start()
    {
        work = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (!work)
        {
            return;
        }
        totalPosCount = Positions.Count - 1;
        if (totalPosCount==-1)
        {
            resetPos();
        }
        if (savePosition)
        {
            savePosition = false;
            savePos();
        }

        if (resetPositions)
        {
            resetPositions = false;
            resetPos();
        }

        if (changePositionNow)
        {
            changePositionNow = false;
            changePos();
        }

        if (takeItBack)
        {
            takeItBack = false;
            var transform1 = transform;
            transform1.position =lastPos;
            transform1.rotation = lastRot;
            
        }
    }

    private void savePos()
    {
        var transform1 = transform;
        Positions.Add( transform1.localPosition) ;
        Rotations.Add( transform1.localRotation ) ;
    }

    private void resetPos()
    {
        List<Vector3> resultPos = new List<Vector3>();
        List<Quaternion> resultRot = new List<Quaternion>();
        resultPos.Add(new Vector3(0,0,0));
        resultRot.Add(Quaternion.identity);
        Positions = resultPos;
        Rotations = resultRot;
    }

    private void changePos()
    {
        if (changePosTo>Positions.Count - 1||changePosTo<0)
        {
            return;
        }
        var transform1 = transform;
        lastPos = transform1.localPosition ;
        lastRot = transform1.localRotation ;

         
        transform1.localPosition = Positions[changePosTo];
        transform1.localRotation = Rotations[changePosTo];
    }
}
