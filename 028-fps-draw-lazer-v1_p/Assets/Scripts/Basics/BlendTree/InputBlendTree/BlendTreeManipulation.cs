using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTreeManipulation : MonoBehaviour
{
    [SerializeField] private bool _Ready=true; // Script active or not, if false Scripts all active state properties will be inaccessible  
    [SerializeField] private bool _Active; // State manipulation is in Use or not
    [SerializeField] private int screenWithPercentWillResultFullEffect=60; // 100 percent means full Horizontal slide effect as 0 blend to 1 blend 50 percent makes half of a full slide gives the same effect
    private float pixelDivideFactorX;
    private float pixelDivideFactorY;
    
    [SerializeField] private string blendState="AnimationPosition";
    
    [SerializeField] private bool oneUsePurpose = true; // if true will cancel input taking and manipulating upon successful release
    [SerializeField] private float reactivateTime=1;
    [SerializeField] private bool willTriggerNextAnimation = true; //  
    [SerializeField] private string nextAnimationParameter="Release";
    [SerializeField] private string nextAnimationSpeedFactor="ReleaseSpeed";
    [SerializeField] private bool effectOnNextAnimationSpeed=true;
    [SerializeField] private bool effectMultipleElseDivide=true;  // if false calculated "speed_factor" will execute as "1/speed_factor"  
    [SerializeField] private float effectMaxFactor=2; 
    // any given value match full blend state "1" and 0 state will be always equal to 1 or -1 for that matter so if this variable set to 5 for example
    // to make next animation run at speed "5" blend tree must be released at "1" if released at "0" next animation will played with speed 1
    

    [SerializeField] private Animator _Animator;

    [SerializeField] private bool inputGetScreenX =true;  // if true finger slide on screen x axis will effect blend tree animation state
    [SerializeField] private bool inputGetScreenY ;   // if true finger slide on screen y axis will effect blend tree animation state 
    [SerializeField] private bool reverseInputX ;  // if true finger slide on screen x axis will effect blend tree animation state
    [SerializeField] private bool reverseInputY ;   // if true finger slide on screen y axis will effect blend tree animation state 

    [SerializeField] private bool allowWastedTouch =true;  // if true finger extra slide after max animation position will be discarded within calculation of future valid slide operations 

    [SerializeField] private bool useReleaseMinMaxLimit;  // if true will make release without triggering the launch possible such event accept as failed release  
    [SerializeField] private int minReleasePercent;  //  max state will accepted as 100 
    [SerializeField] private int maxReleasePercent;  //  min state will accepted as 0 

    [SerializeField] private bool recoverToGivenState; // if true  when a release under min_release_Limit aka "fail release" happens make animation return to the given or its initial state also if 
    [SerializeField] private int desiredRecoveryStatePercent=0;//  max state will accepted as 100
    private float _desiredRecoveryState;
    [SerializeField] private bool useEditorStateForRecovery;
    
    [SerializeField] private float stateRecoveryTime=1; // state recovery time in seconds
    [SerializeField] private bool recoveryIsActive; // state recovery active or not
    private float beforeRecoveryAnimationState;
    private float stateRecoverySpeed;
    

     // max and min pretence could be also used for set a permitted animation space to reserve unused animation positions to create possible wobble effects but it would be irrelevant mostly 
    
    
    

    private float firstAniPos; // animation position at touch start will be useful  for calculating slide difference in time  
    private float firstScreenXPos;
    private float firstScreenYPos;
    private float currentAniPos;
    private float currentScreenXPos;
    private float currentScreenYPos;


    
    
    
    void Start()
    {
        adjustments();
    }

    private void adjustments()
    {
        if (_Animator==null)
        {
            _Animator = gameObject.GetComponent<Animator>();
            if (_Animator == null) _Ready = false;
        }

        screenWithPercentWillResultFullEffect %= 101;
        pixelDivideFactorX= Screen.width / (screenWithPercentWillResultFullEffect/100f);
        pixelDivideFactorY=pixelDivideFactorX*(Screen.height/Screen.width);

        desiredRecoveryStatePercent %= 101;
        desiredRecoveryStatePercent = Math.Abs(desiredRecoveryStatePercent);
        _desiredRecoveryState = useEditorStateForRecovery ? _Animator.GetFloat(blendState): desiredRecoveryStatePercent / 100f;  
            
        minReleasePercent %= 101;
        minReleasePercent = Math.Abs(minReleasePercent);
        
        maxReleasePercent %= 101;
        maxReleasePercent = Math.Abs(maxReleasePercent);

        if (maxReleasePercent<minReleasePercent)
        {
            var temp = maxReleasePercent;
            maxReleasePercent = minReleasePercent;
            minReleasePercent = temp;
            
        }

        if (nextAnimationParameter == null) willTriggerNextAnimation = false;
         
        oneUsePurpose = willTriggerNextAnimation || oneUsePurpose; // because if next animation would starts it would make manipulating blend tree animation become obsolete 
    }

     
    void FixedUpdate()
    {
        if (_Ready)
        {
            
            if ( firstTouch())
            {
                _Active = true ;
                recoveryIsActive = false; // stops self correcting mechanism 
                
                setInitialVariables();


            }else if (continuedTouch())
            {
                manipulateBlendTreeState();
            }
            else if(_Active == true)// release
            {
                _Active = false;
                releaseActions();

            }

            if (recoveryIsActive)
            {
                float temp = _Animator.GetFloat(blendState);  
                
                 
                _Animator.SetFloat(blendState, calculateVariable(temp, _desiredRecoveryState, stateRecoverySpeed));
            }
            
            
        }
    }

    private float calculateSpeedWithTime(float initialValue, float targetValue, float time,bool addDirection=false)
    {
        float speed=( targetValue - initialValue)/time;
        speed = addDirection ? Math.Abs(speed) : speed;
        return speed;
    }
    

    private float calculateVariable( float currentVariable,float targetVariable, float speed_direction)
    {
        float temp = currentVariable;

        speed_direction *= -1; // TODO FIX THIS AT SPEED CALCULATION
        
        temp +=  speed_direction * Time.deltaTime;

        if ((speed_direction<0)&&(temp<targetVariable)  )
        {
          //  Debug.Log("GIVEN CURRENT BLEND STATE WERE : "+currentVariable+"  DESIRED CURRENT  WERE  :  "+temp+" ULTIMATE TARGET VARIABLE WERE :  "+targetVariable+ "  SPEED WERE  : "+speed_direction );
            temp = targetVariable;
            recoveryIsActive = false;
            
             
        }
        else if ((speed_direction>0)&&(temp>targetVariable)  )
        {
          //  Debug.Log("GIVEN CURRENT BLEND STATE WERE : "+currentVariable+"  DESIRED CURRENT  WERE  :  "+temp+" ULTIMATE TARGET VARIABLE WERE :  "+targetVariable+ "  SPEED WERE  : "+speed_direction );
            temp = targetVariable;
            recoveryIsActive = false;
             
        }
        return temp ;
    }

    private float calculateVariable(float currentVariable,float initialState, float targetVariable, float time)// another more compact version  
    {
        float speed_direction= targetVariable - initialState/time;
        
        float temp = currentVariable;

        temp += speed_direction * Time.deltaTime;

        if ((speed_direction<0)&&(temp<targetVariable)  )
        {
            temp = targetVariable;
            recoveryIsActive = false;
             
        }
        else if ((speed_direction>0)&&(temp>targetVariable)  )
        {
            temp = targetVariable;
            recoveryIsActive = false;
             
        }
        return temp ;
        
    }
    private void releaseActions()
    {
        if (useReleaseMinMaxLimit&& ((minReleasePercent>currentAniPos*100)|| (maxReleasePercent<currentAniPos*100)    ))
        {
            if (recoverToGivenState) recoverToTheState(_desiredRecoveryState,stateRecoveryTime);

            return;
        }

        successfulReleaseActions();

    }

    private void successfulReleaseActions()// TODO 
    {
        _Ready = false;
        if (true)
        {
            Invoke(nameof(reActivate), reactivateTime);
            if (recoverToGivenState)
            {
                recoverToTheState(_desiredRecoveryState,stateRecoveryTime);
            }
        }

        if (willTriggerNextAnimation)
        {
            if (effectOnNextAnimationSpeed)
            {
                float speed_Factor=_Animator.GetFloat(nextAnimationSpeedFactor);
                currentAniPos = _Animator.GetFloat(blendState);
                float temp = 1 + (currentAniPos*(effectMaxFactor-1));
                speed_Factor *= effectMultipleElseDivide ? temp : 1 / temp;
                _Animator.SetFloat(nextAnimationSpeedFactor,speed_Factor);
            }

            _Animator.SetBool(nextAnimationParameter, true);

        }
    }

    private void reActivate()
    {
        _Ready = true;
    }

    private void recoverToTheState(float desiredState,float time)
    {
        currentAniPos = _Animator.GetFloat(blendState);
       
       stateRecoverySpeed = calculateSpeedWithTime(currentAniPos, desiredState, time, true);
       
       recoveryIsActive = true;
       
    }

    private void manipulateBlendTreeState()
    {
        currentScreenXPos = getScreenPos( true );
        currentScreenYPos = getScreenPos( false );
        
        
        float xAxisPixelDifference = currentScreenXPos - firstScreenXPos;
        xAxisPixelDifference *= reverseInputX ? -1 : 1;
        float yAxisPixelDifference = currentScreenYPos - firstScreenYPos;
        yAxisPixelDifference *= reverseInputY ? -1 : 1;
        float differanceToInitialAniPos = 0;
        differanceToInitialAniPos += inputGetScreenX ? xAxisPixelDifference/pixelDivideFactorX : 0;
        differanceToInitialAniPos += inputGetScreenY ? yAxisPixelDifference/pixelDivideFactorY : 0;
         

        float temp = firstAniPos + differanceToInitialAniPos;

        if (temp<0||temp>1)
        {
            if (!allowWastedTouch )
            {
                setInitialVariables();
            }
            return;
        }

        currentAniPos = temp;
        _Animator.SetFloat(blendState,currentAniPos);

    }
    private bool firstTouch()
    {
        return  Input.GetMouseButton(0) && !_Active;
    }
    private bool continuedTouch()
    {
        return  Input.GetMouseButton(0) && _Active;
    }
    
    private float getScreenPos(bool xAxisElseYAxis)
    {
        return xAxisElseYAxis ? Input.mousePosition.x : Input.mousePosition.y ;
    }

    private void setInitialVariables()
    {
         firstAniPos =_Animator.GetFloat(blendState);
               
        firstScreenXPos = getScreenPos( true );
        firstScreenYPos = getScreenPos(false);
    }

    
    
}
