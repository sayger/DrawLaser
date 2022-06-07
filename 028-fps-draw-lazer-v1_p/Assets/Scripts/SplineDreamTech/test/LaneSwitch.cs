using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;

public class LaneSwitch : MonoBehaviour
{
    public bool work = true;
    public bool jumpTheNextSpline  ;
    public bool useAlternativeRoute;
    public int primaryRoadAddress = 1;
    public int alternativeRoadAddress;

    [SerializeField] private SplineFollower follower;
    private SplineFollower _follower;
    
    
    void Start()
    {
        _follower = follower;
        if (follower==null)
        {
            _follower = GetComponent<SplineFollower>();
        }
        
        _follower.onNode += OnNodePassed;


    }

    private void OnNodePassed(List<SplineTracer.NodeConnection> passed)
    {
        Debug.Log(" PLAYER ENCOUNTER A NODE ");
        SplineTracer.NodeConnection nodeConnection = passed[0];

//        Debug.Log(nodeConnection.node.name + " at point "+ nodeConnection.point);

        double nodePercent = (double) nodeConnection.point / (_follower.spline.pointCount - 1);
        double followerPercent =_follower.UnclipPercent(_follower.result.percent);
        float distancePastNode =_follower.spline.CalculateLength(nodePercent, followerPercent);
        
        
//        Debug.Log("reached nodePercent in spline : "+(nodePercent*100));

        Node.Connection[] connections = nodeConnection.node.GetConnections();
        for (int i = 0; i < connections.Length; i++)
        {
//            Debug.Log(connections[i].spline.name + " at point " + connections[i].pointIndex);
        }

        
        int givenPrimaryRoadAddress=primaryRoadAddress;

        bool mustSwap = false;
        bool cancelThisNode=true;
        swapPoint nodeProperties = nodeConnection.node.GetComponent<swapPoint>();
        
        if (nodeProperties!=null)
        {
            if (!nodeProperties.swapActive)
            {
                Debug.Log(" swap wasn't activated ");
                
                return;
            }
            else
            {
                cancelThisNode = false;
            }
             
            mustSwap = nodeProperties.swapMustUsed;
            
            if (!nodeProperties.swapActive)
            {
                Debug.Log(" SWAP NODE IS NOT ACTIVE !!!");
                return;
            }
            else
            {
                nodeProperties.disableActivity();
            }

            if (nodeProperties.specialRoadSelected )
            {
                useAlternativeRoute = true;
                alternativeRoadAddress = nodeProperties.getSpecialRoadNo();
            }
            else if (nodeProperties.swapActive)
            {
                givenPrimaryRoadAddress = nodeProperties.getNextRoute();
            }

            
            
        }
         


        
        if ((jumpTheNextSpline||mustSwap)&&!cancelThisNode)  
        {
            int availableSplineCount = connections.Length;
            int defaultAddress = givenPrimaryRoadAddress;
            
            if (defaultAddress>=availableSplineCount) primaryRoadAddress = 1;
            if (defaultAddress>=availableSplineCount) defaultAddress = 1;
       
            int addressToExecute = useAlternativeRoute ? alternativeRoadAddress : defaultAddress ;

           // Debug.Log(" IT IS : ----------------------------------------------  :  "+availableSplineCount);
            if (availableSplineCount<2 || addressToExecute>=availableSplineCount  ) return;
          //  Debug.Log("congratulations!  problem ıts never reaches HERE HERE HERE HERE");
            //0.0f;
            double newNodePercent =  (double) connections[addressToExecute].pointIndex / (connections[addressToExecute].spline.pointCount-1) ;
            double newPercent = connections[addressToExecute].spline.Travel(newNodePercent, distancePastNode, _follower.direction);

            _follower.spline = connections[addressToExecute].spline;
        
            _follower.SetPercent(newPercent);

           /* if (mustSwap)
            {
                jumpTheNextSpline = false;
            }*/
            jumpTheNextSpline = false;

            Debug.Log(" NEW ROAD SELECTED SPLINE NO : "+addressToExecute);

        }
        
        
        
        useAlternativeRoute = false ;

    }

   
}
