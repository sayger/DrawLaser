using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFeedControl : MonoBehaviour
{
    [SerializeField] private PlayerManager001 playerManager001;
    private bool usePlayerManager;
    private bool ScriptConnected;
    public int id = 0;
    [SerializeField] private bool BehaviorFeedsActive;
    [SerializeField] private bool MovementFeedsActive;
    public Vector3 thisScriptsVelocity; //  in house forces combine velocity
    [Header("  ")]
    public CCMovement MasterScript;
    [SerializeField] private GameObject MasterObject;
    [SerializeField] private float countDown=10;// TODO ???
     
    
    [Header("  ")]
    [SerializeField] private bool ActivateBehavior;
    [SerializeField] private bool DeActivateBehavior;
    [SerializeField] private int bTestNo;
    [SerializeField] private List<CCBezierBehaviourFeed>  BehaviorFeeds = new List<CCBezierBehaviourFeed>();
    [SerializeField] private int NumberOfActiveBehaviors;
    [SerializeField] private List<CCBezierBehaviourFeed>  ActiveBehaviorFeeds = new List<CCBezierBehaviourFeed>(); //TODO MAKE PRIVATE
    
    [Header("  ")] 
    [SerializeField] private bool ActivateMovement;
    [SerializeField] private bool DeActivateMovement;
    [SerializeField] private int mTestNo;
    [SerializeField] private List<CCMovement>  MovementFeeds = new List<CCMovement>();
    [SerializeField] private int NumberOfActiveMovements;
    [SerializeField] private List<CCMovement>  ActiveMovementFeeds = new List<CCMovement>();  //TODO MAKE PRIVATE

    [SerializeField] private int BypassRequests;

    [SerializeField] private List<CCBezierBehaviourFeed>  DormantBehaviorFeeds = new List<CCBezierBehaviourFeed>();
    [SerializeField] private List<CCMovement>  DormantMovementFeeds = new List<CCMovement>();
    
    
    void Start()
    {
        adjustments();
    }
    void FixedUpdate()
    {
        testController();

    }

    private void testController()
    {
        
        if (ActivateBehavior)
        {
            ActivateBehavior = false;
            if (!ScriptConnected)
            {
                Debug.Log("Script Not Connected");
                return;
            }

            startBehaviorFeed(bTestNo);
        }
        if (ActivateMovement)
        {
            ActivateMovement = false;
            if (!ScriptConnected)
            {
                Debug.Log("Script Not Connected");
                return;
            }

            startMovementFeed(mTestNo);
        }
        
        
        
        if (DeActivateBehavior)
        {
            DeActivateBehavior = false;
            if (!ScriptConnected)
            {
                Debug.Log("Script Not Connected");
                return;
            }

            endBehaviorFeed(bTestNo,false);
        }
        if (DeActivateMovement)
        {
            DeActivateMovement = false;
            if (!ScriptConnected)
            {
                Debug.Log("Script Not Connected");
                return;
            }

            endMovementFeed(mTestNo,false);
        }
        
    }
    
    private void adjustments()
    {
      
        NumberOfActiveBehaviors = 0;
        ActiveBehaviorFeeds = new List<CCBezierBehaviourFeed>();
        
        NumberOfActiveMovements = 0;
        ActiveMovementFeeds = new List<CCMovement>();

        if (playerManager001!=null)
        {
            usePlayerManager = true;
        }

    }
    
    public Vector3 CollectActiveFeedVelocities()
    {
        checkActiveFeeds();
        thisScriptsVelocity = new Vector3(0, 0, 0);
        if (!BehaviorFeedsActive&&!MovementFeedsActive) return  thisScriptsVelocity;

        if (BehaviorFeedsActive)
        {
            thisScriptsVelocity += getActiveBehaviorFeeds();
        }

        if (MovementFeedsActive)
        {
            thisScriptsVelocity += getActiveMovementFeeds();
        }
        

        return thisScriptsVelocity;
        
    }

    private Vector3 getActiveMovementFeeds()
    {
        Vector3 result=new Vector3();
        foreach (var VARIABLE in ActiveMovementFeeds)
        {
            result += VARIABLE.AllProcess();
        }

        return result;
    }
    private Vector3 getActiveBehaviorFeeds()
    {
        Vector3 result=new Vector3();
        foreach (var VARIABLE in ActiveBehaviorFeeds)
        {
            result += VARIABLE.SourceFeed  ();
        }

        return result;
    }
    //---------------------------------------------

    public void InformFeeds(CCMovement MasterS, GameObject MasterG)
    {
        MasterScript = MasterS;
        MasterObject = MasterG;
        foreach (var VARIABLE in BehaviorFeeds)
        {
            VARIABLE.setMasters(MasterScript, MasterObject,this);
        }

        foreach (var VARIABLE in MovementFeeds)
        {
            VARIABLE.setMasters(MasterScript, MasterObject,this);

        }

        ScriptConnected = true;

    }
    
    private void checkActiveFeeds()
    {
        NumberOfActiveBehaviors = ActiveBehaviorFeeds.Count;
        NumberOfActiveMovements = ActiveMovementFeeds.Count;
         
        BehaviorFeedsActive=NumberOfActiveBehaviors > 0;
        MovementFeedsActive=NumberOfActiveMovements > 0;

    }

    //------------------------START FEEDS ---------------------/*****
    //---------------BEHAVIOR FEEDS----------
    public void startBehaviorFeed()
    {
        for (int i = 0; i < BehaviorFeeds.Count; i++)
        {
            startBehaviorFeed(i);
        }
    }
    public int startBehaviorFeed(CCBezierBehaviourFeed newFeed,bool startAsSolo=false)
    {
        addToFeeds(newFeed);
        startBehaviorFeed(BehaviorFeeds.Count - 1, startAsSolo );
        return BehaviorFeeds.Count - 1;
    }
    public void startBehaviorFeed(int number,bool startAsSolo=false)
    { 
        if (startAsSolo)
        {
            emptyActiveBehaviorFeeds();
        }
        addToActiveFeeds(BehaviorFeeds[number],number);
    }
    
    //---Return Active Version----
    public CCBezierBehaviourFeed startBehaviorFeed_RAF(CCBezierBehaviourFeed newFeed,bool startAsSolo=false)
    {
        addToFeeds(newFeed);
        
        return startBehaviorFeed_RAF(BehaviorFeeds.Count - 1, startAsSolo );;
    }
    public CCBezierBehaviourFeed startBehaviorFeed_RAF(int number,bool startAsSolo=false)
    { 
        if (startAsSolo)
        {
            emptyActiveBehaviorFeeds();
        }
        return addToActiveFeeds_RAF(BehaviorFeeds[number],number);
        
    }
    
    //---------------MOVEMENT FEEDS----------
    public void startMovementFeed()
    {
        for (int i = 0; i < MovementFeeds.Count; i++)
        {
            startMovementFeed(i);
        }

    }
    public int startMovementFeed(CCMovement newFeed,bool startAsSolo=false)
    {
        addToFeeds(newFeed);
        startMovementFeed(MovementFeeds.Count-1,  startAsSolo );
        return MovementFeeds.Count - 1;
    }
    public void startMovementFeed(int number,bool startAsSolo=false)
    {
        if (startAsSolo)
        {
            emptyActiveMovementFeeds();
        }
        addToActiveFeeds(MovementFeeds[number],number);

    }
    //---Return Active Version----
    
    public CCMovement startMovementFeed_RAF(CCMovement newFeed,bool startAsSolo=false)
    {
        addToFeeds(newFeed);
        
        return startMovementFeed_RAF(MovementFeeds.Count-1,  startAsSolo );
    }
    public CCMovement startMovementFeed_RAF(int number,bool startAsSolo=false)
    {
        if (startAsSolo)
        {
            emptyActiveMovementFeeds();
        }

        Debug.Log("requested number : "+number);
        Debug.Log("MovementFeeds.count : "+MovementFeeds.Count );
        return addToActiveFeeds_RAF(MovementFeeds[number],number);

    }
    //-----------------------ADD TO FEEDS -----------------------------/*****
    
    public void addToFeeds(CCBezierBehaviourFeed newFeed)
    {
        BehaviorFeeds.Add(newFeed);
        
    }
    public void addToFeeds(CCMovement newFeed)
    {
        MovementFeeds.Add(newFeed);
        
    }
    //-----------------------ADD TO ACTIVE FEEDS -----------------------------/*****
    public void addToActiveFeeds(CCBezierBehaviourFeed theFeed,int managerID)
    {
        CCBezierBehaviourFeed newFeed = getDormantBehaviorFeed();
        theFeed.InsertItself(newFeed);
        newFeed.setMasters(MasterScript, MasterObject,this);
        newFeed.setManagerId(managerID);
        
        newFeed.FeedStart();
        
        ActiveBehaviorFeeds.Add(newFeed);
        checkActiveFeeds();
    }

    public void addToActiveFeeds(CCMovement theFeed,int managerID)
    {
        CCMovement newFeed = getDormantMovementFeed();
        theFeed.InsertItself(newFeed);
        newFeed.setMasters(MasterScript, MasterObject,this);
        newFeed.setManagerId(managerID);
        
        ActiveMovementFeeds.Add(newFeed);
        checkActiveFeeds();
    }
    
    // ---------ADD TO ACTIVE FEEDS RAF -----
    public CCBezierBehaviourFeed addToActiveFeeds_RAF(CCBezierBehaviourFeed theFeed,int managerID)
    {
        CCBezierBehaviourFeed newFeed = getDormantBehaviorFeed();
        theFeed.InsertItself(newFeed);
        newFeed.setMasters(MasterScript, MasterObject,this);
        newFeed.setManagerId(managerID);
        
        newFeed.FeedStart();
        
        ActiveBehaviorFeeds.Add(newFeed);
        checkActiveFeeds();
        return newFeed;
    }

    public CCMovement addToActiveFeeds_RAF(CCMovement theFeed,int managerID)
    {
        CCMovement newFeed = getDormantMovementFeed();
        theFeed.InsertItself(newFeed);
        newFeed.setMasters(MasterScript, MasterObject,this);
        newFeed.setManagerId(managerID);
        
        ActiveMovementFeeds.Add(newFeed);
        checkActiveFeeds();
        return newFeed;
    }
    //-----------------------DORMANT FEED MANAGEMENT--------------------------------/*****

    private CCBezierBehaviourFeed getDormantBehaviorFeed()
    {
        if (DormantBehaviorFeeds.Count<1)
        {
            CCBezierBehaviourFeed temp =gameObject.AddComponent<CCBezierBehaviourFeed>();
            return temp;
        }
        else
        {
            CCBezierBehaviourFeed temp = DormantBehaviorFeeds[0];
            List<CCBezierBehaviourFeed> tempList = new List<CCBezierBehaviourFeed>();
            for (int i = 0; i < DormantBehaviorFeeds.Count; i++)
            {
                tempList.Add(DormantBehaviorFeeds[i]);
            }

            DormantBehaviorFeeds = tempList;
            return temp;
        }
    }
    private CCMovement getDormantMovementFeed()
    {
        if (DormantMovementFeeds.Count<1)
        {
            CCMovement temp =gameObject.AddComponent<CCMovement>();
             
            return temp;
        }
        else
        {
            CCMovement temp = DormantMovementFeeds[0];
            List<CCMovement> tempList = new List<CCMovement>();
            for (int i = 0; i < DormantMovementFeeds.Count; i++)
            {
                tempList.Add(DormantMovementFeeds[i]);
            }

            DormantMovementFeeds = tempList;
            return temp;
        }
    }
    
    //-----------------------GET ACTIVE FEED ------------------------------*****

    public CCBezierBehaviourFeed getBehaviorFeed(int number, bool typeID_ELSE_listID = true)
    {
        foreach (var VARIABLE in ActiveBehaviorFeeds)
        {
            if (typeID_ELSE_listID)
            {
                if (VARIABLE.getTypeId() == number)
                {
                    
                    return VARIABLE;
                }
            }
            else
            {
                if (VARIABLE.getManagerId() == number)
                {
                     
                    return VARIABLE;
                }
            }
        }

        Debug.Log("ERROR NO FEED FOUND");
        return null;
    }
    public CCMovement getMovementFeed(int number,bool typeID_ELSE_listID=true)
    {
        foreach (var VARIABLE in ActiveMovementFeeds)
        {
            if (typeID_ELSE_listID)
            {
                if (VARIABLE.getTypeId() != number) continue;
                
                return VARIABLE;
            }
            else
            {
                if (VARIABLE.getManagerId() != number) continue;
                 
                return VARIABLE;
            }
        }
        Debug.Log("ERROR NO FEED FOUND");
        return null;
    }
    //-------------------END FEED -----------------------*****
    
    
    public void endBehaviorFeed(int number,bool typeID_ELSE_listID=true)
    {

        CCBezierBehaviourFeed temp = getBehaviorFeed(number, typeID_ELSE_listID);
        temp.feedEnd();
        removeFromActiveFeeds(temp);

    }
    public void endBehaviorFeed(CCBezierBehaviourFeed toEndFeed)
    {
        foreach (var VARIABLE in ActiveBehaviorFeeds)
        {
            if (toEndFeed==VARIABLE)
            {
                VARIABLE.feedEnd();
                removeFromActiveFeeds(VARIABLE);
            }
        }

    }
    
     public void endMovementFeed(int number,bool typeID_ELSE_listID=true)
    {
        removeFromActiveFeeds(getMovementFeed(number,typeID_ELSE_listID));

    }
    public void endMovementFeed(CCMovement toEndFeed)
    {
        removeFromActiveFeeds(toEndFeed);

    } 
    
 
    //------------------REMOVE FROM ACTIVE FEEDS------------------------------

    public void removeFromActiveFeeds(CCBezierBehaviourFeed itself)
    {
        List<CCBezierBehaviourFeed> temp=new List<CCBezierBehaviourFeed>();
        foreach (var VARIABLE in ActiveBehaviorFeeds)
        {
            if (VARIABLE!=itself)
            {
                temp.Add(VARIABLE);
            }
            else
            {
                DormantBehaviorFeeds.Add(VARIABLE);
                if (usePlayerManager)
                {
                    playerManager001.BehaviorEnded(VARIABLE.getTypeId());
                }
                
            }
        }

        ActiveBehaviorFeeds = temp;
        checkActiveFeeds();

    }
    public void removeFromActiveFeeds(CCMovement itself)// TODO maybe id will be needed ?!?
    {
       
        List<CCMovement> temp=new List<CCMovement>();
        foreach (var VARIABLE in ActiveMovementFeeds)
        {
            if (VARIABLE!=itself)
            {
                temp.Add(VARIABLE);
            }
            else
            {
                DormantMovementFeeds.Add(VARIABLE);
                if (usePlayerManager)
                {
                    playerManager001.BehaviorEnded(VARIABLE.getTypeId());
                }
            }
        }

        ActiveMovementFeeds = temp;
        checkActiveFeeds();
    } 
    
    //----------------------------------------------
    public void emptyActiveBehaviorFeeds()
    {
        foreach (var VARIABLE in ActiveBehaviorFeeds)
        {
            VARIABLE.feedEnd();
            DormantBehaviorFeeds.Add(VARIABLE);
        }

        ActiveBehaviorFeeds = new List<CCBezierBehaviourFeed>();
    }
    public void emptyActiveMovementFeeds()
    {
        foreach (var VARIABLE in ActiveMovementFeeds)
        {
            DormantMovementFeeds.Add(VARIABLE);
        }

        ActiveMovementFeeds = new List<CCMovement>();
    }
    
    //---------------------------------------------
   
    public void BypassMasterRequest()
    {
        BypassRequests++;
        MasterScript.BypassInHouseForces = true;
    }
    public void endBypassMasterRequest()
    {
        BypassRequests--;
        if (BypassRequests<1)
        {
            MasterScript.BypassInHouseForces = false;
        }
        
    }

   
    //--------------------------------------------------------
    //  TODO addMeTO dormant remove me from dormant 
}
/*
 _TheVelocity = ConstantMovementDirection;
        if (feedActive)
        {
            _TheVelocity += source.SourceFeed();
            if (countDown>0)
            {
                countDown -= Time.deltaTime;
            }
            else
            {
                countDown = 0;
            }
            
        }
 */