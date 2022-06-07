using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour//TODO UPDATED INSTALL TO THE 000 PROJECT 
{
    [SerializeField] private float PassedTime=0;
    public bool test;
    public Transform Subject;
    [Header(" ")] 
    [SerializeField] private bool work=true;    
    [SerializeField] private bool BezierMoveActive;// OBSERVATION-CONTROL VALUE 
    
    
    private float RouteCompletionCounter;
    
    [Range(0.1f, 20)] 
    [SerializeField] private float EachRouteExecutionTime=10;
    [SerializeField] private float ExecutionSpeed=2.7f;
    [SerializeField] private bool UseSpeedForExecution;
    
    [Header(" ")] 
    [SerializeField] private bool localExecution;
    [SerializeField] private bool localRotationExecution;
    [SerializeField] private bool TakeRouteSnap;
    [SerializeField] private bool NewSnapEachTime;
    [Header("Execution Settings ")] 
    [SerializeField] private int ExecuteNoInRouteList=0 ;
    [SerializeField] private List<BezierRoute > Routes;
    
    
    [Header(" TO USE A SPECIAL SEQUENCE OF ROUTES ")] 
    [SerializeField] private bool useSpecialOrder;
    [SerializeField] private int  specialOrderNoWillExecute;

    private Vector3 lastPos;
    //----------------------------------------------------------------
    
    [System.Serializable]
    public struct RouteList {
        
        public List<BezierRoute > bezierRoutes;
    }
    [SerializeField] private List<RouteList> routesInSpecialOrder =new List<RouteList> () ;
    
    //----------------------------------------------------------------
    [SerializeField] private BezierRoute RouteInAction;
    
    
   
    
      
    void Start()
    {

        adjustments();
    }

    private void adjustments()
    {
        if (Subject == null)
        {
            Subject = gameObject.transform;
        }
        if ( Routes.Count<1)
        {
            work = false;
        }

        if (ExecuteNoInRouteList<0||ExecuteNoInRouteList>=Routes.Count)
        {
            ExecuteNoInRouteList = 0;
        }

        if (routesInSpecialOrder.Count<1)
        {
            useSpecialOrder = false;
        }

        EachRouteExecutionTime = Math.Abs(EachRouteExecutionTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (test)
        {
            test = false;
            if (useSpecialOrder)
            {
                startSpecialOrder();
            }
            else
            {
                startMove();
            }
            
        }
        if (BezierMoveActive)
        {
            
            makeMove();
            
        }
    }

    private void makeMove()
    {
        PassedTime += Time.deltaTime;

        List<BezierRoute> temp=Routes;
        if (useSpecialOrder)
        {
            temp = routesInSpecialOrder[specialOrderNoWillExecute].bezierRoutes;
        }
        if (!UseSpeedForExecution)
        {
            ExecutionSpeed = temp[ExecuteNoInRouteList].GetDistances() / EachRouteExecutionTime;
        }
        else
        {
            EachRouteExecutionTime = temp[ExecuteNoInRouteList].GetDistances() / ExecutionSpeed;
        }
        RouteCompletionCounter += Time.deltaTime * (1 / EachRouteExecutionTime);

       
        
        if (localExecution)
        {


            if (localRotationExecution)
            {
                Vector3 temp2=temp[ExecuteNoInRouteList].LocalPositionByState(RouteCompletionCounter,TakeRouteSnap);
                temp2 = getLocalDirection(temp2, Subject);
                Subject.localPosition = lastPos+temp2;
            }
            else
            {
                Subject.localPosition = lastPos+ temp[ExecuteNoInRouteList].LocalPositionByState(RouteCompletionCounter,TakeRouteSnap);
            }
            
        }
        else
        {
             
            
            Subject.position = temp[ExecuteNoInRouteList].PositionByState(RouteCompletionCounter,TakeRouteSnap);
        }

        if (RouteCompletionCounter>0.999 )
        {
            if (useSpecialOrder&&ExecuteNoInRouteList<(routesInSpecialOrder[specialOrderNoWillExecute].bezierRoutes.Count-1))// TODO its only checks special order ????
            {
                if (NewSnapEachTime)
                {
                    // 
                }
                ExecuteNoInRouteList++;
                RouteCompletionCounter = 0;
                bezierPathEnded();
                    
            }
            else
            {
                allBezierPathEnded();
            }
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

    public void allBezierPathEnded()
    {
        bezierPathEnded();
//        Debug.Log("ALL bezier Path Completed");
        BezierMoveActive = false;
    }
    public void bezierPathEnded()
    {
    //    Debug.Log("bezier Path Completed");
    }

    public void startMove(int targetNo)
    {
        BezierMoveActive = true;
        RouteCompletionCounter = 0;
        targetNo = Math.Abs(targetNo);
        if (targetNo>=Routes.Count)
        {
            targetNo = 0;
        }
        ExecuteNoInRouteList = targetNo;
        lastPos = Subject.localPosition;
    }
    public void startMove()
    {
        takeSnapOfAll(TakeRouteSnap);
        PassedTime = 0;
        startMove(ExecuteNoInRouteList);
    }

    private void takeSnapOfAll(bool do_OR_Not)
    {
        if (do_OR_Not)
        {
            foreach (var VARIABLE in Routes)
            {
                VARIABLE.savePos();
            }

            foreach (var VARIABLE in routesInSpecialOrder)
            {
                foreach (var each in VARIABLE.bezierRoutes)
                {
                    each.savePos();
                }
            }
        }
    }

    public void startSpecialOrder(int listNo)
    {
        BezierMoveActive = true;
        RouteCompletionCounter = 0;
        ExecuteNoInRouteList = 0;
        listNo = Math.Abs(listNo);
        if (listNo>=routesInSpecialOrder.Count)
        {
            listNo = 0;
        }
        specialOrderNoWillExecute = listNo;
    }
    public void startSpecialOrder( )
    {
        startSpecialOrder(specialOrderNoWillExecute);
    }
}
