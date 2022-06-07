using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class ConstantRotatorManager : MonoBehaviour
{
    [SerializeField] private List<ConstantRotator> RotatorList;
    [SerializeField] private bool getListFromChildren;
    [SerializeField] private bool startAll;
    [SerializeField] private bool stopAll;
    [SerializeField] private bool switchDirection;
    [SerializeField] private bool switchDirectionRandom;
    [SerializeField] private bool switchSpeedRandom;
    [SerializeField] private float speedRandomFactor=2;
    [SerializeField] private bool speedChangeMultiplication_elseAddition=true;
    [SerializeField] private bool changeSpeedNow;
    [SerializeField] private float changeSpeedTo;
    
    [SerializeField] private bool Working;
    
    [SerializeField] private bool workWithRequests=true;
    public int requests;
    
     
    
    void Start()
    {
        if (getListFromChildren)
        {
            ConstantRotator[] tempList = GetComponentsInChildren<ConstantRotator>();
            foreach (var VARIABLE in tempList)
            {
                RotatorList.Add(VARIABLE);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (workWithRequests)
        {
            if (requests>0&&!Working)
            {
                StartAll();
            }
            if (requests<1&&Working)
            {
                StopAll();
            }
        }
        if (startAll)
        {
            startAll = false;
            StartAll();
        }

        if (stopAll)
        {
            stopAll = false;
            StopAll();
        }

        if (switchDirection)
        {
            switchDirection = false;
            SwitchDirection();
        }

        if (switchDirectionRandom)
        {
            switchDirectionRandom = false;
            SwitchDirectionRandom();
        }

        if (switchSpeedRandom)
        {
            switchSpeedRandom = false;
            SwitchSpeedRandom(speedRandomFactor, speedChangeMultiplication_elseAddition);
        }

        if (changeSpeedNow)
        {
            changeSpeedNow = false;
            ChangeSpeedNow(changeSpeedTo);
            

        }
    }

    public void ChangeSpeedNow(float speedChangeInto=0)
    {
        foreach (var VARIABLE in RotatorList)
        {
            Vector3 temp = new Vector3(speedChangeInto, speedChangeInto, speedChangeInto);
            VARIABLE.speeds = temp;

        }
        
    }
    public void StartAll()
    {
        Working = true;
        foreach (var VARIABLE in RotatorList)
        {
            VARIABLE.working = true;
        }
    }
    public void StopAll()
    {
        Working = false;
        foreach (var VARIABLE in RotatorList)
        {
            VARIABLE.working = false;
        }
    }
    public void SwitchDirectionRandom()
    {
        foreach (var VARIABLE in RotatorList)
        {
            Vector3 temp = new Vector3(1, 1, 1);
            int random = Random.Range(0, 1);
            temp.x = random;
            temp.x = temp.x==0? -1 : 1;
            random = Random.Range(0, 1);
            temp.y = random;
            temp.y = temp.y==0? -1 : 1;
            random = Random.Range(0, 1);
            temp.z = random;
            temp.z = temp.z==0? -1 : 1;
            VARIABLE.turningDirections = temp;
        }
    }
    public void SwitchDirection ()
    {
        foreach (var VARIABLE in RotatorList)
        {
            Vector3 temp = VARIABLE.turningDirections;
            temp *= -1;
            
            VARIABLE.turningDirections = temp;
        }
    }
    public void SwitchSpeedRandom(float RangeToIt=2,bool multiplication_elseAddition=true)
    {
        if (multiplication_elseAddition)
        {
            foreach (var VARIABLE in RotatorList)
            {
                Vector3 temp = VARIABLE.speeds;
                float random = Random.Range(temp.x/RangeToIt,temp.x*RangeToIt);
                temp.x = random;
             
                random = Random.Range(temp.y/RangeToIt,temp.y*RangeToIt);
                temp.y = random;
             
                random = Random.Range(temp.z/RangeToIt,temp.z*RangeToIt);
                temp.z = random;
             
                VARIABLE.speeds = temp;
            }
        }
        else
        {
            foreach (var VARIABLE in RotatorList)
            {
                Vector3 temp = VARIABLE.speeds;
                float random = Random.Range(temp.x-RangeToIt,temp.x+RangeToIt);
                temp.x = random;
             
                random = Random.Range(temp.y-RangeToIt,temp.y+RangeToIt);
                temp.y = random;
             
                random = Random.Range(temp.z-RangeToIt,temp.z+RangeToIt);
                temp.z = random;
             
                VARIABLE.speeds = temp;
            }
        }
      
    }
}
