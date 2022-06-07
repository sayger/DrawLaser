using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveVelocity : MonoBehaviour
{
    [SerializeField] private float maxSpeed =10;
    
    [SerializeField] private Rigidbody RB;
    [SerializeField] private float power=10000;
    [SerializeField] private float upPower=1;
    
    public bool onAir;
    [SerializeField] private Transform groundCheck;
    public LayerMask groundMask;
    public float detectionDistance;
    public bool jammed;
    [SerializeField] private Transform stuckCheck;

    [SerializeField] private bool downPullActive;
    [SerializeField] private float downPullSpeedChangeSpeed;
    
    [SerializeField] private float currentDownPower=1;
    [SerializeField] private float minDownPower=200;
    [SerializeField] private float maxDownPower=800;
    
    
    
    
 //   [SerializeField] private float currentDistance;
    
 //   [SerializeField] private float maxDistanceToGround;
    
   // [SerializeField] private Vector3 closestPoint;
    
    void Start()
    {
      //  closestPoint = Vector3.ProjectOnPlane(transform.position, -transform.up);

     //   maxDistanceToGround = Vector3.Distance(closestPoint, transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // closestPoint = Vector3.ProjectOnPlane(transform.position, -transform.up);

      //  currentDistance = Vector3.Distance(closestPoint, transform.position);
        
      /*  if (currentDistance>maxDistanceToGround+0.1f)
        {
            onAir = true;
        }
        else
        {
            onAir = false;
        }*/

      jammed= Physics.CheckSphere(stuckCheck.position, detectionDistance,groundMask);
      onAir = !Physics.CheckSphere(groundCheck.position, detectionDistance,groundMask);
      
      

      if (RB.velocity.z<0)
      {
          RB.velocity  = new Vector3(RB.velocity.x,RB.velocity.y,1);
      } 
      
      if (RB.velocity.z<maxSpeed)
      {
          if (!onAir)
          {
              RB.AddForce(transform.forward*Time.deltaTime*power);   
          }
      }
      if (!onAir&&jammed)
      {
          RB.AddForce(transform.up*Time.deltaTime*upPower);   
      }

      if (onAir&&!jammed)
      {
          downPullActive = true;
          if (currentDownPower<maxDownPower)
          {
              Vector3 targeted = new Vector3(maxDownPower, 0, 0);
              Vector3 current=new Vector3(currentDownPower, 0, 0);
              current = Vector3.Lerp(current, targeted, downPullSpeedChangeSpeed * Time.deltaTime);
              currentDownPower = current.x;
          }
          RB.AddForce(-1*transform.up*Time.deltaTime*currentDownPower);  
      }
      else
      {
          downPullActive = false;
          currentDownPower = minDownPower;
      }
      
     
        
      //  RB.velocity = new Vector3(0,  0, 1*power);
    }
}
