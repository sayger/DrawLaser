using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PositionAdjusterLimiter : MonoBehaviour
{
    public bool LimitLocally;
    public Vector3 minLimits = new Vector3(0, 0, 0);
    public Vector3 maxLimits = new Vector3(0, 0, 0);
    public bool limitX;
    public bool limitY;
    public bool limitZ;

    
    public Vector3 LimitThePos(Vector3 input,Transform localRef)
    {
        if (!LimitLocally)
        {
            return LimitThePos(input);
        }
        Vector3 result = input;
        Vector3 parentPos = localRef.parent.position;
        result -= parentPos;

        if (limitX)
        {
            result.x = Mathf.Clamp(result.x, minLimits.x, maxLimits.x);
        }

        if (limitY)
        {
            result.y = Mathf.Clamp(result.y, minLimits.y, maxLimits.y);
        }
        if (limitZ)
        {
            result.z = Mathf.Clamp(result.z, minLimits.z, maxLimits.z);
        }
        result += parentPos;
        return result;
    }
    public Vector3 LimitThePos(Vector3 input )
    {
        Vector3 result = input;


        if (limitX)
        {
            result.x = Mathf.Clamp(result.x, minLimits.x, maxLimits.x);
        }

        if (limitY)
        {
            result.y = Mathf.Clamp(result.y, minLimits.y, maxLimits.y);
        }
        if (limitZ)
        {
            result.z = Mathf.Clamp(result.z, minLimits.z, maxLimits.z);
        }

        return result;
    }

    
}
 