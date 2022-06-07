using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SplineLead : MonoBehaviour
{
    [SerializeField] private SplineComputer _splineComputer ;
    [SerializeField] private Transform initialStartPoint;
    [SerializeField] private float distanceBetweenPoints=0.2f;
    [SerializeField] private int TotalNumOfPoints;
    [SerializeField] private List<SplinePoint> activePoints;
    [SerializeField] private Vector3 lastPos;
    [SerializeField] private float inProgressSplineTipLenght=4;
    [SerializeField] private float simpleDistanceBetweenPoints=2f;

    [SerializeField] private float firstLenght ;
    
    [SerializeField] private bool saveSplineVer1_ElseVer2;
     
     
    
    
    
    
    void Start()
    {
        adjustments();
    }

    private void adjustments()
    {
        installPoints();
    }

    private void installPoints()
    {
        var aPosition = initialStartPoint.position;
        var bPosition1 = transform.position;
        
        Vector3 aToB_Vector = (bPosition1 - aPosition) ;
        float totalDistance = Vector3.Distance(aPosition, bPosition1);

        double each = distanceBetweenPoints/totalDistance;
        for (double i = 0; i < 1; i+=each)
        {
            Vector3 temp = (aPosition + ((float)i * aToB_Vector));
            SplinePoint newPoint = new SplinePoint(temp);
            newPoint.normal = Vector3.up;
            newPoint.size = 1f;
            activePoints.Add(newPoint);
        }

        lastPos = bPosition1;
        SplinePoint lastPoint = new SplinePoint(lastPos);
        activePoints.Add(lastPoint);
        _splineComputer.SetPoints(activePoints.ToArray());
        TotalNumOfPoints = activePoints.Count;
        firstLenght = TotalNumOfPoints * distanceBetweenPoints;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        addPoint(checkMovement());
    }

    private bool checkMovement()
    {
        return Vector3.Distance(lastPos,transform.position)>distanceBetweenPoints;
    }

    private void addPoint(bool do_OrNot)
    {
        if (do_OrNot)
        {
            /*
            lastPos = transform.position;
            SplinePoint lastPoint = new SplinePoint(lastPos);
            _splineComputer.SetPoint(_splineComputer.pointCount,lastPoint);
             */
            
            lastPos = transform.position;
            SplinePoint lastPoint = new SplinePoint(lastPos);
            if (lastPoint.position.z<activePoints[activePoints.Count-1].position.z+(distanceBetweenPoints*(5/6f))) //+distanceBetweenPoints/4
            {
                return; 
            }
            
            activePoints.Add(lastPoint);
            List<SplinePoint> temp=new List<SplinePoint>();

            temp = preventFastPull();
            for (var index = 1; index < activePoints.Count; index++)
            {
                var VARIABLE = activePoints[index];
                temp.Add(VARIABLE);
            }

            temp = simplyfySpline(temp);

            activePoints = temp;
            _splineComputer.SetPoints(activePoints.ToArray());
            
        }
    }

    private List<SplinePoint> preventFastPull()
    {
        List<SplinePoint> result = new List<SplinePoint>();
        if (saveSplineVer1_ElseVer2)
        {

            Vector3 Apoint = activePoints[0].position;
            Vector3 Bpoint = activePoints[1].position;
        
            if (Vector3.Distance(Apoint,Bpoint)>distanceBetweenPoints)
            {
                Vector3 direction = (Bpoint - Apoint).normalized;
                Vector3 newPosForFirstPos = Apoint + (direction * distanceBetweenPoints);
                result.Add(new SplinePoint(newPosForFirstPos));
            }
        }
        else
        {
           

            Vector3 lastPointr = activePoints[1].position;
            lastPointr.z = activePoints[activePoints.Count - 1].position.z-firstLenght;

            if (activePoints[1].position.z>lastPointr.z)
            {
                result.Add(new SplinePoint(lastPointr));
            }
             
            
            
            
        }
       
        return result;
    }

    private List<SplinePoint> simplyfySpline(List<SplinePoint> input)
    {
        List<SplinePoint> result = new List<SplinePoint>();

        float distanceCounter=0;
        int lastSafePos = 0;
        for (var index = input.Count-1; index >0; index--)
        {
            var VARIABLE = input[index];
            var VARIABLEMinus = input[index-1];
            distanceCounter += Vector3.Distance(VARIABLE.position, VARIABLEMinus.position);
            if (distanceCounter>=inProgressSplineTipLenght)
            {
                lastSafePos = index - 1;
                break;
            }
        }
        result.Add(input[0]);
        float distanceCounter2 = 0;

        int lastSaveAddress = 0;
        for (var index = 0; index < lastSafePos; index++)
        {
            var VARIABLE = input[index];
            var VARIABLEPlus = input[index+1];
            float tempDis = Vector3.Distance(VARIABLE.position, VARIABLEPlus.position);
            distanceCounter2 += tempDis;

           /*  
            var VARIABLEPlusPlus = input[index+2];
            float f = VARIABLE.position.y;
            float s = VARIABLEPlus.position.y;
            float t = VARIABLEPlusPlus.position.y;
            if (( ( (s<f)&&(s<t) ) || ( (s>f)&&(s>t) ) ) && index!=lastSafePos-2 )
            {
                int start = index - 3;
                int end = index + 3;
                if (start<lastSaveAddress)
                {
                    start = lastSaveAddress + 1;
                }

                if (end>=lastSafePos)
                {
                    end = lastSafePos;
                }

                for (int i = start; i < end; i++)
                {
                    result.Add(input[i] );
                }
                lastSaveAddress = end-1;
                index=end-1;
                distanceCounter2 = 0;
            }
            else */ if (distanceCounter2>=simpleDistanceBetweenPoints)
            {
                lastSaveAddress = index;
                distanceCounter2 = 0;
                result.Add(VARIABLEPlus);
            }
        }   
        
        for (var index = lastSafePos+1; index < input.Count; index++)
        {
            var VARIABLE = input[index];
             
            result.Add(VARIABLE);
             
          
        }
        
        
        return result;
        
    }
}
/*
   
            var VARIABLEPlusPlus = input[index+2];
            float f = VARIABLE.position.y;
            float s = VARIABLEPlus.position.y;
            float t = VARIABLEPlusPlus.position.y;
            if (( ( (s<f)&&(s<t) ) || ( (s>f)&&(s>t) ) ) && index!=lastSafePos-1 )
            {
                if (Math.Abs(distanceCounter2 - tempDis) > 0.001f)
                {
                    result.Add(VARIABLE);
                }
                result.Add(VARIABLEPlus);
                if (index!=lastSafePos-2)
                {
                    result.Add(VARIABLEPlusPlus);
                }
                distanceCounter2 = 0;
                index++;
            }
 */
