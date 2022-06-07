using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager001 : MonoBehaviour
{
    [SerializeField] private bool PlayerAirBorn;
    [SerializeField] private bool PlayerTakeHit;
     
    [Header("  ")] 
    [SerializeField] private bool TouchActive;
    
    [Header("  ")] 
    [SerializeField] private bool RunningActive;
    [Header("  ")] 
    [SerializeField] private bool HighJumpActive;
    [Header("  ")] 
    [SerializeField] private bool GlideActive;
    [SerializeField] private bool CanGlide;
 
    [Header("  ")] 
    [SerializeField] private bool BasicJumpActive ;
    [SerializeField] private bool CanBasicJump;
    [SerializeField] private int PermittedJumpTimes;
    [Header("  ")] 
    [SerializeField] private bool  FastFallActive=false;
    [SerializeField] private bool  CanFastFall;
    
    [Header("  ")] 
    
    [SerializeField] private CCFeedControl PlayerJumpController;
    [SerializeField] private  GroundFinder _GroundFinder;


    [SerializeField] private int Basic_CC_JumpType;
    [SerializeField] private int Basic_NCC_JumpType;
    [SerializeField] private int GlideCC_type;
    
    [SerializeField] private int FastFallCCType;
    [SerializeField] private int FreeFallCCType;
    [SerializeField] private int HighJumpType;
    
    [Header("  ")] 
    [SerializeField] private float HighJumpTotalTime;

     
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        VariableUpdate();
        StateControlTakeAction();

    }

   
    private void basicJump()
    {
        
        // activate basic jump as solo jump basic jump will cancel ccmovement constant
        // activate jump animation but with execution time to 
        BasicJumpActive = true;
        RunningActive = false;

    }
    //------------------------------------ VARIABLE UPDATE -------------------------------
    private void VariableUpdate()
    {
        if (PlayerAirBorn&&_GroundFinder.GroundTouch)
        {
            PlayerAirBorn = false;
          //   LastTreadMill = _GroundFinder.TouchedGround.GetComponent<TreadMill>();
            
        }
        
        
    }
    
    //---------------------------------- STATE CONTROL -------------------------------------

    private void StateControlTakeAction()
    {
        if (FirstTouch())
        {
            
        }
        if (TouchHold())
        {
            
        }

        if (TouchRelease())
        {
            
        }
    }

    //-------------------------------- "    INPUT TAKING -----------------------------------------------
    private bool FirstTouch()
    {
        if (!TouchActive && Input.GetMouseButton(0))
        {
            TouchActive = true;
            return true;
        }
        return false;
        
    }
    private bool TouchHold()
    {
        return (TouchActive && Input.GetMouseButton(0));
    }

    private bool TouchRelease()
    {
        if (TouchActive && !Input.GetMouseButton(0))
        {
            TouchActive = false;
            return true;
        }

        return false;
    }

    public void BehaviorEnded(int BehaviorType)
    {
        Debug.Log(" ENDED BEHAVIOR TYPE  :  "+BehaviorType);
        if (BehaviorType==HighJumpType)
        {
            if (TouchActive)
            {
                StartGlide();
            }
            else
            {
                StartFreeFall();
            }
        }
    }
    public void MovementEnded()
    {
        
    }

    private void StartGlide()
    {
        CCMovement temp = PlayerJumpController.startMovementFeed_RAF(GlideCC_type,true);
         temp.makeTransition(PlayerJumpController.MasterScript._TheVelocity);
         //TODO ANIMATOR ACTION !!!
    }

    private void StartFreeFall()
    {
        CCMovement temp = PlayerJumpController.startMovementFeed_RAF(FreeFallCCType,true);
        temp.makeTransition(PlayerJumpController.MasterScript._TheVelocity);
        //TODO ANIMATOR ACTION !!!
        
        
    }
    private void StartFastFall()
    {
        CCMovement temp = PlayerJumpController.startMovementFeed_RAF(GlideCC_type,true);
        temp.makeTransition(PlayerJumpController.MasterScript._TheVelocity);
        //TODO ANIMATOR ACTION !!!
    }
    private void StartRunnıng()
    {
        PlayerAirBorn = false;
         PlayerJumpController.emptyActiveBehaviorFeeds();
         PlayerJumpController.emptyActiveMovementFeeds();
         //TODO START IN TOUCH TRAID MILE MODE HERE
         //TODO ANIMATOR ACTION !!!
    }
    private void StartBasıcJump()
    {
        if (CanBasicJump)
        {
            if (PlayerAirBorn)
            {
                 
                PlayerJumpController.emptyActiveMovementFeeds();
                PlayerJumpController.startBehaviorFeed(Basic_NCC_JumpType, true);
            }
            else
            {
                PlayerJumpController.startBehaviorFeed(Basic_CC_JumpType, true);
            }
           
        }
    }

    public void StartHighJump()
    {
        if (FastFallActive)
        {
            
        }
        else
        {
            StartRunnıng();
        }
    }
     
}
