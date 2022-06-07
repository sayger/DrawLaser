using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModeChanger : MonoBehaviour
{
    
    public bool testBool;
    
     
    public bool _Ready;
    public bool switchOneAfterOther;
    private bool _switchOneAfterOther;
    
    public bool usePositionChange=true;
    public bool positionChangeLocal=true;
    public bool useRotationChange ;
    public bool rotationChangeLocal=true;
     
    public bool positionChangeActive;
    public bool rotationChangeActive;
     
    public int selectedTarget = 0;
    public int secondTarget = 0;
    
    public float rotationChangeSpeed ;
    
    public bool useTimeCountForPosition;
    public float positionChangeTime = 2f;
    public float secondPositionChangeTime = 2f;
    public float positionChangeSpeed=10 ;
    
    public bool useLerpForPosition = true;
    public bool useMoveTowardsForPosition;


    public List<Transform> cameraPositions;
     



    void Start()
    {
        adjustments();
        
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_Ready)
        {
            if (testBool)
            {
                testBool = false;
                switchToGivenPosition(selectedTarget,true);
            }
            
            
            
            if (positionChangeActive)
            {
                if (positionChangeLocal)
                {
                    positionChangeActive =changePositionLocal(transform,cameraPositions[selectedTarget],positionChangeSpeed);
                }
                else
                {
                    positionChangeActive =changePositionGlobal(transform,cameraPositions[selectedTarget],positionChangeSpeed);
                }
                

              
            }

            if (rotationChangeActive)
            {
                if (rotationChangeLocal)
                {
                    
                }
                else
                {
                    rotationChangeActive= changeRotationGlobal(transform,cameraPositions[selectedTarget],rotationChangeSpeed);
                }
                
                 
                    
                 
            }

            if (_switchOneAfterOther)
            {
                if (!positionChangeActive&&!rotationChangeActive)
                {
                    int temp = selectedTarget;
                    selectedTarget = secondTarget;
                    if (selectedTarget==cameraPositions.Count)
                    {
                        _switchOneAfterOther = false;
                        selectedTarget=temp;
                    }
                    else
                    {
                        switchToGivenPosition(secondTarget,true,secondPositionChangeTime);
                    }
                    _switchOneAfterOther = false;
                
                
                }
            }
        }   
    }

    private bool changeRotationGlobal(Transform subject, Transform target, float speed)
    { 
        Vector3 targetRotation = target.transform.rotation.eulerAngles;
       
       Quaternion targetQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

       subject.rotation = Quaternion.Slerp(subject.rotation, targetQuaternion,  Time.deltaTime * speed*(1/18f));
       
       if (Vector3.Distance(subject.rotation.eulerAngles, targetRotation)<0.2)
       {
           
           return false;
       }

       return true;
       
    }
    private bool changeRotationLocal(Transform subject, Transform target, float speed)
    { 
        Vector3 targetLocalRotation = target.transform.localRotation.eulerAngles;
       
        Quaternion targetQuaternion = Quaternion.Euler(targetLocalRotation.x, targetLocalRotation.y, targetLocalRotation.z);

        subject.localRotation = Quaternion.Slerp(subject.rotation, targetQuaternion,  Time.deltaTime * speed*(1/18f));
       
        if (Vector3.Distance(subject.localRotation.eulerAngles, targetLocalRotation)<0.2)
        {
           
            return false;
        }

        return true;
       
    }
    private bool changePositionLocal(Transform subject, Transform target, float speed)
    {
        
        Vector3 targetLocalPosition = target.localPosition;
        Vector3 tempTargetPosition = new Vector3();
                
        if(useLerpForPosition) tempTargetPosition=Vector3.Lerp(subject.localPosition,targetLocalPosition,positionChangeSpeed*Time.deltaTime/(10));
                
        if (useMoveTowardsForPosition) tempTargetPosition=Vector3.MoveTowards(subject.localPosition,targetLocalPosition,speed*Time.deltaTime);

        subject.localPosition = tempTargetPosition;
        if (Vector3.Distance(subject.localPosition, targetLocalPosition)<0.2)
        {
           
            return false;
        }

        return true;
    }
    private bool changePositionGlobal(Transform subject, Transform target, float speed)
    {
        
        Vector3 targetPosition = target.position;
        Vector3 tempTargetPosition = new Vector3();
                
        if(useLerpForPosition) tempTargetPosition=Vector3.Lerp(subject.position,targetPosition,positionChangeSpeed*Time.deltaTime/(10));
                
        if (useMoveTowardsForPosition) tempTargetPosition=Vector3.MoveTowards(subject.position,targetPosition,speed*Time.deltaTime);

        subject.position = tempTargetPosition;
        if (Vector3.Distance(subject.position, targetPosition)<0.2)
        {
           
            return false;
        }

        return true;
    }

    private void adjustments()
    {
        if (cameraPositions.Count==0)
        {
            var transform1 = transform;
            GameObject temp = Instantiate(new GameObject("First Position"), transform1.position, transform1.rotation);
        
            cameraPositions.Add(temp.transform);
        }

        _switchOneAfterOther = false;




    }
    
    public void   setTargetAddress(int newTarget)
    {
        newTarget = Math.Abs(newTarget);
        if (newTarget>=cameraPositions.Count)
        {
            newTarget = cameraPositions.Count - 1;
            
        }

        selectedTarget = newTarget;
    }

    public void switchToGivenPosition(int newTargetAddress ,bool changeNow ,float changeTime )
    {
        switchToGivenPosition(newTargetAddress, changeNow, changeTime, switchOneAfterOther);
    }
    public void switchToGivenPosition(int newTargetAddress ,bool changeNow ,float changeTime,bool switchToSecond)
    {
        if (positionChangeActive&&!changeNow)
        {
            Debug.Log(" Problem ?");
            return;
        }
        positionChangeTime = changeTime;
        if (newTargetAddress>=cameraPositions.Count||newTargetAddress<0)
        {
            newTargetAddress = 0;
        }
        selectedTarget = newTargetAddress;
        positionChangeActive = usePositionChange;
        rotationChangeActive = useRotationChange;

        if (useTimeCountForPosition)
        {
            float distance = Vector3.Distance(transform.localPosition, cameraPositions[selectedTarget].transform.localPosition);
            positionChangeSpeed = distance / positionChangeTime;
        }

        _switchOneAfterOther = switchToSecond;

    }

    public void setSecondTarget(int secondTargetAddress ,float changeTime)
    {
        switchOneAfterOther = true;
        secondTarget = secondTargetAddress;
        secondPositionChangeTime = changeTime;
    }
    public void setSecondTarget(int secondTargetAddress  )
    {
        setSecondTarget(secondTargetAddress, secondPositionChangeTime);
    }
    
    public void switchToGivenPosition(int newTargetAddress,bool changeNow)
    {
        switchToGivenPosition(newTargetAddress,changeNow, positionChangeTime);



    }
    
    
}

