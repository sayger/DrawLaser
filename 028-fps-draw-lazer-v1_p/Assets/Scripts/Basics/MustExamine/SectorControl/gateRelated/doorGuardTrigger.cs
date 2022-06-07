using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorGuardTrigger : MonoBehaviour
{
 
    
    [SerializeField] private bool work=true;
    [SerializeField] private bool triggerInvisible=true;
    
    [SerializeField] private bool itsOpenTrigger=true;
    [SerializeField] private bool itsCloseTrigger=true;

    [SerializeField] private GameObject effectOnlyOneDoor;
    [SerializeField] private int doorID=1;
    
     private bool thisIsOneDoorTrigger=false;
     private doorGuard onlyDoorsGuard;
     
     [SerializeField] private bool countDownTriggers=false;
     [SerializeField] private int  remainingActivations=5;
    
    [SerializeField] private List<string> activationTags=new List<string>();
    [SerializeField] private List<GameObject>  activationObjects=new List<GameObject>();

   // [SerializeField] private bool itsTriggered=false;
    
    
    
    void Start()
    {
        makeAdjustments();
    }

    private void makeAdjustments()
    {
        if (effectOnlyOneDoor!=null)
        {
            thisIsOneDoorTrigger = true;
            onlyDoorsGuard = effectOnlyOneDoor.GetComponent<doorGuard>();
        }

        checkValidityOfVariables();
        if (triggerInvisible)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }

    }

    private void checkValidityOfVariables()
    {

        if( activationTags.Count==0&&activationObjects.Count==0)
        {
            work = false;
        }

        if (!(itsOpenTrigger||itsCloseTrigger))
        {
            work = false;
        }


    }


    public void addParameter(GameObject newbie)
    {
        activationObjects.Add(newbie);

        checkValidityOfVariables();
    }
    public void addParameter(string newbie)
    {
         activationTags.Add(newbie);

        checkValidityOfVariables();

    }
    public void addParameter(List<GameObject> newbies)
    {

        foreach (var VARIABLE in newbies)
        {
            addParameter(VARIABLE);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!work) return;
        GameObject otherObj = other.gameObject;
        if (checkTags(otherObj)||checkGameObjects(otherObj))
        {
            detonate();
             
          //  itsTriggered = true;
         //   Invoke(nameof(makeTriggeredFalse),2f);
        }


    }

    private void makeTriggeredFalse()
    {

      //  itsTriggered = false;


    }

    private void  detonate()
    {
        if (remainingActivations == 0)
        {
            work = false;
        }
        if (thisIsOneDoorTrigger)
            detonateDedicatedDoor();
        else
        {
            detonateGateManager();
        }

       
    }

    private void detonateGateManager()
    {

        var key = 0;
        if (itsOpenTrigger&&itsCloseTrigger)
        {
            key = 1;
        }
        else if (itsOpenTrigger)
        {
            key = 2;
        }
        else if (itsCloseTrigger)
        {
            key = 3;
        }

        switch (key)
        {
            case 0:
                work = false;
                return;
            case 1:
                gateManager.Instance.activateTheSectorDoor(doorID);
                remainingActivations--;
                break;
            case 2:
                gateManager.Instance.openTheSectorDoor(doorID);
                remainingActivations--;
                break;
            case 3:
                gateManager.Instance.closeTheSectorDoor(doorID);
                remainingActivations--;
                break;
                
        }


    }

    private void detonateDedicatedDoor()
    {

        var key = 0;
        if (itsOpenTrigger&&itsCloseTrigger)
        {
            key = 1;
        }
        else if (itsOpenTrigger)
        {
            key = 2;
        }
        else if (itsCloseTrigger)
        {
            key = 3;
        }

        switch (key)
        {
            case 0:
                work = false;
                return;
            case 1:
                onlyDoorsGuard.activateIt();  
                remainingActivations--;
                break;
            case 2:
                onlyDoorsGuard.openIt();  
                remainingActivations--;
                break;
            case 3:
                onlyDoorsGuard.closeIt(); 
                remainingActivations--;
                break;
                
        }


    }

    private bool checkTags(GameObject subject)
    {
        foreach (var expr in activationTags)
        {
            if (subject.CompareTag(expr))
            {
                return true;
            }
        }
        return false;
    }

    

    private bool checkGameObjects(GameObject subject)
    {
        foreach (var expr in activationObjects)
        {
            if (subject==expr)
            {
                return true;
            }
        }
        return false;



    }
}
