using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldJoystickMover : MonoBehaviour
{
    // debug
    public float joyStickHorizontal;
    public float joyStickVertical;

     
 //   public float saidRotationY;
    
    //-----------------------------------------------------

    public bool Working=true;
    public bool onTheMove;
    [SerializeField] private bool itsOnSurface; 
    [SerializeField] private bool isGrounded; 
  //  [SerializeField] private bool ScreenOnTheTouch;//TO DO
    private bool firstTouch;



    [SerializeField] private float joyStickDiscardMovementPercent = 0;
    private float _joyStickDiscardPercent=15;
    
    [SerializeField] private bool useRotation;
    [SerializeField] private float rotationSpeed=300;
    [SerializeField] private float joyStickRotationDiscardPercent = 5; // DONT MAKE IT 0 !!
    private float _joyStickRotationDiscardPercent = 0; 
  //  [SerializeField] private bool reversHRotationEffect;// TO DO
 //   [SerializeField] private bool reversVRotationEffect;// TO DO
    //  [SerializeField] private bool discardHorizontalInput;// TO DO
    //  [SerializeField] private bool discardVerticalInput;// TO DO
    
    [SerializeField] private bool useMovement;
    
    [SerializeField] private bool useGradualSpeed=true;
    public float speed=20;
   // [SerializeField] private bool reversHMovementEffect;// TO DO
   // [SerializeField] private bool reversVMovementEffect;// TO DO
    //  [SerializeField] private bool discardHMovementInput;// TO DO
    //  [SerializeField] private bool discardVMovementInput; // TO DO
    //---------------------------------------------------------
    //   [SerializeField] private bool useSpeedBuildUp;// TO DO
//    [SerializeField] private float extraSpeed;//TO DO
//    [SerializeField] private bool useRelativeDirections = false; // TO DO ROTATİON CAN DO IT WITH A NORTH STAR BUT MOVEMENT ?
// TO DO : MAKE A  A NORTH START !!!!
    [SerializeField] private bool useAnimator;
    [SerializeField] private Animator _animator;
    [SerializeField] private string animatorSetTrueOnMovement="Running" ;
    [SerializeField] private string animatorSetTrueOnAir="Airborne" ;
 //   [SerializeField] private string animatorSetTrueOnSpecialGround="OnSpecialGround" ;//TO DO


    [SerializeField] private bool useGravity;
    [SerializeField] private float gravity=-9.81f;
    [SerializeField] private float gravityPowerFold=5;
    
    private Vector3 velocity;
    [SerializeField] private Transform groundCheck;//TO DO MAKE A GROUND CHECK MAKER
    [SerializeField] private float detectionDistance=0.4f;
    [SerializeField] private LayerMask surfaceMask;
    [SerializeField] private LayerMask groundMask;



    [SerializeField] private FloatingJoystick joystick;
    
    [SerializeField] private CharacterController controller;


    public bool finalRotation = false;
    public Vector3 wtf = new Vector3(0,179,0);
    private GameObject lookingTarget;
    
    //--------additional not needed now
    
    
    
     
    //----------------------------
    public Animator getPlayerAnimator()
    {

       return _animator;


    }
    public void turnToCamera(GameObject target)
    {
        finalRotation = true;

        lookingTarget = target;


    }

    public void falling()
    {

        //_animator.SetBool(animatorSetTrueOnAir, true);


    }

    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Working)
        {
            variableUpdate();
            
            move();

            if (useGravity)
            {
                gravitation();
            }

            if (useRotation )
            {
                rotatingProcess();
            }
            
            if (useAnimator)
            {
                animatorUpdate();
            }
        }

        if (finalRotation)
        {
            Quaternion currentRotation = transform.rotation;
            Vector3 angle = findRotationAngles();
            angle = wtf;
           // wtf = angle;
            rotate(gameObject,false,true,false,angle,rotationSpeed );
            
            /*
            Quaternion currentRotation = transform.rotation;
            var q = Quaternion.LookRotation(lookingTarget.transform.position - transform.position);
            wtf = new Vector3(0, q.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);*/
        }

    }
 

    private void gravitation()
    {

        if (itsOnSurface&&velocity.y<0)
        {
            velocity.y = gravity;
        }
        downPulse();

    }
    private void downPulse()
    {

        velocity.y += gravity*Time.deltaTime*gravityPowerFold;
        controller.Move(velocity*Time.deltaTime);



    }
    private void move()
    {
        if (onTheMove)
        {
            float horizontalFactor=joyStickHorizontal;
            float verticalFactor=joyStickVertical;

            float speedH =   speed;
            float speedV =   speed;

            if (!useGradualSpeed)
            {
                float biggerFactor = Math.Abs(joyStickHorizontal) > Math.Abs(joyStickVertical) ? joyStickHorizontal : joyStickVertical;
                float speedFixer = Math.Abs(1 / biggerFactor);

                horizontalFactor *=   speedFixer;
                verticalFactor *=   speedFixer;
            }

            Vector3 directionHorizontal = new Vector3(horizontalFactor, 0, 0);
            controller.Move(directionHorizontal * (speedH * Time.deltaTime));
        
            Vector3 directionVertical = new Vector3(0, 0, verticalFactor);
            controller.Move(directionVertical * (speedV * Time.deltaTime));
        }
        



    }

    private void animatorUpdate()
    {
        if (onTheMove)
        {
            _animator.SetBool(animatorSetTrueOnMovement, true);
        }
        else
        {
            _animator.SetBool(animatorSetTrueOnMovement, false);
        }

        if (useGravity)
        {
            if (itsOnSurface)
            {
              //  _animator.SetBool(animatorSetTrueOnAir, false);
            }
            else
            {
                _animator.SetBool(animatorSetTrueOnAir, true);
            }
                
        }
    }

    public void animatorBoolSet(string aniBool,bool setValue)
    {

        _animator.SetBool(aniBool, setValue);


    }

    private void variableUpdate()
    {
        joyStickHorizontal = joystick.Horizontal;
        joyStickVertical = joystick.Vertical;
        onTheMove = checkIfOnTheMove();
        if (useGravity)
        {
            itsOnSurface = hitSurface(surfaceMask);
            isGrounded= hitSurface(groundMask);
        }
        
    }

    private bool hitSurface(LayerMask layer)
    { 
        return Physics.CheckSphere(groundCheck.position, detectionDistance,layer );
 
    }
    private void setUp()
    {
        _joyStickDiscardPercent = 1*joyStickDiscardMovementPercent/100 ;
        _joyStickRotationDiscardPercent=1*joyStickRotationDiscardPercent/100 ;

    }

    private bool checkIfOnTheMove()
    {
        return (Math.Abs(joyStickHorizontal) > _joyStickDiscardPercent ||
                Math.Abs(joyStickVertical) > _joyStickDiscardPercent);
        

    }

    private void rotatingProcess()
    {

       // alternateRotate(gameObject);
         rotate(gameObject,false,true,false,findRotationAngles(),rotationSpeed );

    }

    private void alternateRotate(GameObject subject)
    {
        var position = subject.transform.position;
        Vector3 target = new Vector3(position.x, position.y, position.z);
        target.x += joyStickHorizontal;
        target.z += joyStickVertical;
        
        
        
   
        

        if (Math.Abs(joyStickHorizontal)>_joyStickRotationDiscardPercent||Math.Abs(joyStickVertical)>_joyStickRotationDiscardPercent)
        {

            subject.transform.LookAt(target);
        }
        
    }

    private Vector3 findRotationAngles()// works for Y only
    {
        float tolerance = _joyStickRotationDiscardPercent;
        Quaternion currentRotation = transform.rotation; 
        Vector3 result=new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);
        
        double joyStickH = joystick.Horizontal;
        double joyStickV = joystick.Vertical;
        bool hPositive = joyStickH>0 ? true : false  ;
        bool vPositive = joyStickV>0 ? true : false  ;

        if (Math.Abs(joyStickH ) < tolerance&&Math.Abs(joyStickV )<tolerance) // SHOULD HAVE BE USELESS
        {
             
            return result;
        }
        /*
        if (Math.Abs(joyStickV )<tolerance) // SHOULD HAVE BE USELESS
        {
            result.y= joyStickH>0 ?  90 :  270;
            return result;
        } 
        
        if (Math.Abs(joyStickH ) < tolerance) // SHOULD HAVE BE USELESS
        {
            result.y= joyStickV>0 ?  0 :  180;
            return result;
        } */
       
        
        double hypotenuse = Math.Sqrt((Math.Pow(joyStickH,2)+Math.Pow(joyStickV,2)));
        double theSin = joyStickH / hypotenuse;
        double theCos = joyStickV / hypotenuse;
        
        double resS = Math.Asin(theSin);
        double restS = resS * (180 / Math.PI);
        
        double resC = Math.Asin(theCos);
        double restC = resC * (180 / Math.PI);
        

      /*  if (hPositive&&vPositive)
        {

            result.y = (float) restS;
        }
        if (hPositive&&!vPositive)
        {
            result.y = (float) restC+90;
        }
        if (hPositive&&!vPositive)
        {
            result.y = (float) restC+270;
        }
        if (!hPositive&&!vPositive)
        {
            result.y = (float) restS+180;
        }*/

      
        result.y = (float) restS;
        if (joyStickV<0)
        {
            result.y = 180-result.y;
        }
        //saidRotationY = result.y;
       // result.y = (float) restC;
        
        return result;
    }

    private void rotate(GameObject subject,bool turnX,bool turnY,bool turnZ,Vector3 angles,float Speed)
    {

        rotate(subject, turnX, turnY, turnZ, angles,Speed,Speed,Speed);


    }

    private void rotate(GameObject subject,bool turnX,bool turnY,bool turnZ,Vector3 angles,float xSpeed,float ySpeed, float zSpeed)
    {
        Quaternion currentRotation = subject.transform.rotation;

        var subjectEulerAngles = subject.transform.eulerAngles;
        
        Quaternion targetRotation = Quaternion.Euler(subjectEulerAngles.x, angles.y, subjectEulerAngles.z);
        
        
        if (Math.Abs(joyStickHorizontal)>_joyStickRotationDiscardPercent||Math.Abs(joyStickVertical)>_joyStickRotationDiscardPercent)
        {

            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
        }
        
        
        
        
        
        
        
       /* Quaternion currentRotation = subject.transform.rotation; 
        
        if (turnX)
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation, Quaternion.Euler(angles.x, currentRotation.y, currentRotation.z), xSpeed * Time.deltaTime);
        
        if (turnY)
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation, Quaternion.Euler(currentRotation.x, angles.y, currentRotation.z), ySpeed * Time.deltaTime);
        
        if (turnZ)
            subject.transform.rotation = Quaternion.RotateTowards(subject.transform.rotation, Quaternion.Euler(currentRotation.x, currentRotation.y, angles.z), zSpeed * Time.deltaTime);

        /* this part is unEffective i assume
         
         
        if (Math.Abs(currentRotation.x - angles.x) < 0.01f)
            turnX = false;
        if (Math.Abs(currentRotation.y - angles.y) < 0.01f)
            turnY = false;
        if (Math.Abs(currentRotation.z - angles.z) < 0.01f)
            turnZ = false;*/


    }


    public Vector3 InputEffect ()//TODO
    {
        return new Vector3();
    }

}
