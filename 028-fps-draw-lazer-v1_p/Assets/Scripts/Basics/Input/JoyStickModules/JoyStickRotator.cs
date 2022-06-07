using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickRotator : MonoBehaviour
{
    [SerializeField]  Vector3 TargetDirection;
    public float joyStickHorizontal;
    public float joyStickVertical;
    [SerializeField]  Vector3 JoyStickRequestDirection;
    public bool InputAccepted;//InputAccepted 
    [Header("  ")] 
    [SerializeField] private Transform Compass;
    [SerializeField] private Transform TargetObject;
    [SerializeField] private char HorizontalEffectAxis='x';
    [SerializeField] private char VerticalEffectAxis='z';
    [SerializeField] private bool reverseHorizontal;
    [SerializeField] private bool reverseVertical;
    
    public bool Working=true;

    [SerializeField] private bool lockRotationToInput;
    
    [SerializeField] private float rotationSpeed=300;
    [SerializeField] private float joyStickRotationDiscardPercent = 20; // DONT MAKE IT 0 !!
    private float lastJoyStickRotationDiscardPercent  ; 
    private float _joyStickRotationDiscardPercent = 0; 
    
    [SerializeField] private Joystick _joystick;//_joystick
    
    

    [SerializeField] private bool checkSurfaceTouch;
    public List <Transform > surfaceCheckPositions;// sphere objects would be preferable if empty objects used their checking distances will be as given default
    public float defaultSurfaceCheckDistance=0.4f; // default check distance
    public List <float >  surfaceDetectionDistances ; // check points assigned check distances  
    
    public LayerMask groundMask; // for select will jump-gravity related layers 
    public bool onGround;  // indicator and check bool  
     
    [SerializeField] private bool TouchActive;
    
    [SerializeField] private bool FreezeReference;
    [SerializeField] private GameObject frozenReference;

    [SerializeField] private bool useSlowTurnWithFirstTouch ;
    [SerializeField] private bool slowTurningActive ;
    [SerializeField] private float slowTurningSpeed;
    
    
    
    void Start()
    {
        adjustments();
    }

     
    void FixedUpdate()
    {
        if (Working)
        {
            AllProcess();
            
            
        }
    }
    //------------------------------ MAIN METHOD -------------------
    
    public void AllProcess()
    {
         
        variableUpdate();
        
        rotatingProcess();
        
         

    }
    private void rotatingProcess()
    {

        float currentTurningSpeed = rotationSpeed;
        if (useSlowTurnWithFirstTouch )
        {
            if (slowTurningActive)
            {
                currentTurningSpeed = slowTurningSpeed;
            }
        }
        if (InputAccepted)
        {
            JoyStickRequestDirection=JoystickInputToDirection(joyStickHorizontal,HorizontalEffectAxis,joyStickVertical,VerticalEffectAxis);
            TargetDirection = JoyStickRequestDirection;
            if (!FreezeReference)
            {
                frozenReference.transform.position =   Compass.position   ;
                frozenReference.transform.rotation=   Compass.rotation  ;
            }

            TargetDirection = getLocalDirection(TargetDirection.normalized,frozenReference.transform);
            TargetDirection = TargetDirection.normalized;

             
            Quaternion lookRotation = Quaternion.LookRotation(TargetDirection );
            TargetObject.localRotation = Quaternion.Slerp(TargetObject.rotation, lookRotation, Time.deltaTime * currentTurningSpeed);
            
            if (useSlowTurnWithFirstTouch)
            {
                if (Vector3.Distance(TargetObject.localRotation.eulerAngles,lookRotation.eulerAngles)<5)//TODO adjust
                {
                    slowTurningActive = false;
                }
            }
        }

        if ( lockRotationToInput)
        {
            currentTurningSpeed = rotationSpeed;// TODO IDK IM LOST !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            
            Quaternion lookRotation = Quaternion.LookRotation(TargetDirection );
            TargetObject.localRotation = Quaternion.Slerp(TargetObject.rotation, lookRotation, Time.deltaTime * currentTurningSpeed);
        }
        
       

    }
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

         

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }

    private Vector3 JoystickInputToDirection(float Horizontal,char hAxis,float Vertical,char vAxis)
    {
        Vector3 result = new Vector3(0, 0, 0);
        if (hAxis=='z'||hAxis=='Z')
        {
            result.z = Horizontal;
        }
        else if (hAxis=='y'||hAxis=='Y')
        {
            result.y = Horizontal;
        }
        else
        {
            result.x = Horizontal;
        }
        if (vAxis=='x'||vAxis=='X')
        {
            result.x = Vertical;
        }
        else if (vAxis=='y'||vAxis=='Y')
        {
            result.y = Vertical;
        }
        else
        {
            result.z = Vertical;
        }

        return result;
    }
    
    //------------------      GOVERNING  -------------------------

    private void variableUpdate()
    {
        touchUpdate();
        updateInput();
        InputAccepted = checkEligibility();
        if (useSlowTurnWithFirstTouch)
        {
            if (TouchActive==false)
            {
                slowTurningActive = true;
            }
        }
        if (checkSurfaceTouch)
        {
            checkSurfaceTouchCondition();
            InputAccepted = InputAccepted && onGround;
        }
        if (Math.Abs(joyStickRotationDiscardPercent - lastJoyStickRotationDiscardPercent) > 1)
        {
            setTolerance();
        }
        
    }
    private void checkSurfaceTouchCondition()
    {
       
        bool _onGround = false;
         
        for (var index = 0; index < surfaceCheckPositions.Count && index < surfaceDetectionDistances.Count; index++)
        {
            var each = surfaceCheckPositions[index];
             
            
            if ( checkCollisionWithLayerMask(each, groundMask,surfaceDetectionDistances[index]))
            {
                _onGround = true;
            }
            
            if (_onGround )
            {
                break;
            }
        }

        onGround = _onGround;
         
    }
    private bool checkCollisionWithLayerMask(Transform position,LayerMask layerMask,float distance)//
    { 
        return Physics.CheckSphere(position.position, distance, layerMask);
 
    }
    private void updateInput()
    {
        int reverseH = reverseHorizontal ? -1 : 1;
        int reverseV = reverseHorizontal ? -1 : 1;
        
       
            joyStickHorizontal = _joystick.Horizontal*reverseH;
            joyStickVertical = _joystick.Vertical*reverseV;
        
    }
    private bool checkEligibility()// checkInput feed
    {
        return (Math.Abs(joyStickHorizontal) > _joyStickRotationDiscardPercent ||
                Math.Abs(joyStickVertical) > _joyStickRotationDiscardPercent);
        

    }
    
    //------------------      ADJUSTMENTS  -------------------------
    private void adjustments()
    {
        if (TargetObject==null)
        {
            TargetObject = transform;
        }

        setTolerance();
        if (_joystick==null)
        {
            Working = false;
            Debug.Log(" NO JOY STICK FOUNDED");
        }
        
        frozenReference=new GameObject("frozen Reference");
    }

    private void setTolerance()
    {
        joyStickRotationDiscardPercent = joyStickRotationDiscardPercent < 0 ? 0 :
            joyStickRotationDiscardPercent > 100 ? 100 :joyStickRotationDiscardPercent; 
        
        lastJoyStickRotationDiscardPercent = joyStickRotationDiscardPercent;
        _joyStickRotationDiscardPercent = adjustPercentage(joyStickRotationDiscardPercent); ;
    }

    private float adjustPercentage(float input,float calculateOver=100)
    {
        float result = input;
        result=result<=0 ? 0 :joyStickRotationDiscardPercent/calculateOver ;
        return result;
    }
    private void touchUpdate()
    {
        if (Input.GetMouseButton(0)&&!TouchActive)
        {
            TouchActive = true;

            frozenReference.transform.position =   Compass.position   ;
            frozenReference.transform.rotation=   Compass.rotation  ;
             
        }

        if (!Input.GetMouseButton(0)&&TouchActive)
        {
            TouchActive = false;
          
        }

       

    }
    
}
