using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixRotation : MonoBehaviour
{
    public Rigidbody RB;
    [SerializeField] private bool work=true;
    
    [SerializeField] private float minRotation=-45 ;
    [SerializeField] private float realMinRotation=-45 ;
    [SerializeField] private float maxRotation=45 ;
    public Vector3 currentRotation;
    public Vector3 targetRotation;
    public bool active;
    public float changeSpeed;
    
    [SerializeField] private bool onAir;
    [SerializeField] private Transform groundCheck;
    public LayerMask groundMask;
    public float detectionDistance;
    public float changeSpeed2;
    public bool useLerp;

    public float onAirRotationX=20;
    void Start()
    {
        realMinRotation = 360 + minRotation;
    }

     
    void FixedUpdate()
    {
        currentRotation  = transform.rotation.eulerAngles;
        targetRotation = currentRotation;
        targetRotation.y = 0;
        targetRotation.z = 0;
        if ((targetRotation.x>180&&targetRotation.x<realMinRotation)||(targetRotation.x<180&&targetRotation.x>maxRotation))
        {
            active = true;
            if (RB.angularVelocity.magnitude > 5)
            {
                RB.angularVelocity  = new Vector3(0,0,0);
            }

            if (targetRotation.x>180)
            {
                targetRotation.x = realMinRotation + 1;
            }
            else
            {
                targetRotation.x = maxRotation + -1;
            }
        }
        else
        {
            active = false;
        }
 
     /*   if (targetRotation.x<realMinRotation&&targetRotation.x>180)
        {
            targetRotation.x = minRotation;
            active = true;
        }
        else if (targetRotation.x>maxRotation&&targetRotation.x<180)
        {
            targetRotation.x = maxRotation;
            active = true;
        }
        else
        {
            active = false;
        }
*/
     onAir = !Physics.CheckSphere(groundCheck.position, detectionDistance,groundMask);
        if ( work)
        {
            if (useLerp)
            {
                Vector3 target = new Vector3(0, 0, 0);;
                float executeSpeed=changeSpeed2;
                if (!onAir)
                {
                    target = targetRotation;
                    executeSpeed=changeSpeed;
                }
               
                Vector3 current = transform.rotation.eulerAngles;
                Vector3 currentTarget = Vector3.Lerp(current,target,Time.deltaTime*executeSpeed);
                
                transform.rotation = Quaternion.Euler(currentTarget);
              
              
            }
            else
            {
                if (onAir)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(onAirRotationX,0,0)),Time.deltaTime* changeSpeed2);
                }
                else if (active)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime* changeSpeed);
                }
                
            }
            


            
            
        }
        
        
    }
}
