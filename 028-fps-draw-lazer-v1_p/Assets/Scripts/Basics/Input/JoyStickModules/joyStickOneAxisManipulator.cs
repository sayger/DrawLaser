using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joyStickOneAxisManipulator : MonoBehaviour
{
    public bool takeHorizontal = true;
    public bool takeVertical = true;
    public float joyStickHorizontal;
    public float joyStickVertical;
    public bool InputAccepted; 
    [Header(" Sources  ")] 
    [SerializeField] private Joystick joystick ;//joystickFloating
    
    [SerializeField] private bool originalPowerAllWays;
    [SerializeField] private float InputPercent;
    [SerializeField] private float minEffectPercent=0;
    [SerializeField] private float maxEffectPercent=100;
    [SerializeField] private float joyStickDiscardPercent = 5; // DON
    private float lastJoyStickDiscardPercent  ; 
    private float _joyStickDiscardPercent = 0;
    [SerializeField] private char effectAxis='x';
     
    [SerializeField] private bool reverseHorizontal;
    [SerializeField] private bool reverseVertical;

    [SerializeField] private float Power;
    private float _Power;

    [SerializeField] private bool  useLimiter;
    [SerializeField] private Transform upLimit;
    [SerializeField] private Transform downLimit;
    
    
    
    void Start()
    {
        _Power = Power;
        if (upLimit==null||downLimit==null)
        {
            useLimiter = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        variableUpdate();
        Power = resizePower(_Power);

        takeEffect(Power);

    }

    private void takeEffect(float changeSpeed)
    {
        Vector3 current = transform.position;
        

        if (effectAxis=='x'||effectAxis=='X')
        {
            current.x += Time.deltaTime * changeSpeed;
        }
        if (effectAxis=='y'||effectAxis=='Y')
        {
            current.y += Time.deltaTime * changeSpeed;
        }
        if (effectAxis=='z'||effectAxis=='Z')
        {
            current.z += Time.deltaTime * changeSpeed;
        }

        if (useLimiter)
        {

            Vector3 dowLimitPos = downLimit.position;
            Vector3 upLimitPos = upLimit.position;
            if (effectAxis=='x'||effectAxis=='X')
            {
                if (current.x<dowLimitPos.x)
                {
                    current.x = dowLimitPos.x;
                }
                if (current.x>upLimitPos.x)
                {
                    current.x = upLimitPos.x;
                }
            }
            if (effectAxis=='y'||effectAxis=='Y')
            {
                if (current.y<dowLimitPos.y)
                {
                    current.y = dowLimitPos.y;
                }
                if (current.y>upLimitPos.y)
                {
                    current.y = upLimitPos.y;
                }
            }
            if (effectAxis=='z'||effectAxis=='Z')
            {
                if (current.z<dowLimitPos.z)
                {
                    current.z = dowLimitPos.z;
                }
                if (current.z>upLimitPos.z)
                {
                    current.z = upLimitPos.z;
                }
            }
            
            
        }

        transform.position = current;
    }
    private void variableUpdate()
    {
      //  touchUpdate();
      
      
        updateInput();
        InputAccepted = checkEligibility();
        
        if (Math.Abs(joyStickDiscardPercent - lastJoyStickDiscardPercent) > 1)
        {
            setTolerance();
        }
        
    }
    private bool checkEligibility()// checkInput feed
    {
        return ( calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100) 
                 > _joyStickDiscardPercent);
        

    }
    public float resizePower(float naturalPower)
    {
        variableUpdate();
        if (!InputAccepted)
        {
            return 0;
        } 
        else if (originalPowerAllWays)
        {
            return naturalPower;
        }
        InputPercent = calculateInputPercent(joyStickHorizontal,joyStickVertical,1,100);

        float factorial = minEffectPercent + // TODO change this with resizePercent
                          (  ((InputPercent - _joyStickDiscardPercent) / (100 - _joyStickDiscardPercent))
                             *(maxEffectPercent-minEffectPercent)  );

        int direction = joyStickVertical < 0 ? -1 : 1;
        return direction* naturalPower*factorial/100;
    }
    private float calculateInputPercent(float horizontal,float vertical,float inputMax,float percentageOver)
    {
        double squareH = Math.Pow(horizontal, 2);
        double squareV = Math.Pow(vertical, 2);
        float result = (((float) Math.Sqrt(squareH + squareV ))/inputMax)*percentageOver; 

        return result;
    }
    private void updateInput()
    {
        int reverseH = reverseHorizontal ? -1 : 1;
        int reverseV = reverseHorizontal ? -1 : 1;
        
         
         
            joyStickHorizontal = joystick.Horizontal*reverseH;
            joyStickVertical = joystick.Vertical*reverseV;
         
       

        if (! takeHorizontal  )
        {
            joyStickHorizontal = 0;
        }

        if (!takeVertical)
        {
            joyStickVertical = 0;
        }
    }
    
    private void setTolerance()
    {
        joyStickDiscardPercent = joyStickDiscardPercent < 0 ? 0 :
            joyStickDiscardPercent > 100 ? 100 :joyStickDiscardPercent; 
        
        lastJoyStickDiscardPercent = joyStickDiscardPercent;
        //  _joyStickDiscardPercent = adjustPercentage(joyStickDiscardPercent); ;
        _joyStickDiscardPercent = joyStickDiscardPercent ; ;
    }
}
