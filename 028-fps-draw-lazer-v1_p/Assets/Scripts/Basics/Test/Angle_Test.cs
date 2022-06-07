using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle_Test : MonoBehaviour
{
    [SerializeField] private bool useForwardDirection;
    [SerializeField] private Transform objectA;
    [SerializeField] private Vector3 directionATob;
    [SerializeField] private Transform objectB;
    [SerializeField] private Vector3 BForwardDirection;
    [SerializeField] private Transform objectC;
    [SerializeField] private float currentAngle ;
    [SerializeField] private float validationMaxAngle;
    [SerializeField] private bool CIsValid;

    public Vector3 targetingDirectionNow;

    [SerializeField] private Transform directionVisualConfirmCompass;

    [SerializeField] private bool useCompass;
    void Start()
    {
        if (directionVisualConfirmCompass==null)
        {
            useCompass = false;
        }
        else
        {
            useCompass = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        directionATob = DirectionFromOneToOther(objectA, objectB);
        BForwardDirection = objectB.forward;
        targetingDirectionNow = BForwardDirection;
        if (!useForwardDirection)
        {
            targetingDirectionNow = directionATob;
            
        }

        if (useForwardDirection)
        {
            CIsValid = ValidateAngle(objectB, objectC, validationMaxAngle, "y");
        }
        else
        {
            CIsValid = ValidateAngle(objectA,objectB, objectC, validationMaxAngle, "y");
        }

        if (useCompass)
        {
            directionVisualConfirmCompass.rotation = Quaternion.LookRotation(targetingDirectionNow);
        }


    }
    
        private bool ValidateAngle(Transform originOfGaze, Transform PossibleTarget, Vector3 CenterAngleLineDirection,
        float AngleRange,string discardAxis= "")
    {
        if (discardAxis!="")
        {
            CenterAngleLineDirection = DiscardAxis(CenterAngleLineDirection,discardAxis);
        }
        float calculatedAngle =CalculateAngle(originOfGaze, PossibleTarget, CenterAngleLineDirection);
        
        return calculatedAngle < AngleRange  ;

    }
    private bool ValidateAngle(Transform originOfGaze, Transform PossibleTarget, 
        float AngleRange,string discardAxis= "")
    {
        Vector3 CenterAngleLineDirection = originOfGaze.forward;

        return ValidateAngle(originOfGaze, PossibleTarget, CenterAngleLineDirection, AngleRange,discardAxis);

    }
    private bool ValidateAngle(Transform Deterrent , Transform originOfGaze, Transform PossibleTarget, 
        float AngleRange,string discardAxis= "")
    {
        Vector3 CenterAngleLineDirection =DirectionFromOneToOther(Deterrent,originOfGaze);
        
        return ValidateAngle(originOfGaze, PossibleTarget, CenterAngleLineDirection, AngleRange,discardAxis);

    }
    //---------------------------------------  CALCULATIONS  -----------------------------------

    
    
    
    
    
    
    
    
    
    
    // ---- Angle Calculatıons ------
    
    
    
    
    private float CalculateAngle(Transform originOfGaze, Transform PossibleTarget, Vector3 CenterForDirection)
    {
         
        var centerForDirection = CenterForDirection;
        Vector3 limiterForward =  centerForDirection;

        var position = PossibleTarget.position;
        Vector3 PossibleTargetPos = position;
      
        Vector3 directionHeading = position - originOfGaze.position;
        float result   = Vector3.Angle(limiterForward, directionHeading.normalized);
        currentAngle = result;
        return result;

    }
    
    private Vector3 DirectionFromOneToOther(Transform origin,Transform towards)
    {
        return (towards.position - origin.position).normalized;

    }
    
    //----  Axis Including Excluding --- 

    private Vector3 DiscardAxis(Vector3 input, string exclusionAxis)
    {
        char[] asACharList = exclusionAxis.ToCharArray();

        bool excludeX = false;
        bool excludeY = false;
        bool excludeZ = false;
        
        foreach (var VARIABLE in asACharList)
        {
            if (excludeX&&excludeY&&excludeZ)
            {
                break;
            }
            
            if (VARIABLE=='x'||VARIABLE=='X')
            {
                excludeX = true;
                continue;
            }
            if (VARIABLE=='y'||VARIABLE=='Y')
            {
                excludeY = true;
                continue;
            }
            if (VARIABLE=='z'||VARIABLE=='Z')
            {
                excludeZ = true;
                continue;
            }
            
            
        }

        if (excludeX)
        {
            input.x = 0;
        }
        if (excludeY)
        {
            input.y = 0;
        }
        if (excludeZ)
        {
            input.z = 0;
        }

        return input;


    }
}
