using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCBezierBehaviourFeed : MonoBehaviour
{
    public Vector3 FeedOutput;
    private CCBezierBehaviourFeed SuccessorBezierFeed; //TODO LATER MAYBE
    private CCMovement SuccessorMovementFeed;
    private bool useSuccessor;
    private float referencePo;
    [SerializeField] private int TypeId; 
    private int ManagerId;//  
    [Header("  ")] 
    
   public CCFeedControl ManagerScript;
   public CCMovement MasterScript;
   public GameObject SubjectObject;
   [SerializeField] private bool BypassMaster;
   [SerializeField] private bool ReleaseMasterAfterWards;
   
   [SerializeField] private bool SubjectsRotationEffective;
   [SerializeField] private bool locationSnapMode;
   
   [SerializeField] private bool SurfaceSensitive;
   [SerializeField] private bool GroundSensitive;
   
   [Range(0, 1)] [SerializeField]
   private float IndicatorSphereSize=0.25f;
   [Range(1, 20)] [SerializeField]
   private float IndicatorSphereFrequency=1f;
  
    //----------------------------------------------------------------
    [SerializeField] private float RemainingTime;
    private float TimeCounter;
    [SerializeField] private bool ControlPointsVisual=true; // SET VALUE
    private bool _VisualOn; // STATE CONTROL VALUE
    
    [SerializeField] private bool Work=true; // SET-CONTROL VALUE
    [SerializeField] private bool FeedIsActive;
    private bool _EditorMode=true;// STATE CONTROL VALUE
    [SerializeField] private float AmplifyResultEffectBy = 1;
    

    
    //----------------------------------------------------------------
     
    [TextArea]
    [Tooltip("SCRIPT PURPOSE : ")]
    [SerializeField]
    private string description;
    
    //------------------------------------------------------------
    [Tooltip(" SET VALUE ")][Range(0, 50)]
    [SerializeField] private float ExecutionTime=1.5f;// SET VALUE
    public float RouteProgressCounter=0;// OBSERVATION VALUE
    [SerializeField] private float AverageSpeed;// OBSERVATION VALUE 
    [SerializeField] private float CurrentSpeed;// OBSERVATION VALUE 
    [Header("  ")] 
   
    [SerializeField] private double TotalDistance; // OBSERVATION VALUE
    [SerializeField] private double XDistance; // OBSERVATION VALUE
    [SerializeField] private double YDistance; // OBSERVATION VALUE
    [SerializeField] private double ZDistance; // OBSERVATION VALUE
    
    //--------------------------------------------------------------
    [Header(" REFERENCE EQUATION VARIABLES  ")] 
    [SerializeField] private float StartSpeed;// OBSERVATION VALUE
    [SerializeField] private float EndSpeed; // OBSERVATION VALUE
    [SerializeField] private bool UseReferenceEquation=false;// SET-CONTROL VALUE
    [SerializeField] private BezierEase bezierEase;

    
   // [Header(" F(x) = A*x^Q + B*x^W + C  ")] 
    private float Q_ReferenceEquationParabolaFactor=3;// SET VALUE
    private float W_ReferenceEquationParabolaFactor=2;// SET VALUE                                   
    
    private float A_ReferenceEquationParameter=1;// SET VALUE 
    private float B_ReferenceEquationParameter=-2;// SET VALUE 
    private float C_ReferenceEquationParameter=6;// SET VALUE 
    
    private float Reference_x_StartPoint=0;// SET VALUE 
    private float Reference_x_EndPoint=2.5f;// SET VALUE 
    private float CurrentSpeedFactor;// OBSERVATION VALUE  //TODO can made private
    private float StartSpeedFactor;// OBSERVATION VALUE   //TODO can made private
    private float EndSpeedFactor;// OBSERVATION VALUE   //TODO can made private
    private float EquationResizeFactor;// OBSERVATION VALUE   //TODO can made private
    
    //---------------------------------------------------------------------------
     
    
    [Header(" ROUTE   ADJUSTING   ELEMENTS  ")] 
    [SerializeField] private Transform[] controlPoints = new Transform[4]; // SET VALUE
    private Vector3 gizmosPosition; // in-process value
    
    //------------------------------------------------------
    private float targetIntegral;
    private float equationIntegral;

    private Vector3 snapX; 
    private Vector3 snapY;
    private Vector3 snapZ;
    
    
    void Start()
    {
        Adjustments();
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
    void FixedUpdate()
    {
       /* if (testUseless)
        {
            testUseless = false;
            Debug.Log(" start original : "+EquationResult( Reference_x_StartPoint) );
            Debug.Log(" end original : "+EquationResult( Reference_x_EndPoint) );
            Debug.Log(" original integral: "+equationIntegral );
            Debug.Log(" target integral: "+ targetIntegral );
            Debug.Log(" resized integral : "+EquationResizeFactor*equationIntegral );
        }*/
    }


    public void FeedStart()
    {
        FeedIsActive = true;
        RemainingTime = ExecutionTime;
        RouteProgressCounter = 0;
        TimeCounter = 0;
        GetDistances();
        if (BypassMaster)
        {
            ManagerScript.BypassMasterRequest();
        }

        if (locationSnapMode)
        {
            takeSnap(SubjectObject.transform);
        }
        
    }  
    
    private float CalculateSpeedFactor()
    {
        
        if (!UseReferenceEquation)
        {
            CurrentSpeedFactor = 1;
            return CurrentSpeedFactor;
        }
        

        float referencePoint = (Reference_x_StartPoint)+ (TimeCounter * ((Reference_x_EndPoint - Reference_x_StartPoint) / ExecutionTime));
        
        referencePo = referencePoint;

        float realResult = EquationResult(referencePoint); 

        CurrentSpeed = realResult * EquationResizeFactor*(/*TimeCounter **/ ((Reference_x_EndPoint - Reference_x_StartPoint) / ExecutionTime));
        CurrentSpeedFactor = CurrentSpeed / AverageSpeed;
        return CurrentSpeedFactor;

    }

    private void stopFeedCheck()
    {
        ManagerScript.removeFromActiveFeeds(this);
    }

    private void checkCollision()
    {
        if (SurfaceSensitive && MasterScript.onSurface)
        { FeedIsActive = false; return;}
        if (GroundSensitive && MasterScript.onGround) 
        { FeedIsActive = false; return;}
        
    }

                                                                                     
    public void feedEnd() 
    {
        FeedIsActive = false;
        RemainingTime = 0;
        
        if (ReleaseMasterAfterWards)
        {
            ManagerScript.endBypassMasterRequest();
        }

        if (!useSuccessor)
        {
            stopFeedCheck();
        }
      /*  else if (SuccessorMovementFeed!=null) // TODO MAYBE ADD SUCCESSOR LATER HUH ?
        {
            
            CCMovement temp = ManagerScript.startMovementFeed_RAF(SuccessorMovementFeed);
            temp.makeTransition(FeedOutput);// TODO why not total velocity
            stopFeedCheck();
        }
        else
        {
            BecomeSuccessor(SuccessorBezierFeed); //TODO this should be solved more cautiously 
           
            FeedStart();
        }*/
        
        
    }
    public Vector3 SourceFeed()
    {
        if (SurfaceSensitive||GroundSensitive)
        {
            checkCollision();
        }
        if (!FeedIsActive)
        {
          //  stopFeedCheck();
            return new Vector3(0, 0, 0);
            
        }

        TimeCounter += Time.deltaTime;
        RemainingTime = ExecutionTime - TimeCounter;
        
        
        float speedFactor = CalculateSpeedFactor();
 
        float divider = (1 / ExecutionTime)*speedFactor;
        float addition =  Time.deltaTime*divider  ;
        RouteProgressCounter += addition;
        if (RouteProgressCounter>0.9999)
        {
            feedEnd();
            FeedOutput=new Vector3(0, 0, 0);
            return FeedOutput;
        }
        
        
        float nowTime = RouteProgressCounter;
        if (nowTime > 1 - addition) nowTime = 1 - addition;
        Vector3 current= Mathf.Pow(1 - nowTime, 3) * controlPoints[0].position +
                         3 * Mathf.Pow(1 - nowTime, 2) * nowTime * controlPoints[1].position +
                         3 * (1 - nowTime) * Mathf.Pow(nowTime, 2) * controlPoints[2].position +
                         Mathf.Pow(nowTime, 3) * controlPoints[3].position;
        
        
        
        
        //--------------------------------------------------------------------------
        
        float nextTime = nowTime + addition;
        if (nextTime > 1) nextTime = 1;
         
        Vector3 next= Mathf.Pow(1 - nextTime, 3) * controlPoints[0].position +
                         3 * Mathf.Pow(1 - nextTime, 2) * nextTime * controlPoints[1].position +
                         3 * (1 - nextTime) * Mathf.Pow(nextTime, 2) * controlPoints[2].position +
                         Mathf.Pow(nextTime, 3) * controlPoints[3].position;

        
        //----------------------------------------------
        float XMiniDistance = (float)  ((Math.Round(next.x*1000) - Math.Round(current.x*1000)) / 1000) ;
        float YMiniDistance = (float)  ((Math.Round(next.y*1000) - Math.Round(current.y*1000)) / 1000) ;
        float ZMiniDistance = (float)  ((Math.Round(next.z*1000) - Math.Round(current.z*1000)) / 1000) ;
        
        Vector3 direction = new Vector3(XMiniDistance, YMiniDistance, ZMiniDistance );

        direction *= (1/Time.deltaTime )*AmplifyResultEffectBy;
        if (locationSnapMode)
        {
            direction = getFrozenLocalDirection(direction);
        }
        else if (SubjectsRotationEffective)
        {
            direction = getLocalDirection(direction, SubjectObject.transform);
        }

        FeedOutput = direction;
        return direction;

    }
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }

    private Vector3 getFrozenLocalDirection(Vector3 planedDirection, float power = 1)
    {
        Vector3 localYDirection = snapY*planedDirection.y;
        Vector3 localXDirection = snapX*planedDirection.x;
        Vector3 localZDirection = snapZ*planedDirection.z;

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }

    private void takeSnap(Transform subject)
    {
        snapY = subject.TransformDirection(Vector3.up);
        snapX = subject.TransformDirection(Vector3.right);
        snapZ = subject.TransformDirection(Vector3.forward);
    }
    public void GetDistances()
    {
        TotalDistance  = 0; 
        XDistance  = 0; 
        YDistance = 0;
        ZDistance = 0;
        
        Vector3 last=new Vector3();
        
        for (float t = 0; t <=1 ; t+=0.001f)
        {
            
            Vector3 current= Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                             3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                             3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                             Mathf.Pow(t, 3) * controlPoints[3].position;
            if (t == 0) last = current;

            double xVariant = Math.Abs(Math.Round(current.x*1000) - Math.Round(last.x*1000))/1000;
            double yVariant = Math.Abs(Math.Round(current.y*1000) - Math.Round(last.y*1000))/1000;
            double zVariant = Math.Abs(Math.Round(current.z*1000) - Math.Round(last.z*1000))/1000;
            
            XDistance += xVariant;
            YDistance += yVariant;
            ZDistance += zVariant;
            
            TotalDistance += Math.Sqrt(Math.Pow(xVariant,2) + Math.Pow(yVariant,2) + Math.Pow(zVariant,2) );
            
            // TotalDistance += Vector3.Distance(last, current);

            last = current;
        }

        AverageSpeed = (float) TotalDistance / ExecutionTime;
       
         
       FindEquationVariables();

    }
    private void FindEquationVariables( )
    {
        
        if (UseReferenceEquation)
        {

            SetEquationVariables();


            float ReferenceEquationIntegralStart = (float)( ((1/(Q_ReferenceEquationParabolaFactor+1))*A_ReferenceEquationParameter * (Math.Pow(Reference_x_StartPoint, Q_ReferenceEquationParabolaFactor)))
                                                            + ((1/(W_ReferenceEquationParabolaFactor+1))*B_ReferenceEquationParameter * (Math.Pow(Reference_x_StartPoint, W_ReferenceEquationParabolaFactor))) 
                                                            + (C_ReferenceEquationParameter*Reference_x_StartPoint));
            float ReferenceEquationIntegralEnd = (float)( ((1/(Q_ReferenceEquationParabolaFactor+1))*A_ReferenceEquationParameter * (Math.Pow(Reference_x_EndPoint, Q_ReferenceEquationParabolaFactor)))
                                                          + ((1/(W_ReferenceEquationParabolaFactor+1))*B_ReferenceEquationParameter * (Math.Pow(Reference_x_EndPoint, W_ReferenceEquationParabolaFactor))) 
                                                          + (C_ReferenceEquationParameter*Reference_x_EndPoint));

            float ReferenceEquationIntegral = ReferenceEquationIntegralEnd - ReferenceEquationIntegralStart;
            equationIntegral = ReferenceEquationIntegral;
        
            targetIntegral = (AverageSpeed * ExecutionTime);// delete maybe
        
            EquationResizeFactor = (AverageSpeed * ExecutionTime) / ReferenceEquationIntegral;
            StartSpeed = EquationResult( Reference_x_StartPoint)  * EquationResizeFactor *(/*TimeCounter **/ ((Reference_x_EndPoint - Reference_x_StartPoint) / ExecutionTime));;
            EndSpeed = EquationResult( Reference_x_EndPoint)   * EquationResizeFactor *(/*TimeCounter **/ ((Reference_x_EndPoint - Reference_x_StartPoint) / ExecutionTime));;

        }
        else
        {
            StartSpeed = AverageSpeed;
            EndSpeed = AverageSpeed;
            
        }
        StartSpeedFactor = StartSpeed / AverageSpeed;
        EndSpeedFactor = EndSpeed / AverageSpeed;
        
        
       
    }

    private float EquationResult(float x_Variable)
    {
        return  (float)
            ( (A_ReferenceEquationParameter * (Math.Pow(x_Variable, Q_ReferenceEquationParabolaFactor)))
              + (B_ReferenceEquationParameter * (Math.Pow(x_Variable, W_ReferenceEquationParabolaFactor))) 
              + C_ReferenceEquationParameter);

    }


    private void SetEquationVariables()
    {
        List<float> presetValues = bezierEase.GetEquationSettings();

     Q_ReferenceEquationParabolaFactor=presetValues[0];
     W_ReferenceEquationParabolaFactor=presetValues[1];                                   
    
     A_ReferenceEquationParameter=presetValues[2];
     B_ReferenceEquationParameter=presetValues[3];
     C_ReferenceEquationParameter=presetValues[4];
    
     Reference_x_StartPoint=presetValues[5];
     Reference_x_EndPoint=presetValues[6];
     
    }

    //----------------------------------------------------------------editor visual settings methods
    private void OnDrawGizmos()
    {
        if (ControlPointsVisual)
        {
            float Frequency = (1 / IndicatorSphereFrequency) * 0.05f*(20*(float)(1f/TotalDistance));
            for (float t = 0; t <=1 ; t+=Frequency)
            {
                gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                                 3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                                 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                                 Mathf.Pow(t, 3) * controlPoints[3].position;
                Gizmos.DrawSphere(gizmosPosition,IndicatorSphereSize);
            }
            Gizmos.DrawLine(new Vector3(controlPoints[0].position.x,controlPoints[0].position.y,controlPoints[0].position.z),
                new Vector3(controlPoints[1].position.x,controlPoints[1].position.y,controlPoints[1].position.z));
            Gizmos.DrawLine(new Vector3(controlPoints[2].position.x,controlPoints[2].position.y,controlPoints[2].position.z),
                new Vector3(controlPoints[3].position.x,controlPoints[3].position.y,controlPoints[3].position.z));
        }

        if (_EditorMode)
        {
            GetDistances();
        }

        checkVisualActivity();


    }

    private void checkVisualActivity()
    {
        if (ControlPointsVisual&&!_VisualOn)
        {
            _VisualOn = true;
            foreach (var VARIABLE in controlPoints)
            {
                VARIABLE.gameObject.GetComponent<MeshRenderer>().enabled=true;
            }
        }
        if (!ControlPointsVisual&&_VisualOn)
        {
            _VisualOn = false;
            foreach (var VARIABLE in controlPoints)
            {
                VARIABLE.gameObject.GetComponent<MeshRenderer>().enabled=false;
            }
        }
    }
     
    //------------------------------------------------------------- adjustment methods
    
    private void Adjustments()
    {
        _EditorMode = false;
        CheckEssentials();
        GetDistances();
    }
    private void CheckEssentials()
    {
        RemainingTime = ExecutionTime;
        CheckControlPoints();
         
        AmplifyResultEffectBy = AmplifyResultEffectBy == 0 ? 1 : AmplifyResultEffectBy;
        
        if (bezierEase==null)
        {
            UseReferenceEquation = false;
        }

        if (SuccessorBezierFeed!=null||SuccessorMovementFeed!=null)
        {
            useSuccessor = true;
        }

    }
    private void CheckControlPoints()
    {
        if (!IsFull(controlPoints))
        {
            for (int i = 0; i < controlPoints.Length; i++)
            {
                controlPoints[i] = this.gameObject.transform.GetChild(i);
            }
            if (!IsFull(controlPoints)) Work = false;
            
        }
    }

    private bool IsFull(Transform[] input)
    {
        foreach (var VARIABLE in input)
        {
            if (VARIABLE==null)
            {
                return false;
            }
             
        }

        return true;
    }
    public void setMasters(CCMovement supreme, GameObject masterG,CCFeedControl manager)
    {
        SubjectObject = masterG;
        MasterScript = supreme;
        ManagerScript = manager;

    }
    //---------------------------------------SELF DUPLICATION -----------------------

    public void InsertItself(CCBezierBehaviourFeed dormantScript)
    {
        dormantScript.referencePo=referencePo;
        dormantScript.TypeId=TypeId;
        dormantScript.ManagerScript=ManagerScript;
        dormantScript.MasterScript=MasterScript;
        dormantScript.SubjectObject=SubjectObject;
        dormantScript.BypassMaster=BypassMaster;
        dormantScript.SubjectsRotationEffective=SubjectsRotationEffective;
        dormantScript.locationSnapMode=locationSnapMode;
        dormantScript.SurfaceSensitive=SurfaceSensitive;
        dormantScript.GroundSensitive=GroundSensitive;
        dormantScript.IndicatorSphereSize =IndicatorSphereSize;
        dormantScript.IndicatorSphereFrequency =IndicatorSphereFrequency ;
        dormantScript.RemainingTime=RemainingTime;
        dormantScript.TimeCounter=TimeCounter;
        dormantScript.ControlPointsVisual  =ControlPointsVisual;  
        dormantScript._VisualOn=_VisualOn;
        dormantScript.Work =Work ; 
        dormantScript.FeedIsActive=FeedIsActive;
        dormantScript._EditorMode =_EditorMode; 
        dormantScript.AmplifyResultEffectBy =AmplifyResultEffectBy ;
        dormantScript.description=description;
        dormantScript.ExecutionTime=ExecutionTime ; 
        dormantScript.RouteProgressCounter =RouteProgressCounter; 
        dormantScript.AverageSpeed=AverageSpeed;  
        dormantScript.CurrentSpeed=CurrentSpeed;
        dormantScript.TotalDistance=TotalDistance;  
        dormantScript.XDistance=XDistance;  
        dormantScript.YDistance=YDistance; 
        dormantScript.ZDistance=ZDistance;
        dormantScript.StartSpeed=StartSpeed; 
        dormantScript.EndSpeed=EndSpeed; 
        dormantScript.UseReferenceEquation =UseReferenceEquation; 
        dormantScript.bezierEase=bezierEase;
        dormantScript.Q_ReferenceEquationParabolaFactor =Q_ReferenceEquationParabolaFactor; 
        dormantScript.W_ReferenceEquationParabolaFactor  =W_ReferenceEquationParabolaFactor;
        dormantScript.A_ReferenceEquationParameter=A_ReferenceEquationParameter;  
        dormantScript.B_ReferenceEquationParameter=B_ReferenceEquationParameter;  
        dormantScript.C_ReferenceEquationParameter=C_ReferenceEquationParameter;
        dormantScript.Reference_x_StartPoint=Reference_x_StartPoint;  
        dormantScript.Reference_x_EndPoint=Reference_x_EndPoint;  
        dormantScript.CurrentSpeedFactor=CurrentSpeedFactor; 
        dormantScript.StartSpeedFactor=StartSpeedFactor; 
        dormantScript.EndSpeedFactor=EndSpeedFactor; 
        dormantScript.EquationResizeFactor=EquationResizeFactor; 
        dormantScript.controlPoints=controlPoints; 
        dormantScript.gizmosPosition=gizmosPosition;
        dormantScript.targetIntegral=targetIntegral;
        dormantScript.equationIntegral=equationIntegral;
        dormantScript.snapX=snapX; 
        dormantScript.snapY=snapY;
        dormantScript.snapZ=snapZ;
        dormantScript.SuccessorBezierFeed = SuccessorBezierFeed;
        dormantScript.SuccessorMovementFeed = SuccessorMovementFeed;
    }

    private void BecomeSuccessor(CCBezierBehaviourFeed newFeed )
    {
        
        referencePo=newFeed.referencePo;
        TypeId=newFeed.TypeId;
        
        
        
        BypassMaster=newFeed.BypassMaster;
        SubjectsRotationEffective=newFeed.SubjectsRotationEffective;
        locationSnapMode=newFeed.locationSnapMode;
        SurfaceSensitive=newFeed.SurfaceSensitive;
        GroundSensitive=newFeed.GroundSensitive;
        IndicatorSphereSize =newFeed.IndicatorSphereSize;
        IndicatorSphereFrequency =newFeed.IndicatorSphereFrequency ;
        RemainingTime=newFeed.RemainingTime;
        TimeCounter=newFeed.TimeCounter;
        ControlPointsVisual  =newFeed.ControlPointsVisual;  
        _VisualOn=newFeed._VisualOn;
        Work =newFeed.Work ; 
        FeedIsActive=newFeed.FeedIsActive;
        _EditorMode =newFeed._EditorMode; 
        AmplifyResultEffectBy =newFeed.AmplifyResultEffectBy ;
        description=newFeed.description;
        ExecutionTime=newFeed.ExecutionTime ; 
        RouteProgressCounter =newFeed.RouteProgressCounter; 
        AverageSpeed=newFeed.AverageSpeed;  
        CurrentSpeed=newFeed.CurrentSpeed;
        TotalDistance=newFeed.TotalDistance;  
        XDistance=newFeed.XDistance;  
        YDistance=newFeed.YDistance; 
        ZDistance=newFeed.ZDistance;
        StartSpeed=newFeed.StartSpeed; 
        EndSpeed=newFeed.EndSpeed; 
        UseReferenceEquation =newFeed.UseReferenceEquation; 
        bezierEase=newFeed.bezierEase;
        Q_ReferenceEquationParabolaFactor =newFeed.Q_ReferenceEquationParabolaFactor; 
        W_ReferenceEquationParabolaFactor  =newFeed.W_ReferenceEquationParabolaFactor;
        A_ReferenceEquationParameter=newFeed.A_ReferenceEquationParameter;  
        B_ReferenceEquationParameter=newFeed.B_ReferenceEquationParameter;  
        C_ReferenceEquationParameter=newFeed.C_ReferenceEquationParameter;
        Reference_x_StartPoint=newFeed.Reference_x_StartPoint;  
        Reference_x_EndPoint=newFeed.Reference_x_EndPoint;  
        CurrentSpeedFactor=newFeed.CurrentSpeedFactor; 
        StartSpeedFactor=newFeed.StartSpeedFactor; 
        EndSpeedFactor=newFeed.EndSpeedFactor; 
        EquationResizeFactor=newFeed.EquationResizeFactor; 
        controlPoints=newFeed.controlPoints; 
        gizmosPosition=newFeed.gizmosPosition;
        targetIntegral=newFeed.targetIntegral;
        equationIntegral=newFeed.equationIntegral;
        snapX=newFeed.snapX; 
        snapY=newFeed.snapY;
        snapZ=newFeed.snapZ;
        SuccessorBezierFeed = newFeed.SuccessorBezierFeed;
        
    }
}
