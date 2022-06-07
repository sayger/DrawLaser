using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitDirectionFinder : MonoBehaviour
{
    public Vector3 OrbitDirection;
    
    public Transform baseObj;
    public Transform orbiterObj;
    public bool RigtORLeft;
    public bool ShowGizmos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 basePos = baseObj.position;
        Vector3 orbiterPos = orbiterObj.position;
        orbiterPos.y = basePos.y;
        OrbitDirection = (basePos - orbiterPos).normalized;
        Vector3 temp = OrbitDirection;
        OrbitDirection.x = temp.z;
        OrbitDirection.z = temp.x;
        if (RigtORLeft)
        {
            OrbitDirection.x *= -1;
        }
        else
        {
            OrbitDirection.z *= -1;
        }

        if (ShowGizmos)
        {
            Debug.DrawRay(orbiterObj.position, OrbitDirection*10, Color.yellow);
        }
        
    }
}
