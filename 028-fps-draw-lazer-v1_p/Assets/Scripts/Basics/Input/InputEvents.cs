using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    #region Singleton

    public static InputEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("EXTRA : " + this + "  SCRIPT DETECTED RELATED GAME OBJ DESTROYED");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        adjustments();
    }

    public void awakeFunctions()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("EXTRA : " + this + "  SCRIPT DETECTED RELATED GAME OBJ DESTROYED");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        adjustments();
    }

    #endregion
    
    [SerializeField] private float EffectPercentage;
    [SerializeField] private float InputPercentage;
    [Header("  ")] 
    [SerializeField] private bool InputAccepted;
    [SerializeField] private bool InputMaximum;
    [SerializeField] private bool TouchActive;
    [Header("  ")] 
    [SerializeField] private float JoyStickHorizontal;
    [SerializeField] private float JoyStickVertical;
    [SerializeField]  Vector3 JoyStickPointDirection;
    [Header("  ")] 
    [SerializeField] private Vector2 FirstTouchPosition;
    [SerializeField] private Vector2 LastTouchPosition;
    [Header("  ")] 
    [SerializeField] private bool ExcludeHorizontal;
    [SerializeField] private bool ExcludeVertical;
    [SerializeField] private char HorizontalEffectAxis='x';
    [SerializeField] private char VerticalEffectAxis='z';
    [Header("  ")] 
    [SerializeField] private bool useCompass;
    [SerializeField] private Transform Compass;
    
    
    [Header(" Sources  ")] 
    [SerializeField] private Joystick _joystick;//_joystick
     
    private bool useJoystickInput=true;
     
    [SerializeField] private float joyStickDiscardPercent = 5; // DONT MAKE IT 0 !!
    private float lastJoyStickDiscardPercent  ; 
    private float _joyStickDiscardPercent = 0;

    public bool OverwritePercentage;
    [SerializeField] private float minEffectPercent=0;
    [SerializeField] private float maxEffectPercent=100;
    
    
    private void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AllProcess();
    }
    public void AllProcess()
    {
         
        variableUpdate();
        
    }
    //--------------------Updates -----------------

    private void variableUpdate()
    {
        
        touchUpdate();
        if (useJoystickInput)
        {
            joystickInputUpdate();
        }
        if (Math.Abs(joyStickDiscardPercent - lastJoyStickDiscardPercent) > 1)
        {
            setTolerance();
        }
    }

    private void joystickInputUpdate()
    {
        
            JoyStickHorizontal = ExcludeHorizontal ? 0 : _joystick.Horizontal;
            JoyStickVertical = ExcludeVertical ? 0 : _joystick.Vertical;
        

        InputPercentage = calculateInputPercent(JoyStickHorizontal, JoyStickVertical, 1, 100);
        EffectPercentage = !OverwritePercentage ? InputPercentage : _OverwritePercentage();
        if (InputPercentage>=100)
        {
            if (!InputMaximum)
            {
                ActivateMaxInputReachedActions();
            }

            InputMaximum = true;
        }
        else
        {
            if (!InputMaximum  )
            {
                ActivateMaxInputLostActions();
            }
            InputMaximum = false;
        }
        
        if (checkEligibility())
        {
            if (!InputAccepted)
            {
                ActivateMinInputReachedActions();
//                Debug.Log(" Running ");
            }

             
            InputAccepted = true;
             
            
        }
        else
        {
            if (InputAccepted)
            {
                ActivateMinInputLostActions();
               // Debug.Log(" Idle ");
            }
            InputAccepted = false;
            
        }
        
        

    }
    
    private void touchUpdate()
    {
        if (Input.GetMouseButton(0)&&!TouchActive)
        {
            TouchActive = true;
            FirstTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            ActivateNewTouchActions();
        }

        if (!Input.GetMouseButton(0)&&TouchActive)
        {
            TouchActive = false;
            LastTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            ActivateTouchReleaseActions();
        }

        JoyStickPointDirection=JoystickInputToDirection(JoyStickHorizontal,HorizontalEffectAxis,JoyStickVertical,VerticalEffectAxis);
        if (useCompass)
        {
            JoyStickPointDirection = getLocalDirection(JoyStickPointDirection, Compass);
        }

    }
    
    //---------------------- CALCULATIONS  ---------------
    
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
    private float _OverwritePercentage()
    {
        return resizePercent(minEffectPercent, InputPercentage, _joyStickDiscardPercent, 100, maxEffectPercent);
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
    private bool checkEligibility()// checkInput feed
    {
        return ( calculateInputPercent(JoyStickHorizontal,JoyStickVertical,1,100) 
                 > _joyStickDiscardPercent);
        

    }
    //--------------------ADJUSTMENTS ----------------
    private void adjustments()
    {
        solveInconsistency();
        setTolerance();
        if (_joystick==null)
        {
            useJoystickInput = false;
            Debug.Log(" NO JOY STICK FOUNDED");
        }
        

        InputAccepted = false;

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
    //------------------------- GETTERS ----------------

    public float getCurrentPercentage(bool getBetween0_1=false)
    {
        return getBetween0_1 ? EffectPercentage/100 : EffectPercentage;
    }
    public float getInputPercentage()
    {
        return InputPercentage;
    }
    public float getJoyStickHorizontal()
    {
        return JoyStickHorizontal;
    }
    public float getJoyStickVertical()
    {
        return JoyStickVertical;
    }
    public bool getInputAccepted()
    {
        return InputAccepted;
    }
    public bool getTouchActive()
    {
        return TouchActive;
    }

    public bool getInputMaximum()
    {
        return InputMaximum;
    }
    public Vector2 getFirstTouchPosition()
    {
       return FirstTouchPosition;
    }
    public Vector2 getLastTouchPosition()
    {
        return LastTouchPosition;
    }

    public float getPercentageAcceptedOver()
    {
        return _joyStickDiscardPercent;
    }
    
    //------------------------  EVENTS  ---------------- 
    
    public event Action  NewTouch;

    public void ActivateNewTouchActions( )
    {
        if (NewTouch != null)
        {
            NewTouch( );
        }
    }
    
    public event Action  TouchRelease;

    public void ActivateTouchReleaseActions( )
    {
        if (TouchRelease != null)
        {
            TouchRelease( );
        }
    }
    
    public event Action  MaxInputReached;

    public void ActivateMaxInputReachedActions( )
    {
        if (MaxInputReached != null)
        {
            MaxInputReached( );
        }
    }
    public event Action  MaxInputLost;

    public void ActivateMaxInputLostActions( )
    {
        if (MaxInputLost != null)
        {
            MaxInputLost( );
        }
    }
    public event Action  MinInputReached;

    public void ActivateMinInputReachedActions( )
    {
        if (MinInputReached != null)
        {
            MinInputReached( );
        }
    }
    public event Action  MinInputLost;

    public void ActivateMinInputLostActions( )
    {
        if (MinInputLost != null)
        {
            MinInputLost( );
        }
    }
     
    
    
    
}
