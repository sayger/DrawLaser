﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addForceWithLimit : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed=10;
    [SerializeField] private float coefficient=10;
    [SerializeField] private Vector3 direction;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        addForceWithLimit.ApplyForceToReachVelocity(rb,direction*speed,coefficient);

    }
    public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * (0.2f * rigidbody.drag);

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }
}
