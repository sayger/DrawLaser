using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavmeshTarget : MonoBehaviour
{
    public bool Working=true;
    
    [SerializeField] private Transform Center;
    [SerializeField] private bool reCenterInTheStart=true;
    [SerializeField] private bool recenterAll=true;
    
    
    [SerializeField] private Vector3 permanentOffset;
     
    
    public bool trackX =true;
    public bool trackY =true;
    public bool trackZ =true;
     
    [SerializeField] private bool TouchActive;
    
    [SerializeField] private bool letJoystickEffect=true;
    [SerializeField]  Vector3 JoyStickRequestDirection;//offset
    [SerializeField]  Vector3 Joystickoffset;//finaloffset
    [SerializeField] private float HorizontalMaxOffset;
    [SerializeField] private float VerticalMaxOffset;

    [SerializeField] private bool ResetWhenReleased;
    
    [SerializeField] private Transform Compass;
    [SerializeField] private bool ReferenceCompassElseTargetObj=true;
    [SerializeField] private Joystick joystick;
    public float joyStickHorizontal;
    public float joyStickVertical;
    public bool InputAccepted;//InputAccepted 
    [SerializeField] private bool horizontalActive=true;
    [SerializeField] private bool verticalActive=true;
    [SerializeField] private char HorizontalEffectAxis='x';
    [SerializeField] private char VerticalEffectAxis='z';
    [SerializeField] private bool reverseHorizontal;
    [SerializeField] private bool reverseVertical;
    [SerializeField] private float joyStickDiscardPercent = 15;  
    private float lastJoyStickDiscardPercent  ; 
    private float _joyStickDiscardPercent = 0; 
    [SerializeField] private float InputPercent;
    [SerializeField] private float minEffectPercent=0;
    [SerializeField] private float maxEffectPercent=100;
    [SerializeField] private bool originalPowerAllWays;
    [SerializeField] private bool FreezeReference;
    [SerializeField] private GameObject frozenReference;

    public float HorizontalResizeOffsetFactor =1;
    public  float VerticalResizeOffsetFactor =1;

    [SerializeField] private float stoppingTime;
    
    [SerializeField] private float stoppingSpeed=1;
    
    
    
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
        float HResize = HorizontalResizeOffsetFactor;
        float VResize = VerticalResizeOffsetFactor;
        
        Vector3 result = new Vector3(0, 0, 0);
        if (hAxis=='z'||hAxis=='Z')
        {
            result.z = Horizontal*HResize;
        }
        else if (hAxis=='y'||hAxis=='Y')
        {
            result.y = Horizontal*HResize;
        }
        else if (hAxis=='x'||hAxis=='X')
        {
            result.x = Horizontal*HResize;
        }
        if (vAxis=='x'||vAxis=='X')
        {
            result.x = Vertical*VResize;
        }
        else if (vAxis=='y'||vAxis=='Y')
        {
            result.y = Vertical*VResize;
        }
        else if (vAxis=='z'||vAxis=='Z')
        {
            result.z = Vertical * VResize;
        }

        result = result.normalized;
        //----------------------------------------
        if (hAxis=='z'||hAxis=='Z')
        {
            result.z *= HResize;
        }
        else if (hAxis=='y'||hAxis=='Y')
        {
            result.y *= HResize;
        }
        else if (hAxis=='x'||hAxis=='X')
        {
            result.x *= HResize;
        }
        if (vAxis=='x'||vAxis=='X')
        {
            result.x *= VResize;
        }
        else if (vAxis=='y'||vAxis=='Y')
        {
            result.y *= VResize;
        }
        else if (vAxis=='z'||vAxis=='Z')
        {
            result.z *=  VResize;
        }

        return result;
    }
    private void AllProcess()
    {
         
        variableUpdate();
       
         
            
        
        if (InputAccepted)
        {
            JoyStickRequestDirection=JoystickInputToDirection(joyStickHorizontal,HorizontalEffectAxis,joyStickVertical,VerticalEffectAxis);
            
            Joystickoffset = JoyStickRequestDirection;
            if (!FreezeReference)
            {
                frozenReference.transform.position = ReferenceCompassElseTargetObj ? Compass.position  : Center.position ;
                frozenReference.transform.rotation= ReferenceCompassElseTargetObj ? Compass.rotation  : Center.rotation ;
            }
           
            Joystickoffset = getLocalDirection(Joystickoffset ,frozenReference.transform);
            Joystickoffset= resizePower(Joystickoffset); // TODO MAYBE NOT ?

        }
        else
        {
            if (ResetWhenReleased&&!TouchActive)
            {
                if (stoppingTime<0.0001)
                {
                    Joystickoffset = Vector3.zero;
                }
                else
                {
                    Joystickoffset = Vector3.MoveTowards(Joystickoffset, new Vector3(),Time.deltaTime*stoppingSpeed);
                }
                
            }
        }

        Vector3 tracking = transform.position;
        if (trackX)
        {
            tracking.x = Center.position.x;
        }
        if (trackY)
        {
            tracking.y = Center.position.y;
        }
        if (trackZ)
        {
            tracking.z = Center.position.z;
        }
        
        transform.position = Joystickoffset + tracking + permanentOffset;

    }
    
     
    private void variableUpdate()
    {
        touchUpdate();
        
        updateInput();
        InputPercent = calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100);
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
          
        if (! horizontalActive  )
        {
            joyStickHorizontal = 0;
        }

        if (!verticalActive)
        {
            joyStickVertical = 0;
        }

        if (ResetWhenReleased&&!TouchActive)
        {
            joyStickHorizontal = 0;
            joyStickVertical = 0;
        }
    }
    private void touchUpdate()
    {
        if (Input.GetMouseButton(0)&&!TouchActive)
        {
            TouchActive = true;

            frozenReference.transform.position = ReferenceCompassElseTargetObj ? Compass.position  : Center.position ;
            frozenReference.transform.rotation= ReferenceCompassElseTargetObj ? Compass.rotation  : Center.rotation ;
             
        }

        if (!Input.GetMouseButton(0)&&TouchActive)
        {
            TouchActive = false;
            stoppingSpeed = (Vector3.Distance(Vector3.zero, (permanentOffset + Joystickoffset))) / stoppingTime;

        }

       

    }
    private void adjustments()
    {
        Centering();
        SolveConflict();
        setTolerance();
        
    }

    private void SolveConflict()
    {
        if (joystick==null)
        {
            letJoystickEffect = false;
            Debug.Log(" NO JOY STICK FOUNDED");
        }
        frozenReference=new GameObject("frozen Reference");
        solveInconsistency();
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
    private void setTolerance()
    {
        joyStickDiscardPercent = joyStickDiscardPercent < 0 ? 0 :
            joyStickDiscardPercent > 100 ? 100 :joyStickDiscardPercent; 
        
        lastJoyStickDiscardPercent = joyStickDiscardPercent;
        //  _joyStickDiscardPercent = adjustPercentage(joyStickDiscardPercent); ;
        _joyStickDiscardPercent = joyStickDiscardPercent ; ;
    }
    private void Centering()
    {
        if (reCenterInTheStart)
        {
            if (recenterAll)
            {
                transform.position = Center.position;
            }
            else
            {
                Vector3 centerPos = transform.position;
                if (trackX)
                {
                    centerPos.x = Center.position.x;
                }
                if (trackY)
                {
                    centerPos.y = Center.position.y;
                }
                if (trackZ)
                {
                    centerPos.z = Center.position.z;
                }

                transform.position = centerPos;
            }
            
        }

        permanentOffset = transform.position - Center.position;
        if (!trackX)
        {
            permanentOffset.x = 0;
        }
        if (!trackY)
        {
            permanentOffset.y =0;
        }
        if (!trackZ)
        {
            permanentOffset.z = 0;
        }
    }
    private bool checkEligibility()// checkInput feed
    {
        return ( calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100) 
                 > _joyStickDiscardPercent);
        

    }
    private float calculateInputPercent(float horizontal,float vertical,float inputMax,float percentageOver)
    {
        double squareH = Math.Pow(horizontal, 2);
        double squareV = Math.Pow(vertical, 2);
        float result = (((float) Math.Sqrt(squareH + squareV ))/inputMax)*percentageOver; 

        return result;
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

        float effectiveInputPercent = (InputPercent - joyStickDiscardPercent) / (100 - joyStickDiscardPercent);

        float factorial = (minEffectPercent + ((maxEffectPercent - minEffectPercent) * effectiveInputPercent))/100;

         
       
        return naturalPower*factorial;
    }

    public void activation(bool active)
    {
        if (active)
        {
            Working = true;
            Joystickoffset = Vector3.zero;
        }
        else
        {
            
                Working = false;
                Joystickoffset = Vector3.zero;
             
        }
         
    }
     
    
}
/*minEffectPercent + // TODO change this with resizePercent
                          (  ((InputPercent - _joyStickDiscardPercent) / (100 - _joyStickDiscardPercent))
                             *(maxEffectPercent-minEffectPercent)  );*/