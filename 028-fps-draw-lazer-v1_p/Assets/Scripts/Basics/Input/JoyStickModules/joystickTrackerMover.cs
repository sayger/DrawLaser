using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickTrackerMover : MonoBehaviour
{
    [SerializeField] private bool testReset;
    [SerializeField] private float testValueH;
    [SerializeField] private float testValueV;
    [SerializeField] private float testValue3;
    
     
    
    
    public bool work=true;

    public GameObject target;
    [SerializeField] private bool changeInAction;
    [SerializeField] private char horizontalEffectOnAxis = 'x';
    [SerializeField] private char verticalEffectOnAxis = 'z';
     
    [SerializeField] private float ChangeFactor=1;
    [SerializeField] private bool useGradualSpeed=false;
     
     
    
    [SerializeField] private bool useMinMax;
    [SerializeField] private float HmaxOffset;
    [SerializeField] private float HminOffset;
    [SerializeField] private float VmaxOffset;
    [SerializeField] private float VminOffset;
    
    [SerializeField] private float joyStickDiscardMovementPercent = 0;
    private float _joyStickDiscardPercent=15;

    [SerializeField] private bool reverseHorizontalInput;
    [SerializeField] private bool reverseVerticalInput;



    [SerializeField] private bool joystick1Else2;
    
    [SerializeField] private FloatingJoystick joystick1;
    [SerializeField] private DynamicJoystick  joystick2;
    
     
    
    public float joyStickHorizontal;
    public float joyStickVertical;
    [SerializeField] private Vector3 firstOffsets;
    private bool happened;

    public Vector3 targetOffsets = new Vector3();
    private float firsty;
    
    
    void Start()
    {
        firsty = transform.position.y;
        targetOffsets = new Vector3(transform.position.x - target.transform.position.x, transform.position.y - target.transform.position.y, transform.position.z - target.transform.position.z);
    }
    private void variableUpdate()
    {
        int reverser = reverseHorizontalInput ? -1 : 1;
        joyStickHorizontal = joystick1Else2 ? joystick1.Horizontal : joystick2.Horizontal ;
        joyStickHorizontal *= reverser;
        reverser = reverseVerticalInput ? -1 : 1;
        joyStickVertical = joystick1Else2 ? joystick1.Vertical : joystick2.Vertical ;
        joyStickVertical *= reverser;




    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!happened)
        {
            happened = true;
            
            firstOffsets = targetOffsets;
        }

        Vector3 finalPos = target.transform.position + targetOffsets;
        finalPos.y = firsty;
        transform.position = finalPos;
        
        if (!work)
            return;
        
        variableUpdate();
        if (movementRequired())
        {
            if (Math.Abs(joyStickHorizontal)<=0.1&&Math.Abs(joyStickVertical)<=0.1)
             return;
            
            changeInAction = true;
            targetOffsets = getChangedOffset(targetOffsets, horizontalEffectOnAxis,ChangeFactor,true);
            targetOffsets=  getChangedOffset(targetOffsets, verticalEffectOnAxis,ChangeFactor,false);
             


        }else changeInAction = false;

        if (testReset)
        {
            testReset = false;
            resetPos();

        }
        
        
    }

    public void resetPos()
    {

        targetOffsets = firstOffsets;
    }
    private Vector3 getChangedOffset(Vector3 current, char Axis,   float differanceFactor,bool HelseV)
    {
        float minOffset = HelseV ? HminOffset : VminOffset;
        float maxOffset = HelseV ? HmaxOffset : VmaxOffset;
        
        Vector3 result = new Vector3(current.x, current.y, current.z);
         
        float allPower = Math.Abs(joyStickHorizontal) + Math.Abs(joyStickVertical)  ;
         
        float input=differanceFactor*( HelseV ? joyStickHorizontal : joyStickVertical );

        if (!useGradualSpeed)
        {
            input=differanceFactor*( HelseV ? joyStickHorizontal/allPower : joyStickVertical/allPower );
        }
        
        
         
        
        switch (Axis)
        {
            case 'x':
            case 'X':
                
                result.x += input;
                if (useMinMax)
                {
                    if (result.x < minOffset) result.x = minOffset;
                    if (result.x > maxOffset) result.x = maxOffset;

                }
                break;
            case 'y':
            case 'Y':
                result.y += input;
                if (useMinMax)
                {
                    if (result.y < minOffset) result.y = minOffset;
                    if (result.y > maxOffset) result.y = maxOffset;

                }
                break;
            case 'z':
            case 'Z':
                result.z += input;
                if (useMinMax)
                {
                    if (result.z < minOffset) result.z = minOffset;
                    if (result.z > maxOffset) result.z = maxOffset;

                }
                break;
        }

        return result;

    }

    private bool movementRequired()
    {
        if ((horizontalEffectOnAxis > joyStickDiscardMovementPercent / 100)||(verticalEffectOnAxis > joyStickDiscardMovementPercent / 100))
        {
            return true;

        }

        return false;
    }
}

