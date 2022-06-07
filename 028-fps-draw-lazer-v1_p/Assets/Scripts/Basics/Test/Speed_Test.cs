using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_Test : MonoBehaviour
{
    [SerializeField] public bool overrideSpeed=false;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float constantSpeed =0;
    private float speed = 0;
    [SerializeField] private bool handBreak=false;
    
    public float Speed;
    public float AngularSpeed;
    private Rigidbody rb;
     
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = constantSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Speed = rb.velocity.magnitude;
        AngularSpeed = rb.angularVelocity.magnitude;
        if (handBreak)
            speed = 0;
        else speed = constantSpeed;


        if (overrideSpeed)
            setVelocity();


    }

    private void setVelocity()
    {  
        rb.velocity = direction * speed;
        rb.velocity = direction * speed;
    }
}
