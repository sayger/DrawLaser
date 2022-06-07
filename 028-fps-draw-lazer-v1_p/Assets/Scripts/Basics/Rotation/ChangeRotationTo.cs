using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotationTo : MonoBehaviour
{
    public bool test;
    public bool rotatingNow;
    public Vector3 targetedRotation;
    public bool useTimeElseSpeed;
    public float turningTime;
    public float _turnSpeed;
    private float turnSpeed;
    void Start()
    {
        turningTime = Math.Abs(turningTime);
        _turnSpeed= Math.Abs(_turnSpeed);
        turnSpeed = _turnSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (test)
        {
            test = false;
            startRotation();
            
        }

        if (rotatingNow)
        {
            rotatingNow = rotate(transform, targetedRotation, _turnSpeed);
        }
    }

    private bool rotate(Transform subject,Vector3 targetRotation,float Speed)
    {
        Quaternion target = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

        subject.rotation = Quaternion.Slerp(subject.rotation, target,  Time.deltaTime * Speed);

        if (Vector3.Distance(subject.rotation.eulerAngles,targetedRotation)<0.1)
        {
            return false;
        }

        return true;
    }

    public void startRotation()
    {
        rotatingNow = true;
        if (useTimeElseSpeed)
        {
            _turnSpeed = Vector3.Distance(transform.rotation.eulerAngles, targetedRotation) / (turningTime*100);
        }
        else
        {
            _turnSpeed = turnSpeed;
        }
    }
}
