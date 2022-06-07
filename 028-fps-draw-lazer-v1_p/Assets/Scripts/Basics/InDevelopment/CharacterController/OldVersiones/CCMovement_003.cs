using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMovement_003 : MonoBehaviour
{ 
    /*
    [Header("  ")]
    public bool TheScriptIsActive=true; //Means Script active or not, if false Scripts all active state properties will be incapable of effect
    [SerializeField] private CharacterController characterController;// CController
    private GameObject effectedGameObject ; // MasterObject
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;
    [Header(" INDICATORS FOR VELOCITY  ")]
    public Vector3 _TheVelocity;//   will executed movement as currentVelocity at the moment // just an indicator of it
    public Vector3 thisScriptsVelocity ; //  ThisScriptsVelocity  in house forces combine velocity
    public Vector3 collectedVelocity ; // CollectedVelocity  combined Slave script velocities 
    
    
    //------------------
    
    [Header(" ESSENTIAL SETTINGS  ")]
    
    public bool _MasterScript_ElseSlave=true;// if its not master script only can be use as additional jump feature to a MASTER SCRIPT                        //-*-// IMPORTANT  (non master scripts refers to masters scripts gameObject when localized calculations)
    public CCMovement_003 SupremeMasterScript;
    
    [SerializeField] private bool UseCollectedPower;//usePresetMoves
    public CCFeedControl ccFeedController; 
    public bool bypassInHouseForces;//BypassInHouseForces
    
    
    
    //------------------
   
    [Header(" CONSTANT MOVEMENT SETTINGS  ")]
    public bool _moveConstantlyActive = true; // use given direction to move constantly special jump parameters could override this if so will revert back to its original setting when jump phase over
    private bool moveConstantlyActive ; // original reference setting                         //-*-// *REASON FOR ALL ORIGINAL SETTINGS ARE BEEN KEPT ESSENTIALLY IS FOR MAKE POSSIBLE THE SPECIAL PARAMETER JUMP EVENTS //-*-// 

    public Vector3 constantMovingDirection=new Vector3(0,0,1); // desired direction for nonstop movement
    private Vector3 targetConstantMovingDirection ; //TODO TargetConstant Direction
    private Vector3 _constantMovingForce;// current resulted "currentVelocity" additive to ready to use in player controller move 
    public bool constantDirectionLocal = true; // if true will make given direction execute in local sense
    
    public float _constantPower; // speed factor for constant movement direction at the momnet// variance value
    private float constantPower; // original reference value //TODO TargetConstantPower

    //-----------------

    [Header(" ADJUSTABLE GRAVITY SETTINGS  ")]
    public bool _gravityUseActive;  // use given gravity settings or not ,special jump parameters could override this 
    private bool gravityUseActive; // original reference setting

    public Vector3 gravityDirection = new Vector3(0,-1,0);
    private Vector3 targetGravityDirection;
     
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

    //---------------------------------------------------------
    [Header(" CONSTANT POWER IN GAME UPDATE SETTINGS   ")]//TODO MAYBE CAN BE MADE PRIVATE AND EDITOR USE, COULD PROVIDED WITH HELP OF AN OTHER SCRIPT
     
     private bool ConsDirectionIsChanging; 
     private bool ConsPowerIsChanging; 
     private bool changeConsDirectionNow;
     private bool changeConsPowerNow;
     private float ConsPowerChangeSpeed; 
     private float ConsDirectionChangeSpeed;
     
     private bool GravityDirectionIsChanging; 
     private bool GravityPowerIsChanging; 
     private bool changeGravityDirectionNow;
     private bool changeGravityPowerNow;
     private float GravityPowerChangeSpeed; 
     private float GravityDirectionChangeSpeed;
     
     
     [Header(" SLAVE SETTINGS ")]
     [SerializeField] private float changeSpeed;
    
     [SerializeField] private Vector3 overWrittenVelocity;
    
     [SerializeField] private Vector3 startValues;
    
     [SerializeField] private bool transitionMode;
    
     [SerializeField]private int TypeId;// TODO ?
     private CCFeedControl ManagerScript;
     private int ManagerId;// TODO ?
     
    
    void Start()
    {
        Adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_MasterScript_ElseSlave||!TheScriptIsActive) return;
       
        makeMovement(AllProcess());

    }
    
    //------------------------------Main Method -------------------
    public Vector3 AllProcess()
    {
        Vector3 result = new Vector3(0, 0, 0);
     
        checkSurfaceTouchCondition();

        InPlayValueChanges();
        
        if(!bypassInHouseForces)
            result += inHouseForces();
            
        thisScriptsVelocity = result;
  
        collectedVelocity = collectFeed();
        result += collectedVelocity;

        if (transitionMode)
        {
            overWrittenVelocity = Vector3.MoveTowards(overWrittenVelocity, result, changeSpeed *Time.deltaTime); // *Time.deltaTime
            if (Vector3.Distance(overWrittenVelocity,result)<0.1)
            {
                transitionMode = false;
            }

            result = overWrittenVelocity;
        }

        _TheVelocity = result;
        return result;
    }

    private Vector3 inHouseForces()
    {
        Vector3 Velocity=new Vector3(0,0,0);

        if (_moveConstantlyActive)
        {
            _constantMovingForce  = calculateConstant (constantMovingDirection,_constantPower,constantDirectionLocal) ;
            Velocity += _constantMovingForce;
        }
        if (_gravityUseActive)
        {
            _gravitationalForce = calculateGravity();
            Velocity += _gravitationalForce;
        }

        return Velocity;
    }
    //--------------------------------Velocity CalX ------------

    
    private Vector3 calculateConstant(Vector3 Direction,float Power,bool localCalculation)
    {
        if (localCalculation)
        {
            return calculateVelocity(getLocalDirection(Direction,effectedGameObject.transform), Power);
        }
        else return  calculateVelocity( Direction , Power);
         
    }
    private Vector3 calculateGravity()// calculates gravitational power to be apply 
    {
        if (_gravityUseActive)
        {
            
            if (useIncreasingGravity&&!onGround)
            {
                _gravitationalPower += gravityBuildUPPerSecond * Time.deltaTime;

                if (Math.Abs(_gravitationalPower)>Math.Abs(maxGravity))
                {
                    _gravitationalPower = maxGravity;
                }
            }
            else  _gravitationalPower = gravitationalPower; // to able to reset if setting get changed mid air
             

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
   
  
    
    //------------------------------ MATH TRANSLATIONS ------------------------
    
    private Vector3 reduceToDirection(Vector3 input)// to remove the possible multiplying effect of "directions"  on power variables 
    {
        float total = Math.Abs(input.x)+Math.Abs(input.y)+Math.Abs(input.z) ;
        return new Vector3(input.x / total, input.y / total, input.z / total);
    }
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }

    private Vector3 calculateVelocity(Vector3 direction,float power)
    {
        return direction * power;
    }

    //------------------------------ FEED GIVING ------------------------
    
    public Vector3 getAllForces()
    {
        if (_MasterScript_ElseSlave||!TheScriptIsActive)
        {
            return new Vector3(0, 0, 0);
        }
        return AllProcess();
        
    }
    //------------------------------ FEED TAKING ------------------------
    private Vector3 collectFeed()
    {
        if (UseCollectedPower)
        {
            return ccFeedController.CollectActiveFeedVelocities();
        }
        return new Vector3(0, 0, 0);
        
    }

    public void setMasters(CCMovement_003 supreme, GameObject masterG,CCFeedControl manager)
    {
        effectedGameObject = masterG;
        SupremeMasterScript = supreme;
        ManagerScript = manager;
        _MasterScript_ElseSlave = false;
    }
    //-------------------------------- Movement Maker --------------------

    private void makeMovement(Vector3 input)
    {
        characterController.Move(input * Time.deltaTime) ;
    }

    //---------------------------------- CONDITION CHECK ----------------------------,
    
    private void checkSurfaceTouchCondition()
    {
        if (!_MasterScript_ElseSlave)
        {
            onGround =SupremeMasterScript.onGround;
            onSurface = SupremeMasterScript.onSurface;
            return;
        }
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
    
    private bool checkCollisionWithLayerMask(Transform position,LayerMask layerMask,float distance)//
    { 
        return Physics.CheckSphere(position.position, distance, layerMask);
 
    }
    
    //------------------------------- SELF GOVERNING VALUE CHANGE METHODS ---------------- // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
    private void InPlayValueChanges()
    {
        
        
    }

    private void ConstantPowerChanges()
    {
        
    }
    private void ConstantDirectionChanges()
    {
        
    }
    
    private void GravityMinPowerChanges()
    {
        
    }
    private void GravityDirectionChanges()
    {
        
    }
    //------------------------------ IN CODE REQUESTS ------------------------ // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
    //----------------- CONSTANT ---- 
    public void setConstantPower(float newPower)
    {
        _constantPower = newPower;
        constantPower = newPower;
        ConsPowerIsChanging = false;
        
    }

    public void setConstantPower(float newPower, float changeTime, bool whenItActive = true)
    {
        //TODO
    }

    public void setConstantDirection(Vector3 newDirection)
    {
        constantMovingDirection = newDirection ;
        targetConstantMovingDirection = newDirection;
        ConsDirectionIsChanging = false;
    }

    public void setConstantDirection(Vector3 newDirection, float changeTime, bool whenItActive = true)
    {
        //TODO
    }

    public void setConstant(Vector3 newDirection, float newPower)
    {
        setConstantPower(newPower);
        setConstantDirection(newDirection);
    }

    public void setConstant(Vector3 newDirection, float newPower, float changeTime, bool whenItActive = true)
    {
        setConstantPower(newPower,changeTime,whenItActive);
        setConstantDirection(newDirection,changeTime,whenItActive);
        
    }
    //----------------- GRAVITY ---- 
    
    public void setGravityBasePower(float newPower)
    {
        _gravitationalPower = newPower;
        gravitationalPower = newPower;
        GravityPowerIsChanging = false;
         
    }

    public void setGravityBasePower(float newPower, float changeTime, bool whenItActive = true)
    {
        //TODO
    }

    public void setGravityDirection(Vector3 newDirection)
    {
        gravityDirection = newDirection ;
        targetGravityDirection = newDirection;
        GravityDirectionIsChanging = false;
    }

    public void setGravityDirection(Vector3 newDirection, float changeTime, bool whenItActive = true)
    {
        //TODO
    }

    public void setGravity (Vector3 newDirection, float newPower)
    {
        setConstantPower(newPower);
        setConstantDirection(newDirection);
    }

    public void setGravity(Vector3 newDirection, float newPower, float changeTime, bool whenItActive = true)
    {
        setConstantPower(newPower,changeTime,whenItActive);
        setConstantDirection(newDirection,changeTime,whenItActive);
        
    }
    
    //------------------------------ADJUSTMENT SETTINGS ------------------------
    private void Adjustments() // to save initial settings and solve possible conflicts within settings
    {
        checkEssentialElements();
        
        SetBehaviorController(ccFeedController);

        fixMustFixedElements();
        
        checkGravitySettings();
        
        fixSurfaceCheckPositions();

        setOriginalSettingsFromInitialValues(); 
         
         
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
        }
        else
        {
            characterController = null;
        }
    }
    private void SetBehaviorController(CCFeedControl input)
    {
        if (input!=null)
        {
            input.InformFeeds(this,effectedGameObject);
            
        }
        else
        {
            UseCollectedPower = false;
        }
    }
    private void fixMustFixedElements()
    {
        ConsPowerChangeSpeed = Math.Abs(ConsPowerChangeSpeed);
        
        constantMovingDirection = reduceToDirection(constantMovingDirection) ; // To make direction values purely on pointing directions but not effecting power factors//TODO MUST CHECK OUTER SETUPS whıle settıng new directions 
        gravityDirection = reduceToDirection(gravityDirection) ;
    }

    private void checkGravitySettings()// to set gravity power variables 
    {
        gravitationalPower=Math.Abs(gravitationalPower);
        gravityBuildUPPerSecond = Math.Abs(gravityBuildUPPerSecond);
        ExtraPowerToCalculateMaxGravity=  Math.Abs(ExtraPowerToCalculateMaxGravity);
        maxGravity = gravitationalPower + ExtraPowerToCalculateMaxGravity;
    }
    private void fixSurfaceCheckPositions() // to get surface check points necessary values for later use 
    {
        if (surfaceCheckPositions.Count==0)
        {
            useIncreasingGravity = false; // if cant check landing some features cant be used
             

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
    private void setOriginalSettingsFromInitialValues() // to save editor settings as original settings  
    {
        moveConstantlyActive =   _moveConstantlyActive ; //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH SO IF IN JUMP STATE JUST CHANGE REFERENCE IF NOT CHANGE BOTH 
        gravityUseActive = _gravityUseActive;     //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH
        constantPower = _constantPower ;   
        gravitationalPower = _gravitationalPower;  //TODO WHEN CHANGING THIS MUST BE CONSIDER TO MAKE IT EFFECT WITH OUT A JUMP START OR END MUST CHANGE BOTH SO IF IN JUMP STATE JUST CHANGE REFERENCE IF NOT CHANGE BOTH 
        targetConstantMovingDirection = constantMovingDirection ; //TODO TargetConstant Direction
        targetGravityDirection = gravityDirection;

    }
    public int getManagerId()
    {
        return ManagerId;
    }
    public void setManagerId(int newID)
    {
        ManagerId = newID;
    }
    public int getTypeId()
    {
        return TypeId;
    }
    public void setTypeId(int newID)
    {
        TypeId = newID;
    }
    public void InsertItself(CCMovement_003 dormantScript)// TODO must do some stuff like cloning inferiors as well
    {
        
        dormantScript.changeSpeed=changeSpeed;
        dormantScript.overWrittenVelocity=overWrittenVelocity;
        dormantScript.startValues=startValues;
        dormantScript.transitionMode=transitionMode;
        
        dormantScript.TypeId = TypeId;  
        dormantScript.TheScriptIsActive =TheScriptIsActive ; 
        dormantScript.characterController=characterController ; 
        dormantScript.effectedGameObject = effectedGameObject;  
        dormantScript.descriptionTextArea= descriptionTextArea; 
        dormantScript._TheVelocity= _TheVelocity; 
        dormantScript.thisScriptsVelocity = thisScriptsVelocity; 
        dormantScript.collectedVelocity =collectedVelocity ; 
        dormantScript._MasterScript_ElseSlave= _MasterScript_ElseSlave ; 
        dormantScript.SupremeMasterScript=SupremeMasterScript ; 
        dormantScript.UseCollectedPower= UseCollectedPower; 
        dormantScript.ccFeedController= ccFeedController; 
        dormantScript.bypassInHouseForces= bypassInHouseForces;  
        dormantScript._moveConstantlyActive =  _moveConstantlyActive;  
        dormantScript.moveConstantlyActive = moveConstantlyActive;   
        dormantScript.constantMovingDirection= constantMovingDirection; 
        dormantScript.targetConstantMovingDirection = targetConstantMovingDirection;  
        dormantScript._constantMovingForce=_constantMovingForce ;  
        dormantScript.constantDirectionLocal = constantDirectionLocal;  
        dormantScript._constantPower=_constantPower ; 
        dormantScript.constantPower=constantPower ;   
        dormantScript._gravityUseActive=_gravityUseActive ;    
        dormantScript.gravityUseActive=gravityUseActive ;   
        dormantScript.gravityDirection =gravityDirection ;
        dormantScript.targetGravityDirection=targetGravityDirection ; 
        dormantScript._gravitationalForce =_gravitationalForce ;   
        dormantScript.gravityDirectionLocal  =gravityDirectionLocal ;  
        dormantScript._gravitationalPower= _gravitationalPower;
        dormantScript.gravitationalPower =gravitationalPower ;   
        dormantScript.useIncreasingGravity= useIncreasingGravity;  
        dormantScript.gravityBuildUPPerSecond=gravityBuildUPPerSecond ;
        dormantScript.ExtraPowerToCalculateMaxGravity= ExtraPowerToCalculateMaxGravity ;
        dormantScript.maxGravity = maxGravity;   
        dormantScript.surfaceCheckPositions= surfaceCheckPositions; 
        dormantScript.defaultSurfaceCheckDistance= defaultSurfaceCheckDistance;
        dormantScript.surfaceDetectionDistances =surfaceDetectionDistances ;    
        dormantScript.groundMask=groundMask ;  
        dormantScript.surfaceMask=surfaceMask ;  
        dormantScript.onGround= onGround;     
        dormantScript.onGroundLatestRecord=onGroundLatestRecord ;    
        dormantScript.onSurface= onSurface;  
        dormantScript.ConsDirectionIsChanging=ConsDirectionIsChanging ; 
        dormantScript.ConsPowerIsChanging= ConsPowerIsChanging;
        dormantScript.changeConsDirectionNow=changeConsDirectionNow ;
        dormantScript.changeConsPowerNow=changeConsPowerNow ;
        dormantScript.ConsPowerChangeSpeed= ConsPowerChangeSpeed; 
        dormantScript.ConsDirectionChangeSpeed= ConsDirectionChangeSpeed; 
        dormantScript.GravityDirectionIsChanging= GravityDirectionIsChanging; 
        dormantScript.GravityPowerIsChanging= GravityPowerIsChanging; 
        dormantScript.changeGravityDirectionNow=changeGravityDirectionNow ;
        dormantScript.changeGravityPowerNow=changeGravityPowerNow ;
        dormantScript.GravityPowerChangeSpeed=GravityPowerChangeSpeed ; 
        dormantScript.GravityDirectionChangeSpeed=GravityDirectionChangeSpeed ;
  
        
    }

    public void takeYourLeave()
    {
        ManagerScript.endMovementFeed(this);
    }

    public void makeTransition(Vector3 startPoint)
    {
        Debug.Log("Taken Start Point : "+startPoint);
        transitionMode = true;
        overWrittenVelocity = startPoint;
    }*/
}
