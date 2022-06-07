using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierRoute : MonoBehaviour
{
 [Range(0, 1)] [SerializeField]
    private float IndicatorSphereSize=0.25f;
    [Range(1, 20)] [SerializeField]
    private float IndicatorSphereFrequency=1f;
 
    [SerializeField] private bool ControlPointsVisual=true; // SET VALUE
    [SerializeField] private bool inGameVisual = false;
    [SerializeField] private bool _VisualOn; // STATE CONTROL VALUE
    
    
    private bool _EditorMode=true;// STATE CONTROL VALUE
     [SerializeField] private double TotalDistance; // OBSERVATION VALUE
     [SerializeField] private double XDistance; // OBSERVATION VALUE
     [SerializeField] private double YDistance; // OBSERVATION VALUE
     [SerializeField] private double ZDistance; // OBSERVATION VALUE

     
     
    [SerializeField] private Transform[] controlPoints=new Transform[4];
    
    private Vector3[] savedPos =new Vector3[4] ;
    private Vector3[] savedPosLocal =new Vector3[4] ;
    private Vector3 gizmosPosition;
    [Header(" ONLY DOLLY MODE")]
    
    [SerializeField] private bool useDolly;

    [SerializeField] private DistanceBasedOperation reference;
    [SerializeField] private bool secondReference;
    
    [SerializeField] private floatInput reference2;
    
    
    [SerializeField] private Transform Subject;
    private Vector3 lastPos;
    [SerializeField] private bool getDelayed;
    
    [SerializeField] private bool reverse;
    [SerializeField] private bool localExecution;
    [SerializeField] private float referencePercent;


    [SerializeField] private bool switchPlaces;

    private bool GameStarted;
    

    public void savePos()
    {
        savedPos[0] = controlPoints[0].position;
        savedPos[1] = controlPoints[1].position;
        savedPos[2] = controlPoints[2].position;
        savedPos[3] = controlPoints[3].position;
        
        savedPosLocal[0] = controlPoints[0].localPosition;
        savedPosLocal[1] = controlPoints[1].localPosition;
        savedPosLocal[2] = controlPoints[2].localPosition;
        savedPosLocal[3] = controlPoints[3].localPosition;
    }
    void Start()
    {
        Adjustments();
    }
    private void Adjustments()
    {
        _EditorMode = false;
       // CheckEssentials();
        GetDistances();
        if (useDolly)
        {
            lastPos = Subject.localPosition;
        }

        if (!inGameVisual)
        {
            ControlPointsVisual = false;
        }
    }
 
    private void FixedUpdate()
    {
        if (useDolly)
        {
            float percent;
            if (secondReference)
            {
                percent = reference2.saidFloat;
            }
            else
            {
                percent = getDelayed ? reference.delayedPercentage() : reference.currentPercentage();
            }
            percent = reverse ? (100 - percent) : percent;
            referencePercent = percent;
            percent /= 100;
            
            if (localExecution)
            {
             
                
                Subject.localPosition = lastPos+  LocalPositionByState(percent);
            }
            else
            {

//                Debug.Log(" percent : "+percent);
                Subject.position = PositionByState(percent);
            }
        }

        if (!GameStarted)
        {
            GameStarted = true;
            checkVisualActivity();
        }
        
    }
 
    private void OnDrawGizmos()
    {
        if (ControlPointsVisual)
        {
            float Frequency = (1 / IndicatorSphereFrequency) * 0.05f*(20*(float)(1f/TotalDistance));
            for (float t = 0; t <=1 ; t+=Frequency)
            {
                gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                                 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                                 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                                 Mathf.Pow(t, 3) * controlPoints[3].position;
                Gizmos.DrawSphere(gizmosPosition,IndicatorSphereSize);
            }
            Gizmos.DrawLine(new Vector3(controlPoints[0].position.x,controlPoints[0].position.y,controlPoints[0].position.z),
                new Vector3(controlPoints[1].position.x,controlPoints[1].position.y,controlPoints[1].position.z));
            Gizmos.DrawLine(new Vector3(controlPoints[2].position.x,controlPoints[2].position.y,controlPoints[2].position.z),
                new Vector3(controlPoints[3].position.x,controlPoints[3].position.y,controlPoints[3].position.z));
        }
        if (_EditorMode)
        {
            GetDistances();
        }

        checkVisualActivity();

        if (switchPlaces)
        {
            switchPlaces = false;
            mirrorSwitchPlaces();
            
        }
        
    }

    private void mirrorSwitchPlaces()
    {
        Vector3 A_OldPlace = new Vector3(controlPoints[0].position.x, controlPoints[0].position.y,
            controlPoints[0].position.z);
        Vector3 Ac_OldPlace = new Vector3(controlPoints[1].position.x, controlPoints[1].position.y,
            controlPoints[1].position.z);
        Vector3 Bc_OldPlace = new Vector3(controlPoints[2].position.x, controlPoints[2].position.y,
            controlPoints[2].position.z);
        Vector3 B_OldPlace = new Vector3(controlPoints[3].position.x, controlPoints[3].position.y,
            controlPoints[3].position.z);

        controlPoints[0].position = B_OldPlace;
        controlPoints[1].position = Bc_OldPlace;
        controlPoints[2].position = Ac_OldPlace;
        controlPoints[3].position = A_OldPlace;
    }
    private void checkVisualActivity()
    {
        if (ControlPointsVisual&&!_VisualOn)
        {
            _VisualOn = true;
            foreach (var VARIABLE in controlPoints)
            {
                VARIABLE.gameObject.GetComponent<MeshRenderer>().enabled=true;
            }
        }
        if (!ControlPointsVisual&&_VisualOn)
        {
            _VisualOn = false;
            foreach (var VARIABLE in controlPoints)
            {
               VARIABLE.gameObject.GetComponent<MeshRenderer>().enabled=false;
            }
        }
    }
    public Vector3 PositionByState(float completionPercent,bool useSavedPos=false)// input between 0-1
    {
        Vector3 result;
        if (useSavedPos)
        {
            result=Mathf.Pow(1 - completionPercent, 3) * savedPos[0] +
                   3 * Mathf.Pow(1 - completionPercent, 2) * completionPercent * savedPos[1] +
                   3 * (1 - completionPercent) * Mathf.Pow(completionPercent, 2) * savedPos[2] +
                   Mathf.Pow(completionPercent, 3) * savedPos[3];
        }
        else
        {
          result =  Mathf.Pow(1 - completionPercent, 3) * controlPoints[0].position +
                              3 * Mathf.Pow(1 - completionPercent, 2) * completionPercent * controlPoints[1].position +
                              3 * (1 - completionPercent) * Mathf.Pow(completionPercent, 2) * controlPoints[2].position +
                              Mathf.Pow(completionPercent, 3) * controlPoints[3].position;
        }
        

        return result;

    }
    public Vector3 LocalPositionByState(float completionPercent,bool useSavedPos=false)// input between 0-1
    {
        Vector3 result;
        if (useSavedPos)
        {
            result =Mathf.Pow(1 - completionPercent, 3) * savedPosLocal[0] +
                    3 * Mathf.Pow(1 - completionPercent, 2) * completionPercent * savedPosLocal[1]  +
                    3 * (1 - completionPercent) * Mathf.Pow(completionPercent, 2) * savedPosLocal[2]  +
                    Mathf.Pow(completionPercent, 3) * savedPosLocal[3] ;
            result -= savedPosLocal[0] ;
        }
        else
        {
            result =Mathf.Pow(1 - completionPercent, 3) * controlPoints[0].localPosition +
                    3 * Mathf.Pow(1 - completionPercent, 2) * completionPercent * controlPoints[1].localPosition +
                    3 * (1 - completionPercent) * Mathf.Pow(completionPercent, 2) * controlPoints[2].localPosition +
                    Mathf.Pow(completionPercent, 3) * controlPoints[3].localPosition;
            result -= controlPoints[0].localPosition;
        }
      
        return result;



    }
    public Vector3 PositionByState(float completionPercent,Vector3 aPos,Vector3 bPos )// input between 0-1
    {
        var result = Mathf.Pow(1 - completionPercent, 3) * aPos +
                     3 * Mathf.Pow(1 - completionPercent, 2) * completionPercent * controlPoints[1].position +
                     3 * (1 - completionPercent) * Mathf.Pow(completionPercent, 2) * controlPoints[2].position +
                     Mathf.Pow(completionPercent, 3) * bPos;


        return result;
    }
    
    public float GetDistances()
    {
        TotalDistance = 0;
        XDistance = 0;
        YDistance = 0;
        ZDistance = 0;

        Vector3 last = new Vector3();

        
        for (float t = 0; t <= 1; t += 0.01f)
        {

            Vector3 current = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                              3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                              3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                              Mathf.Pow(t, 3) * controlPoints[3].position;
            if (t == 0) last = current;

            double xVariant = Math.Abs(Math.Round(current.x * 1000) - Math.Round(last.x * 1000)) / 1000;
            double yVariant = Math.Abs(Math.Round(current.y * 1000) - Math.Round(last.y * 1000)) / 1000;
            double zVariant = Math.Abs(Math.Round(current.z * 1000) - Math.Round(last.z * 1000)) / 1000;

            XDistance += xVariant;
            YDistance += yVariant;
            ZDistance += zVariant;

            TotalDistance += Math.Sqrt(Math.Pow(xVariant, 2) + Math.Pow(yVariant, 2) + Math.Pow(zVariant, 2));

            // TotalDistance += Vector3.Distance(last, current);

            last = current;
        }

        return (float)TotalDistance;
    }
}
