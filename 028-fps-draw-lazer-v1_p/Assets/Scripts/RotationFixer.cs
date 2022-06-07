using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFixer : MonoBehaviour
{
    public bool FİxX;
    public float smoothSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       Vector3 temp = transform.rotation.eulerAngles;
       temp.x =Mathf.MoveTowardsAngle(temp.x,0,Time.deltaTime);
       transform.rotation=Quaternion.Euler(temp);
    }
}
