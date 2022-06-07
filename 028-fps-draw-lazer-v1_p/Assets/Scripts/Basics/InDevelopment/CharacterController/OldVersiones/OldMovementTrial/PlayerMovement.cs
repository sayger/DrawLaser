using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
    
    //-*-//    //-*-//   //-*-//   //-*-// 
    // Script Features are :
    // movement gravity and phase like executed jump 
    //jump parameters been set like in range "A-to-B" power parameter, will used to calculate the power value will execute the given jump direction at given parameters can set to actively change in desired execution time gap in desired changing types 
{//TODO set all directions to something might be 0 0 0 idk else its become none NAND
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;
    private float offsetTest;
    private float baseTest;
    [Header(" INDICATORS FOR VELOCITY  ")]
    public Vector3 _TheVelocity; //   will executed movement as currentVelocity at the moment // just an indicator of it
    public Vector3 thisScriptsVelocity; //  in house forces combine velocity
    public Vector3 collectedVelocity; //  combined Slave script velocities 
     
    public bool jumpTest; // testUseless bool to activate jump in the editor
    public bool _CancelJump; //  editor jump cancel bool
    
    //---------------------------------
    
    [Header(" ESSENTIAL SETTINGS  ")]
    private GameObject effectedGameObject;
    public bool _MasterScript_ElseSlave=true;// if its not master script only can be use as additional jump feature to a MASTER SCRIPT                        //-*-// IMPORTANT  (non master scripts refers to masters scripts gameObject when localized calculations)
    public bool jumpSlave = true;
    public bool usePresetMoves;
    public List<PresetMove> PresetMoves; 
    public List<PlayerMovement> Slaves=new List<PlayerMovement>();
    public bool TheScriptIsActive=true; //Means Script active or not, if false Scripts all active state properties will be incapable of effect
    public bool bypassInHouseForces;
    public CharacterController characterController;

    //------------------
   
    [Header(" CONSTANT MOVEMENT SETTINGS  ")]
    public bool _moveConstantlyActive = true; // use given direction to move constantly special jump parameters could override this if so will revert back to its original setting when jump phase over
    private bool moveConstantlyActive ; // original reference setting                         //-*-// *REASON FOR ALL ORIGINAL SETTINGS ARE BEEN KEPT ESSENTIALLY IS FOR MAKE POSSIBLE THE SPECIAL PARAMETER JUMP EVENTS //-*-// 

    public Vector3 constantMovingDirection=new Vector3(0,0,1); // desired direction for nonstop movement
    private Vector3 _constantMovingForce;// current resulted "currentVelocity" additive to ready to use in player controller move 
    public bool constantDirectionLocal = true; // if true will make given direction execute in local sense
    
    public float _constantPower; // speed factor for constant movement direction at the momnet// variance value
    private float constantPower; // original reference value 

    //-----------------

    [Header(" ADJUSTABLE GRAVITY SETTINGS  ")]
    public bool _gravityUseActive;  // use given gravity settings or not ,special jump parameters could override this 
    private bool gravityUseActive; // original reference setting

    public Vector3 gravityDirection = new Vector3(0,-1,0);
    private Vector3 _gravitationalForce ; //current resulted additive to "currentVelocity" its ready to use in player controller move 
    public bool gravityDirectionLocal  ; // if true make given direction execute in local sense
    public float _gravitationalPower= 9.81f; // Always positive value must change direction to achieve reverse effects  // variance value
    private float gravitationalPower ; // // original reference value 

    public bool useIncreasingGravity; //if true gravitational power will increase when airborne
    public float gravityBuildUPPerSecond=15; // power addition to gravity per second
    public float ExtraPowerToCalculateMaxGravity= 60f; // addition to base gravitational power to find max possible gravity appliance
    private float maxGravity ; // top gravity power value to reach with increasing gravity // Always positive
    
    //--------------------------
    
    [Header(" SURFACE CONTACT EFFECTS AND REFERENCES  ")]
    
    public List <Transform > surfaceCheckPositions;// sphere objects would be preferable if empty objects used their checking distances will be as given default
    public float defaultSurfaceCheckDistance=0.4f; // default check distance
    public List <float >  surfaceDetectionDistances ; // check points assigned check distances  
    
    public LayerMask groundMask; // for select will jump-gravity related layers 
    public LayerMask surfaceMask; // extra surface mask for extra conditions might needed in later use

    public bool onGround;  // indicator and check bool  
    private bool onGroundLatestRecord;  // variable to compare new ground touch or continued contact 
    public bool onSurface; // indicator and check bool
    
    //-------------------------

    
    [Header(" 'JUMP' LIKE TIME LIMITED FORCE SHIFT PARAMETERS  ")]
    
    public float jumpTimeCounter = 0;
    public float  _currentJumpPower ; // indicator of executed value at the moment
    public bool jumpCanBeUsed=true; // if not it cant Be used withOut Forced method call
    public bool _jumpPhaseInUse  ;// if true means either jump is active or in waiting mode for repeating jumps
    public bool _jumpingActive;// indicates activity  and governs jump movements continuity 

    public bool _jumpUseSecondState;  // To make domino jump effects with same given jump direction but changing power factor and controlled velocity of it
    // e.g giving jumping up power to jump up and giving landing down power in succession to other to having the resulting position change curve in control
    // on the contrary to a regular launch type self canceling jump power adding method
    private bool jumpUseSecondState;
    public bool _JumpingInStage2; // only second stage declaration that matters

    public bool _repeatJump; // makes jumps one after other automatically
    private bool repeatJump; // original setting
    public int _remainingJumpRepeats=5;
    private int remainingJumpRepeats;// original limit value
    
    public float waitBetweenJumps=2f; // the wait time between continues jumps
    
    public bool _jumpsConstantActiveDuring;  // if true constant movement effect will be added on top of the jump properties *its needed for smooth motion at jump state ending  
    private bool jumpsConstantActiveDuring;  
    public bool _jumpsConstantActiveBetween; // if constant movement is not a active factor during jumps and if jumps waits between one other makes constant movement active during that totalTime redundant if _jumpsConstantActiveDuring true
    private bool jumpsConstantActiveBetween;

    public bool _jumpsGravityActiveDuring;   
    private bool jumpsGravityActiveDuring;   
    public bool _jumpsGravityActiveBetween;
    private bool jumpsGravityActiveBetween;
    
    public bool _jumpGroundSensitive;   // if true jump effects cancels out if landing occurs
    private bool jumpGroundSensitive;   // original setting

    public float _jumpingTime=3;   // total jump totalTime its in inverse proportion with speed acceleration 
    private float jumpingTime ; // original setting
    public float _jumpSecondStateTime=3;   // total jump totalTime its in inverse proportion with speed acceleration 
    private float jumpSecondStateTime ; // original value

    public Vector3 _jumpDirection;
    private Vector3 jumpDirection;
    
    public bool    _jumpLocal;   // if true make given direction execute in local sense
    private bool    jumpLocal; // original setting 
    
    private Vector3 currentJumpForce;
    
    public  float _startJumpPower ;  
    private float startJumpPower ; // original value
    
    public float _firstTargetJumpPower ;  
    private float firstTargetJumpPower ; // original value

    public float _secondTargetJumpPower ;   
    private float secondTargetJumpPower ;
    
    [Header(" '0' IS STABLE CHANGE USE NEGATIVE FOR MORE CONSISTENT CURVE ")]
    [Header(" POSITIVE NUMBERS WILL BE POWER OF X IN CONVEX PARABOLA FOR POWER CHANGE VELOCITY   ")]
    [Header(" NEGATIVE NUMBERS WILL BE ROOT OF X IN CONCAVE PARABOLA FOR POWER CHANGE VELOCITY   ")]
    public  int _firstTransitionFactor=0 ;  // 3 type increasing modes available for value change speed acceleration over totalTime 0= stable change speed -/- 1= convex over totalTime -/- 2 = concave  over totalTime   
    private int  firstTransitionFactor ; // original setting

    public int _secondTransitionFactor = 0;  
    private int secondTransitionFactor;


    [Header(" FOR A LITTLE MORE EASIER GRASP ALTERNATIVE INPUT TAKING METHOD  ")]
     
    public bool useDistanceChangeTypeParameter;
    public float _startSpeed;
    private float startSpeed;
    public float _firstStateDistanceChange;
    private float firstStateDistanceChange;
    public float _secondStateDistanceChange;
    private float secondStateDistanceChange;
     
    //---------------------------------------------------------
    [Header(" CONSTANT POWER IN GAME UPDATE SETTINGS   ")]
     
    [SerializeField] private bool ConsPowerIsChanging; 
    [SerializeField] private bool _waitJumpEnd;
    private bool waitJumpEnd;
    [SerializeField] private bool _smoothChange;
    [SerializeField] private float ConsPowerChangeSpeed;
    private bool willWaitIfANewJumpHappens = false;// fixed and not accessible parameter because of its exaggerated nature but can be make effective in possible feature improvements on the script

    
     
    private void Start()
    {
        adjustments();
    }
    private void adjustments() // to save initial settings and solve possible conflicts within settings
    {
        presetMoveSettings();
         
        checkEssentialElements();
        fixMustFixedElements();
        
        checkGravitySettings();
        
        fixSurfaceCheckPositions();

        setOriginalSettingsFromInitialValues(); 
         
        resolveContrast(); //TODO DELETE LATER


    }

    private void presetMoveSettings()
    {
        if (usePresetMoves)
        {
            foreach (var VARIABLE in PresetMoves)
            {
                VARIABLE.masterScript = this;
                foreach (var each in VARIABLE.slaveScripts)
                {
                    Slaves.Add(each);
                }
            }
            
        }
    }
    private void fixSurfaceCheckPositions() // to get surface check points necessary values for later use 
    {
        if (surfaceCheckPositions.Count==0)
        {
            useIncreasingGravity = false; // if cant check landing some features cant be used
            jumpGroundSensitive = false;

        }
        else
        {
            foreach (var each in surfaceCheckPositions) // to use check point objects with their visual extend in the editor 
            {
                float checkDistance = defaultSurfaceCheckDistance;
                Renderer checkRenderer = each.GetComponent<Renderer>();
                if (checkRenderer!=null)
                {
                    checkDistance = each.localScale.y / 2;
                }
                surfaceDetectionDistances.Add(checkDistance);
            }
            
        }
    }
    private void checkEssentialElements()
    {
        if (characterController==null)
        {
            characterController = gameObject.GetComponent<CharacterController>();
        }
        if (characterController==null&&_MasterScript_ElseSlave)
        {
            TheScriptIsActive = false; 
            return;
        }
        
        if (_MasterScript_ElseSlave)
        {
            
            effectedGameObject = characterController.gameObject;
            foreach (var VARIABLE in Slaves)
            {
                VARIABLE.effectedGameObject = effectedGameObject;
            }
        }
        else
        {
            characterController = null;
        }
        


    }
    private void fixMustFixedElements()// to prevent unwanted start states for some variables
    {
        ConsPowerChangeSpeed = Math.Abs(ConsPowerChangeSpeed);
        
        constantMovingDirection = reduceToDirection(constantMovingDirection) ; // To make direction values purely on pointing directions but not effecting power factors//TODO MUST CHECK OUTER SETUPS whıle settıng new directions 
        gravityDirection = reduceToDirection(gravityDirection) ;
        _jumpDirection = reduceToDirection(_jumpDirection) ;

        _JumpingInStage2 = false; // script cant start in middle of a jump
        _jumpPhaseInUse = false;
        _jumpingActive = false;
        
    }
    private void checkGravitySettings()// to set gravity power variables 
    {
        gravitationalPower=Math.Abs(gravitationalPower);
        gravityBuildUPPerSecond = Math.Abs(gravityBuildUPPerSecond);
        ExtraPowerToCalculateMaxGravity=  Math.Abs(ExtraPowerToCalculateMaxGravity);
        maxGravity = gravitationalPower + ExtraPowerToCalculateMaxGravity;
    }
    private void setOriginalSettingsFromInitialValues() // to save editor settings as original settings  
    {

    waitJumpEnd = _waitJumpEnd;
        
    moveConstantlyActive =   _moveConstantlyActive ; //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH SO IF IN JUMP STATE JUST CHANGE REFERENCE IF NOT CHANGE BOTH 
    constantPower = _constantPower ;   
    gravityUseActive = _gravityUseActive;     //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH
    gravitationalPower = _gravitationalPower;  //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH SO IF IN JUMP STATE JUST CHANGE REFERENCE IF NOT CHANGE BOTH 
    
    jumpUseSecondState = _jumpUseSecondState;
    repeatJump = _repeatJump;
    remainingJumpRepeats = _remainingJumpRepeats ;
    jumpsConstantActiveDuring =_jumpsConstantActiveDuring;
    jumpsConstantActiveBetween = _jumpsConstantActiveBetween;
    jumpsGravityActiveDuring = _jumpsGravityActiveDuring;
    jumpsGravityActiveBetween = _jumpsGravityActiveBetween;
    jumpGroundSensitive = _jumpGroundSensitive;
    jumpingTime = _jumpingTime;
    jumpSecondStateTime = _jumpSecondStateTime;
    jumpDirection = _jumpDirection;
    jumpLocal = _jumpLocal;
    startJumpPower = _startJumpPower;
    firstTargetJumpPower = _firstTargetJumpPower;
    secondTargetJumpPower = _secondTargetJumpPower ;
    firstTransitionFactor = _firstTransitionFactor ;
    secondTransitionFactor = _secondTransitionFactor ;
    
    startSpeed = _startSpeed;
    firstStateDistanceChange = _firstStateDistanceChange;
    secondStateDistanceChange = _secondStateDistanceChange ;
     
    }
    private void getJumpOriginalSettings()// to restore original Jump settings because jump settings could have been altered by specialJump request via Player Movement setter script
    {
        _jumpUseSecondState = jumpUseSecondState;
        _repeatJump = repeatJump;
        _remainingJumpRepeats = remainingJumpRepeats ;
        _jumpsConstantActiveDuring = jumpsConstantActiveDuring;
        _jumpsConstantActiveBetween = jumpsConstantActiveBetween;
        _jumpsGravityActiveDuring = jumpsGravityActiveDuring;
        _jumpsGravityActiveBetween = jumpsGravityActiveBetween;
        _jumpGroundSensitive = jumpGroundSensitive;
        _jumpingTime = jumpingTime;
        _jumpSecondStateTime = jumpSecondStateTime;
        _jumpDirection = jumpDirection;
        _jumpLocal = jumpLocal;
        _startJumpPower = startJumpPower;
        _firstTargetJumpPower = firstTargetJumpPower;
        _secondTargetJumpPower = secondTargetJumpPower ;
        _firstTransitionFactor = firstTransitionFactor ;
        _secondTransitionFactor = secondTransitionFactor ;
        
        _startSpeed = startSpeed;
        _firstStateDistanceChange = firstStateDistanceChange;
        _secondStateDistanceChange = secondStateDistanceChange ;
    }
    private void resolveContrast()//TODO DELETE LATER
    {
        

    }
    private Vector3 reduceToDirection(Vector3 input)// to remove the possible multiplying effect of "directions"  on power variables 
    {
        float total = Math.Abs(input.x)+Math.Abs(input.y)+Math.Abs(input.z) ;
        return new Vector3(input.x / total, input.y / total, input.z / total);
    }
//----------------------------------------------------
    private Vector3 calculateVelocity(Vector3 direction,float power)
    {
        return direction * power;
    }
    private Vector3 calculateGravity()// calculates gravitational power to be apply 
    {
        if (_gravityUseActive)
        {
            if (onGround )
            {
                _gravitationalPower = gravitationalPower;// to reset build up gravitational force if grounded 
            }
            else
            {
                if (useIncreasingGravity)
                {
                    _gravitationalPower += gravityBuildUPPerSecond * Time.deltaTime;

                    if (Math.Abs(_gravitationalPower)>Math.Abs(maxGravity))
                    {
                        _gravitationalPower = maxGravity;
                    }
                }
                else  _gravitationalPower = gravitationalPower; // to able to reset if setting get changed mid air
            }

            if (gravityDirectionLocal)
            {
                //if power given it can directly calculate factor
                return getLocalDirection(gravityDirection, effectedGameObject.transform, Math.Abs(_gravitationalPower));
            }
            else
            {
                return calculateVelocity(gravityDirection, Math.Abs(_gravitationalPower));
            }
        }

        return new Vector3(0,0,0);
    }
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

        return  power * (localYDirection + localXDirection + localZDirection)/3;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect
    }

    public void resetGravity()
    {
        _gravitationalPower = gravitationalPower;
    }
    private bool checkCollisionWithLayerMask(Transform position,LayerMask layerMask,float distance)//
    { 
        return Physics.CheckSphere(position.position, distance, layerMask);
 
    }
    private void checkSurfaceTouchCondition()
    {
        bool _onGround = false;
        bool _onSurface = false;
        for (var index = 0; index < surfaceCheckPositions.Count && index < surfaceDetectionDistances.Count; index++)
        {
            var each = surfaceCheckPositions[index];
             
            
            if ( checkCollisionWithLayerMask(each, groundMask,surfaceDetectionDistances[index]))
            {
                _onGround = true;
            }
            if ( checkCollisionWithLayerMask(each, surfaceMask,surfaceDetectionDistances[index]))
            {
                _onSurface = true;
            }
            if (_onGround&&_onSurface)
            {
                break;
            }
        }

        onGround = _onGround;
        onSurface = _onSurface;
    }

  
    private bool onGroundValueChange()//detects the new ground touch 
    {
        bool result = false;
        
        if (onGroundLatestRecord!=onGround)
        {
            result = true;
            onGroundLatestRecord = onGround;

        }

        return result;
    }
    private void checkLandingForJump() 
  {
  
      if (_jumpingActive&&_jumpGroundSensitive&&onGround&&onGroundValueChange()) //detects the new touch to ground only
      {
          jumpActionCompleted();
      }
  }
    void FixedUpdate()
    {
        if (!_MasterScript_ElseSlave||!TheScriptIsActive) return;

        makeMovement(allProcess());
        
    }
    private Vector3 allProcess()
    {
        Vector3 result = new Vector3(0, 0, 0);
        if (TheScriptIsActive )
        {
            if (jumpTest )// editor testUseless
            {
                jumpTest = false;
                startJump();
            }
            if (_CancelJump) // editor testUseless
            {
                jumpPhaseCompleted();
            }

            checkSurfaceTouchCondition();

            checkLandingForJump();

            if(!bypassInHouseForces)
                result = inHouseForces();
            
            thisScriptsVelocity = result;
 
            if (_MasterScript_ElseSlave)
            {
                 collectedVelocity = collectSlaveForces();
                 result += collectedVelocity;
            }
        }

        if (_MasterScript_ElseSlave) _TheVelocity = result;

        return result;
    }
    public Vector3 getAllForces()
    {
        if (_MasterScript_ElseSlave||!TheScriptIsActive)
        {
            return new Vector3(0, 0, 0);
        }

        return allProcess();
    }
    private Vector3 collectSlaveForces()//
    {
        Vector3 result = new Vector3(0, 0, 0);
        foreach (var VARIABLE in Slaves)
        {
            if (!VARIABLE.TheScriptIsActive) continue; // TODO NEEDS TESTING
             
                result += VARIABLE.getAllForces();
        }

        return result;
    }
    private Vector3 inHouseForces()
    {
        Vector3 Velocity=new Vector3(0,0,0);
        
        if (ConsPowerIsChanging )
        {
            ConsPowerIsChanging = constantPowerIsChanging();
        }
        
        if (_moveConstantlyActive)
        {
            Vector3 constantExecuteDirection=constantDirectionLocal ? getLocalDirection(constantMovingDirection,effectedGameObject.transform) :constantMovingDirection;
            _constantMovingForce  = calculateVelocity (constantExecuteDirection,_constantPower) ;
            Velocity += _constantMovingForce;
        }
        if (_gravityUseActive)
        {
            _gravitationalForce = calculateGravity();
            Velocity += _gravitationalForce;
        }

        if (_jumpingActive)
        {
            currentJumpForce = calculateJump();

            Velocity += currentJumpForce;
        }

        return Velocity;
    }
    private Vector3 calculateJump()
    {
        Vector3 result = new Vector3(0, 0, 0);
        jumpTimeCounter += Time.deltaTime;
        float jumpTargetTime;
        float startPower;
        float targetPower;
        int transitionMode=0;
         
        if (!_JumpingInStage2)
        {
            startPower = _startJumpPower;
            targetPower = _firstTargetJumpPower;
            jumpTargetTime = _jumpingTime;
            transitionMode = _firstTransitionFactor;

        }
        else
        {
            startPower = _firstTargetJumpPower;
            targetPower = _secondTargetJumpPower;
            jumpTargetTime = _jumpSecondStateTime;
            transitionMode = _secondTransitionFactor;
        }
       

        float tempCurrentPower = calculateVariable(startPower,targetPower,jumpTimeCounter,jumpTargetTime,transitionMode);
        
        if (_jumpLocal)
        {
            result = calculateVelocity(getLocalDirection(_jumpDirection, effectedGameObject.transform),tempCurrentPower);
        }
        else
        {
            result = calculateVelocity(_jumpDirection, tempCurrentPower);
        }
        _currentJumpPower = tempCurrentPower;
        
         
        if (jumpTimeCounter>jumpTargetTime)
        {
            targetValueReached();
        }

        return result;

    }
    private float calculateVariable(float baseValue, float targetVariable,float currentTime ,float totalTime,float transitionMode=1)  
    {
        float totalDifferance = (targetVariable - baseValue);

        bool changeTowardsPositiveElseNegative= totalDifferance > 0 ? true : false;

        float totalTimeReachPercent = (currentTime/ totalTime) ;// between " 0 - 1 "

        if (transitionMode == 0) transitionMode = 1;
        transitionMode = transitionMode < 0 ? 1.0f / Math.Abs(transitionMode) : transitionMode;

        float transitionModeEffect=Mathf.Pow(totalTimeReachPercent, transitionMode)  ;

        float additionOffset = totalDifferance * transitionModeEffect ;

        offsetTest = additionOffset;
        baseTest = baseValue;

        float result = baseValue + additionOffset;

        if (changeTowardsPositiveElseNegative &&(result>=targetVariable)  )
        {
            result = targetVariable;
            // targetValueReached();// Activate this measure if current value fails to reach to target value or system stops before its reaches  
             
        }
        else if (!changeTowardsPositiveElseNegative&&(result<targetVariable)  )
        {
            result = targetVariable;
            // targetValueReached();// Activate this measure if current value fails to reach to target value or system stops before its reaches  
             
        }
        return result ;
        
    }
    private void makeMovement(Vector3 input)
    {
        characterController.Move(input * Time.deltaTime) ;
    }
    public void changeConstantPower(float newPower,bool waitTillJumpEnd,bool smoothChange,float changeTime )
    {
        _waitJumpEnd = waitTillJumpEnd;
        waitJumpEnd = waitTillJumpEnd;

        ConsPowerChangeSpeed = Math.Abs(_constantPower - newPower) / changeTime;
        
        constantPower = newPower;
        if (!waitTillJumpEnd&&!smoothChange)
        {
            ConsPowerIsChanging = false;
            _constantPower = constantPower;
            return;
        }

        ConsPowerIsChanging = true;

        _smoothChange = smoothChange;

    } 
    
    public void changeConstantPower(float newPower,bool waitTillJumpEnd,bool smoothChange )
    {
        ConsPowerChangeSpeed = Math.Abs(ConsPowerChangeSpeed);
        float time = ConsPowerChangeSpeed * Math.Abs(_constantPower - newPower);
        changeConstantPower(newPower, waitTillJumpEnd, smoothChange,time);

    }
    private bool constantPowerIsChanging()// Power Changing Method
    {
        float temp = ConsPowerChangeSpeed;
        temp *= _constantPower < constantPower ? 1 : -1;
        
        if (!_waitJumpEnd&&!_smoothChange)
        {
            _constantPower = constantPower;
            _waitJumpEnd = waitJumpEnd;
            return false;
        }
        
        if (_waitJumpEnd)
        {
            if (_jumpingActive)
            {
                return true;
            }
            else
            {
                if (!_smoothChange)
                {
                    _constantPower = constantPower;
                    _waitJumpEnd = waitJumpEnd;
                    return false;
                }

                if (!willWaitIfANewJumpHappens)// this bool decides if change starts but a new jump interrupts either change continue or will halt  
                {
                    _waitJumpEnd = false;
                }
            }
            
        }
        
        if (_smoothChange)
        {
            _constantPower += temp * Time.deltaTime;
        }

        if (Math.Abs(_constantPower - constantPower) < 1)
        {
            _waitJumpEnd = waitJumpEnd;
            return false;
        }

        return true;
    }
    
    private void targetValueReached()// TODO
    {
        jumpTimeCounter = 0;
        if (!_JumpingInStage2&&_jumpUseSecondState)
        {
            _JumpingInStage2 = true;
            //  _currentJumpPower = _firstTargetJumpPower; //TODO TEST THIS ıf needed 
        }
        else
        {
            _JumpingInStage2 = false;
            jumpActionCompleted(); // jump ended
        }
    }
    public void startJump( )//TODO its for checking if there is a jumping phase before call actual jump function
    {
        if(_jumpPhaseInUse||!jumpCanBeUsed) return; // if jump is already in use or waiting for continues jump cant be called / if not permitted of use cant be called
        getJumpOriginalSettings();
        _jumpPhaseInUse = true;
        _jump();

    }
     

    public void customJump(bool forceTheJump=false)
    {
        if (forceTheJump && _jumpingActive) jumpPhaseCompleted();
        
        _jumpPhaseInUse = true;
        _jump();

    }
    private void _jump()
    {
        
        if(_repeatJump)
        {
            if (_remainingJumpRepeats<1)
            {
                jumpPhaseCompleted();
                return;
            }
            _remainingJumpRepeats--;
        }
        
        if (_jumpPhaseInUse&&!_jumpingActive)
        {
            if (useDistanceChangeTypeParameter) adaptOldParameters();
             
          //  Debug.Log("jump started");
             
            _jumpingActive = true;
            _JumpingInStage2 = false;
            jumpTimeCounter = 0;

            _moveConstantlyActive = _jumpsConstantActiveDuring;

            _gravityUseActive = _jumpsGravityActiveDuring;

            return;
        }
        //Debug.Log("cant jump");
    }

    private void adaptOldParameters()
    {
        if (_firstTransitionFactor == 0) _firstTransitionFactor = 1;
        if (_secondTransitionFactor == 0) _secondTransitionFactor = 1;
        _startJumpPower = _startSpeed;
        
        _firstTargetJumpPower = calculateTargetPower(_startJumpPower,_jumpingTime,_firstStateDistanceChange,_firstTransitionFactor);
        _secondTargetJumpPower= calculateTargetPower(_firstTargetJumpPower,_jumpSecondStateTime,_secondStateDistanceChange,_secondTransitionFactor);
     
     
    }

    private float calculateTargetPower(float baseSpeed,float actionTime,float wantedDistance,float parabolaFactor)
    {
        bool squareIntegralElsePower=parabolaFactor < 0  ;
        float tempParabolaFactor = squareIntegralElsePower ? Math.Abs(parabolaFactor) : parabolaFactor;
        if (squareIntegralElsePower)
        {
            return (  ((((wantedDistance/actionTime)-baseSpeed)*(tempParabolaFactor+1))/tempParabolaFactor+baseSpeed)         ); 
        }
        else
        {
            return (    ((wantedDistance/actionTime)-baseSpeed)*(tempParabolaFactor+1)+baseSpeed         ); 
        }
        
        
    }
    private void jumpActionCompleted() 
    {
        if (_repeatJump)
        {
            _jumpingActive = false; 
             
            if (_remainingJumpRepeats<=0)
            {
                jumpPhaseCompleted();
                return;
            }
            if (_jumpsConstantActiveBetween)
            {
                _moveConstantlyActive = true;
            }
            if (_jumpsGravityActiveBetween)
            {
                _gravityUseActive = true;
            }
            //Debug.Log(" will jump again");
            Invoke(nameof( _jump ),waitBetweenJumps);
        }
        else
        {
            jumpPhaseCompleted();
        }
 
    }
    private void jumpPhaseCompleted()
    {
        _jumpPhaseInUse = false;
        _jumpingActive = false;
        _JumpingInStage2 = false;
        _moveConstantlyActive = moveConstantlyActive ;
        _gravityUseActive = gravityUseActive ;
        getJumpOriginalSettings();
        
    }    
   
    public void forceJumping() // stops ongoing jumps and start jump over again also doesnt effect by jump lock a.k.a "jumpCanBeUsed"
    {
        jumpPhaseCompleted();
        _jumpPhaseInUse = true;
        _jump();
    }

    public void cancelJumpNow()  
    {
        jumpPhaseCompleted();
    }
    
    public void cancelNextAutoJump()
    {
        _repeatJump = false;
    }
    
    public float getGravityBasePower()
    {
        return gravitationalPower;
    }
    public void setGravityBasePower(float input)
    {
         gravitationalPower=input;
    }

    public float getJumpingBaseTime()
    {
        return jumpingTime;
    }
    public void setJumpingBaseTime(float input)
    {
        jumpingTime = input;
    }

    public float getBaseStartSpeed()
    {
        return startSpeed;
    }
    public void setBaseStartSpeed(float input)
    {
        startSpeed = input;
    }

    public float getBaseFirstStateDistanceChange()
    {
        return firstStateDistanceChange;
    }
    public void setBaseFirstStateDistanceChange(float input)
    {
        firstStateDistanceChange = input;
    }

    public float getBaseStartJumpPower()
    {
        return startJumpPower;
    }

    public void setBaseStartJumpPower(float input)
    {
        startJumpPower = input;
    }
 
    

}

 