  using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickMover : MonoBehaviour  // TODO UPDATE ACROOS ALL PROJECTS 011 014 000 
{
    public bool Working=true;
    [SerializeField]  Vector3 TargetDirection;
    public bool takeHorizontal = true;
    public bool takeVertical = true;
    public float joyStickHorizontal;
    public float joyStickVertical;
    [SerializeField]  Vector3 JoyStickRequestDirection;
    public bool InputAccepted;//InputAccepted 
    [Header(" Sources  ")] 
    [SerializeField] private Joystick joystick;//joystick


    [SerializeField] private CCMovement _ccMovement;
    public bool  effect_CCs_PowerCollection;
    
    
    [Header("If Power Will Managed  ")] 
    
    public bool ManageExistedPower=true  ;

    [Header("Power Output Settings  ")]
    
    [SerializeField] private bool originalPowerAllWays;
    [SerializeField] private float InputPercent;
    [SerializeField] private float minEffectPercent=0;
    [SerializeField] private float maxEffectPercent=100;
    [SerializeField] private float joyStickDiscardPercent = 5; // DONT MAKE IT 0 !!
    private float lastJoyStickDiscardPercent  ; 
    private float _joyStickDiscardPercent = 0;


    [Header("If Power Will Provided  ")]
    
    [SerializeField] private Vector3 GeneratedVelocity;

    [SerializeField] private bool FreezeReference;
    [SerializeField] private GameObject frozenReference;
    
    [SerializeField] private bool ReferenceCompassElseTargetObj;
    
    [SerializeField] private Transform Compass;
    [SerializeField] private Transform TargetObject;
    [SerializeField] private char HorizontalEffectAxis='x';
    [SerializeField] private char VerticalEffectAxis='z';
    [SerializeField] private bool reverseHorizontal;
    [SerializeField] private bool reverseVertical;
    [Header("  ")] 
    public float MovementPower=4;

    [SerializeField] private bool TouchActive;
     
    void  Start()
    {
        adjustments();
    }

    public CCMovement getCCMovement()
    {
        return _ccMovement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Working)
        {
            AllProcess();
             
        }
    }

    public Vector3 AllProcess()
    {
         
        variableUpdate();
        if (ManageExistedPower)
        {
            return new Vector3();
        }
        if (!InputAccepted)
        {
            return new Vector3(0,0,0);
        }
        else
        {
            JoyStickRequestDirection=JoystickInputToDirection(joyStickHorizontal,HorizontalEffectAxis,joyStickVertical,VerticalEffectAxis);
            TargetDirection = JoyStickRequestDirection;
            if (!FreezeReference)
            {
                frozenReference.transform.position = ReferenceCompassElseTargetObj ? Compass.position  : TargetObject.position ;
                frozenReference.transform.rotation= ReferenceCompassElseTargetObj ? Compass.rotation  : TargetObject.rotation ;
            }
           
            TargetDirection = getLocalDirection(TargetDirection.normalized,frozenReference.transform);
            TargetDirection = TargetDirection.normalized;
        }

        GeneratedVelocity = TargetDirection * MovementPower;
        GeneratedVelocity = resizePower(GeneratedVelocity);
         


        return GeneratedVelocity;
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
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

         

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }
    public void setRelevant(Transform subjectObj)
    {
        TargetObject = subjectObj;
        
    }

    
    //-------------------------------ADJUSTMENTS -------------------------
    private void adjustments()
    {
        _ccMovement.InputTakingActive(this);
        solveInconsistency();
        setTolerance();
        if (joystick==null)
        {
            Working = false;
            Debug.Log(" NO JOY STICK FOUNDED");
        }
      
        frozenReference=new GameObject("frozen Reference");

    }

    private void solveInconsistency()
    {
        minEffectPercent = Math.Abs(minEffectPercent);
        maxEffectPercent = Math.Abs(maxEffectPercent);
        if (minEffectPercent>maxEffectPercent)
        {
            (maxEffectPercent, minEffectPercent) = (minEffectPercent, maxEffectPercent);
        }
 
    }
    //------------------------- CALCULATIONS -------------------------
    
    
    
    //------------------      GOVERNING  -------------------------

    private void variableUpdate()
    {
        touchUpdate();
        if (ManageExistedPower!=_ccMovement.getManagementType())
        {
            _ccMovement.setManagementType(ManageExistedPower);
        }
        updateInput();
        InputAccepted = checkEligibility();
        
        if (Math.Abs(joyStickDiscardPercent - lastJoyStickDiscardPercent) > 1)
        {
            setTolerance();
        }
        
    }
    private void updateInput()
    {
        int reverseH = reverseHorizontal ? -1 : 1;
        int reverseV = reverseVertical ? -1 : 1;
        
       
            joyStickHorizontal = joystick.Horizontal*reverseH;
            joyStickVertical = joystick.Vertical*reverseV;
         
       

        if (! takeHorizontal  )
        {
            joyStickHorizontal = 0;
        }

        if (!takeVertical)
        {
            joyStickVertical = 0;
        }
    }
    private void setTolerance()
    {
        joyStickDiscardPercent = joyStickDiscardPercent < 0 ? 0 :
            joyStickDiscardPercent > 100 ? 100 :joyStickDiscardPercent; 
        
        lastJoyStickDiscardPercent = joyStickDiscardPercent;
      //  _joyStickDiscardPercent = adjustPercentage(joyStickDiscardPercent); ;
        _joyStickDiscardPercent = joyStickDiscardPercent ; ;
    }
    private float adjustPercentage(float input,float calculateOver=100)
    {
        float result = input;
        result=result<=0 ? 0 :joyStickDiscardPercent/calculateOver ;
        return result;
    }
    private bool checkEligibility()// checkInput feed
    {
        return ( calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100) 
                > _joyStickDiscardPercent);
        

    }

    public Vector3 resizePower(Vector3 naturalPower)
    {
        variableUpdate();
        if (!InputAccepted)
        {
            return new Vector3(0,0,0);
        } 
        else if (originalPowerAllWays)
        {
            return naturalPower;
        }
        InputPercent = calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100);

        float factorial = minEffectPercent + // TODO change this with resizePercent
                              (  ((InputPercent - _joyStickDiscardPercent) / (100 - _joyStickDiscardPercent))
                                 *(maxEffectPercent-minEffectPercent)  );
       
        return naturalPower*factorial;
    }

    private float calculateInputPercent(float horizontal,float vertical,float inputMax,float percentageOver)
    {
        double squareH = Math.Pow(horizontal, 2);
        double squareV = Math.Pow(vertical, 2);
        float result = (((float) Math.Sqrt(squareH + squareV ))/inputMax)*percentageOver; 

        return result;
    }

    private float resizePercent(float minEffectPer,float inputPer,float discardPercent,float percentageOver,float maxEffectPer)
    {
        float result= minEffectPer +
                      (  ((inputPer - discardPercent) / (percentageOver - discardPercent))
                         *(maxEffectPer-minEffectPer)  );

        if (inputPer<discardPercent)
        {
            return minEffectPer;
        }
        return result;

    }
    private void touchUpdate()
    {
        if (Input.GetMouseButton(0)&&!TouchActive)
        {
            TouchActive = true;

            frozenReference.transform.position = ReferenceCompassElseTargetObj ? Compass.position  : TargetObject.position ;
            frozenReference.transform.rotation= ReferenceCompassElseTargetObj ? Compass.rotation  : TargetObject.rotation ;
             
        }

        if (!Input.GetMouseButton(0)&&TouchActive)
        {
            TouchActive = false;
          
        }

       

    }
}
