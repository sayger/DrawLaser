 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.Serialization;

 public class PresetMove : MonoBehaviour
{
    public int id = 0;
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string descriptionTextArea;
    public bool jumpTest;
    public bool jumpingStateInUse;
    public PlayerMovement masterScript;
    public bool useMasterByPass;
  //  public float totalPlaceHolderJumpTime;//TODO special request for jump conditions can be made
    //TODO jump request to slaves can be made by standard method or force jump method
    public bool RepeatJumps;
    private int _RemainingJumps;
    public int RemainingJumps;

    public List<PlayerMovement> slaveScripts = new List<PlayerMovement>();

  
    
    void Start()
    {
          
     //   LevelEvents.Instance.JumpLimitationActions += CancelRemainingJumps;
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (jumpTest)
        {
            jumpTest = false;
            executePresetJumps(id);
        }

        if (jumpingStateInUse)
        {
            checkJumpEnd();
        }
        
    }

    private void checkJumpEnd()
    {
        bool jumpEnded = true;
        foreach (var VARIABLE in slaveScripts)
        {
            if (VARIABLE._jumpPhaseInUse)
            {
                jumpEnded = false;
                break;
            }
           
        }

        if (jumpEnded)
        {
            if (RepeatJumps)
            {
                repeater();
            }
            else
            {
                jumpPhaseEnded();
            }
        }

    }
    public void executePresetJumps(int _id)
    {
        if (id==_id)
        {
            jumpingStateInUse = true;
            if (RepeatJumps)
            {
                repeater();
            }
            else
            {
                callJumps();
            }
             
        }
    }
    public void executePresetJumps( )
    {
        executePresetJumps(id);
    }
     
    public void repeater()
    {
        if (RemainingJumps > 0)
        {
            RemainingJumps--;
          //  powerUpUI.ChangeFillAmount();// TODO DELETE AFTER PROJECT
            callJumps();
        }
        else jumpPhaseEnded();

    }

    private void jumpEnd()
    {
        if (RepeatJumps)
        {
            repeater();
        }
        else jumpPhaseEnded();
         
    }

    private void jumpPhaseEnded()
    {
        jumpingStateInUse = false;
        RemainingJumps = _RemainingJumps;
        foreach (var VARIABLE in slaveScripts)
        {
            if (VARIABLE.jumpSlave)
            {
                VARIABLE.TheScriptIsActive = false;
            }
        }
        if (useMasterByPass)
        {
            masterScript.bypassInHouseForces = false;
        }
        
        
    //    LevelEvents.Instance.jumpCompleted();
    }
    
    private void callJumps()
    {
        if (useMasterByPass)
        {
          /*  masterScript._jumpDirection = new Vector3(0, 0, 0);
            masterScript._repeatJump = false;
            masterScript._jumpUseSecondState = false;
            masterScript._jumpsGravityActiveDuring = false;
            masterScript._jumpsConstantActiveDuring = false;
            masterScript._jumpingTime = totalPlaceHolderJumpTime;
            
            masterScript.customJump();*/
          masterScript.bypassInHouseForces = true;
        }
        foreach (var VARIABLE in slaveScripts)
        {
            if (VARIABLE.jumpSlave)
            {
                VARIABLE.TheScriptIsActive = true;
            }
            
            VARIABLE.customJump();
        }

        ReportRemainingJumps();
    }

    private void adjustments()
    {
        foreach (var VARIABLE in slaveScripts)
        {
            VARIABLE._MasterScript_ElseSlave = false;
        }

        if (RemainingJumps < 1) RemainingJumps = 1;
        _RemainingJumps = RemainingJumps;

    }

    public void CancelRemainingJumps()
    {
        if (jumpingStateInUse)
        {
            RemainingJumps = 0;
        }
    }

    public void ReportRemainingJumps()
    {
        int report = RepeatJumps ? RemainingJumps : 0;
        
     //   LevelEvents.Instance.playerEachJumpReport(report);
    }
    
    
}
