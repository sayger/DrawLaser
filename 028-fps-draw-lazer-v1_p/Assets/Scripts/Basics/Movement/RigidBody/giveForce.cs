using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveForce : MonoBehaviour
{
    public bool Work;
    [SerializeField] private Transform target;
    
    [SerializeField] private Vector3 targetDirection;
    [SerializeField] private float maxSpeed =10;
        
    [SerializeField] private Rigidbody RB;
    [SerializeField] private float power=10000;
    [SerializeField] private float stopDistance=2;
    
    void Start()
    {
        if (RB==null)
        {
            RB = GetComponent<Rigidbody>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Work)
        {
            if (Vector3.Distance(target.position,transform.position)<stopDistance)
            {
                return;
            }
            applyForce();  
            if (RB.velocity.magnitude>maxSpeed)
            {
                float reverseFactor=maxSpeed/RB.velocity.magnitude;
                float newX= RB.velocity.x * reverseFactor;
                float newY= RB.velocity.y * reverseFactor;
                float newZ= RB.velocity.z * reverseFactor;
                RB.velocity = new Vector3(newX, newY, newZ);
 
            }
        }
       
    }
    private void applyForce()
    {
        Vector3 tempTarget = target.position;
        var position = transform.position;
        //tempTarget.y = position.y;
        targetDirection = (tempTarget-position).normalized;
        RB.AddForce( targetDirection*Time.deltaTime*power);
    }
}
