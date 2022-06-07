using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    [SerializeField] private Vector3 targetDirection;
    
    [SerializeField] private bool effectOnlyWİthTouch;
    [SerializeField] private bool touchActive;
    
    //------------------------------------------------------
    
    
    [SerializeField] private bool effectOnlyJoystickAcceptedInput;
    [SerializeField] private JoyStickRotator joystick;
    
    [SerializeField] private bool InputAccepted;
     
    [SerializeField] private float maxSpeed =10;
        
    [SerializeField] private Rigidbody RB;
    [SerializeField] private float power=10000;
    [SerializeField] private bool workOnlyGrounded;
    [SerializeField] private bool grounded ;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckDistance ;
    [SerializeField] private LayerMask surfaceMask;
    
    
    
    
        
         
        
         
        
        void Start()
        {
            if (RB==null)
            {
                RB = GetComponent<Rigidbody>();
            }

            if (groundCheckPos==null)
            {
                workOnlyGrounded=false;
            }
            
        }
    
        // Update is called once per frame
        void FixedUpdate()
        {
            if (workOnlyGrounded)
            {
                grounded= Physics.CheckSphere(groundCheckPos.position, groundCheckDistance, surfaceMask);
                if (!grounded) return;
                
            }
            if (effectOnlyWİthTouch)
            {
                if (Input.GetMouseButton(0))
                {
                    touchActive = true;
                    applyForce();  
                }
                else
                {
                    touchActive =  false;
                }
            }
            else if(effectOnlyJoystickAcceptedInput)
            {
                InputAccepted = joystick.InputAccepted;
                if (InputAccepted)
                {
                    touchActive = true;
                    applyForce();  
                }
               
            }
            else
            {
                applyForce();  
            }
            
          
          if (RB.velocity.magnitude>maxSpeed)
          {
              float reverseFactor=maxSpeed/RB.velocity.magnitude;
              float newX= RB.velocity.x * reverseFactor;
              float newY= RB.velocity.y * reverseFactor;
              float newZ= RB.velocity.z * reverseFactor;
              RB.velocity = new Vector3(newX, newY, newZ);


          }
          
    
        }

        private void applyForce()
        {
            targetDirection = transform.forward;
            RB.AddForce( targetDirection*Time.deltaTime*power);
        }
}
