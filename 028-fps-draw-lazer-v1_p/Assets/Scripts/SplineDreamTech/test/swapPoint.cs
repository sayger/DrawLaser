using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapPoint : MonoBehaviour
{
    public bool swapActive = true;
    public bool swapMustUsed = true;
    private bool _initialSwapMust;
    public bool reusablePoint = true;
    public float reactivationTime = 2f;
   // public List<TargetTrigger> relatedTriggers;
    
    public List<int> primaryNextSplineRouteList;
    public int nextOnList = 0;

    public bool specialRoadSelected;
    public int specialRoadNo=-1; // if this  overwritten by hand other trigger like components wont effect this
    void Start()
    {
        _initialSwapMust = swapMustUsed;
        if(primaryNextSplineRouteList.Count<1) primaryNextSplineRouteList.Add(1);
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableActivity()
    {
        swapActive = false;
        swapMustUsed = false;
        if (reusablePoint) Invoke("reActivate", reactivationTime);
    }

    private void reActivate()
    {
        swapActive = true;
        swapMustUsed = _initialSwapMust;
        /*
        foreach (var VARIABLE in relatedTriggers)
        {
            VARIABLE.work = true;
        }*/
    }

    public int getNextRoute()
    {
        if (nextOnList > primaryNextSplineRouteList.Count - 1) nextOnList = 0;

        Debug.Log(" MY GAME OBJECT IS  : "+this.gameObject);
        int result = primaryNextSplineRouteList[nextOnList];

        nextOnList++;
        return result;
        

    }

    public int getSpecialRoadNo()
    {
        Debug.Log(" some idiot ask for road no whoo");
        if (specialRoadNo == -1) specialRoadNo = 1;
        return specialRoadNo;
    }

    public void setSpecialRoadNo(int newSpecialRoadNo)//fail safe for too high numbers is in lane switch code sadly
    {
        if (specialRoadNo == -1) specialRoadNo = newSpecialRoadNo;
        Debug.Log(" special road no wanted to be assign as : "+newSpecialRoadNo);
        Debug.Log(" special road no is now : "+specialRoadNo);
         

    }
    
     
}
