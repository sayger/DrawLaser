using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LocalOffsetManipulator : MonoBehaviour
{
    [SerializeField] public bool working = true;
    public bool returnToCenter;
    [SerializeField] private bool inputHorizontalElseVertical=true;
    [SerializeField] private char worldAxis='x';
    [SerializeField] private bool reverseControls = false;
    [SerializeField] private bool allowWastedTouch=false;
    [SerializeField] private float basePixelPercent=1f;
    [SerializeField] private float worldEquivalent=0.1f;
    [SerializeField] private bool useWorldLimit=false ;
    [SerializeField] private bool makeScreenMaxEqualsToLimits=false ;  
    [SerializeField] private float [] limitMinMax=new float[2];
    private float reverseFactor = 1f;
    
    private float _firstWorld;
    private float _currentWorld;
    
    private float _firstScreen;
    // private float _currentScreen;

    private bool _onTheMove = false;

    private float _basePixelCount;
    // private float _baseWorldDistance;

    public float currentAxisMust;
    void Start()
    {
        if (reverseControls)
            reverseFactor = -1;

        limitMinMax = checkOrder(limitMinMax);

        calculateRates();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (working)
        {
            reverseFactor = reverseControls ? -1 : 1;

            if (firstTouch())
            {
                resetValues();
            }

            if (fingerMove())
            {
                move();
            }

            if (_onTheMove && !allowWastedTouch)
            {
                discardExtra();
            }
        }
    }
    private void move()
    {

         
            soloMove();
         



    }
    private Vector3 assignAxis(char A,float replacement, Transform subject)
    {
       // var position = subject.position;
       var position = subject.localPosition;
        var result= new Vector3(position.x,position.y,position.z);

        switch (A)
        {
            case 'x':case 'X':

                result.x = replacement;
                break;
            case 'y':case 'Y':
                
                result.y = replacement;
                break;
            case 'z':case 'Z':
                
                result.z = replacement;
                break;
        }


        return result;
    }
    private void soloMove()
    {

        if ( ( ! maxLimitBreach()&&!minLimitBreach() ) || ( maxLimitBreach()&&asksForDecrease() ) || ( minLimitBreach()&& asksForIncrease() ) )
        {
            var addition = ((getCurrentScreen()-_firstScreen)/_basePixelCount)*worldEquivalent*reverseFactor;
            var axisMust = (_firstWorld + addition);
            
            if (useWorldLimit)
            {
                axisMust = axisMust < limitMinMax[0] ? limitMinMax[0] : axisMust;
                axisMust = axisMust > limitMinMax[1] ? limitMinMax[1] : axisMust;
            }

            if (returnToCenter)
            {
                axisMust = currentAxisMust;
            }
            else
            {
                currentAxisMust = axisMust;
            }
            
            
            transform.localPosition = assignAxis(worldAxis, axisMust, transform);  
            //transform.position = assignAxis(worldAxis, axisMust, transform);
        }
        



    }

    public void autoReturnCenter()
    {
        returnToCenter = true;
        DOTween.To(() => currentAxisMust, x => currentAxisMust = x, 0, 2).OnComplete(() =>
        {
            returnToCenter = false;
            working = false;
        });
    }
    private void resetValues()
    {

        _firstScreen = getCurrentScreen();
        _firstWorld = getCurrentWorld(worldAxis,transform);


    }
    private void discardExtra()
    { 
        if ((maxLimitBreach()&&asksForIncrease())||(minLimitBreach()&&asksForDecrease()) )
            resetValues(); 
    }
    private bool maxLimitBreach()
    { 
        return !useWorldLimit || (getCurrentWorld(worldAxis, transform) > limitMinMax[1]);
 
    }
    private bool minLimitBreach()
    { 
        return !useWorldLimit || (getCurrentWorld(worldAxis,transform)<limitMinMax[0]);
 
    }
    private bool asksForIncrease()
    { 
        return reverseControls ? (getCurrentScreen() < _firstScreen) : (getCurrentScreen()  > _firstScreen);
 
    }
    private bool asksForDecrease()
    { 
        return reverseControls ? (getCurrentScreen() >_firstScreen ) : (getCurrentScreen() <_firstScreen );
 
    }
    private float getCurrentScreen()
    {
        return inputHorizontalElseVertical ? Input.mousePosition.x : Input.mousePosition.y;
    }
    
    private float getCurrentWorld(char A,Transform subject)
    {
        float result=0;

        switch (A)
        {
            case 'x':case 'X':

               // result = subject.position.x;
               result = subject.localPosition.x;
               break;
            case 'y':case 'Y':
                
               // result= subject.position.y;
                result= subject.localPosition.y;
                break;
            case 'z':case 'Z':
                
               // result = subject.position.z;
                result = subject.localPosition.z;
                break;
        }

        return result;
        
    }
    private bool firstTouch()
    {
        if(!_onTheMove&& (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButton(0)))
        {
            _onTheMove = true;
            return true;
        }
        return false;
    }
    private bool fingerMove()
    {
        if (_onTheMove&&(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0)))
        {
           
            return true;
        }
        else if(_onTheMove)
            _onTheMove = false;
        
        return false;
    }
    
    private void calculateRates()
    {

        float totalPixelCount = Screen.width;

        _basePixelCount = (totalPixelCount / 100) * basePixelPercent;

        if (makeScreenMaxEqualsToLimits)
        {
            float nineOfTen = totalPixelCount - (totalPixelCount / 10);

            float limitWidth = limitMinMax[1] - limitMinMax[0];

            worldEquivalent = ((int )((limitWidth / (nineOfTen / _basePixelCount))*100)) /100f;

        }
         


    }
    
    private float [] checkOrder(float[] input) // being fancy eh ? 
    {
        if (!(input[0] > input[1])) return input;
        
        input[0] += input[1];
        input[1] = input[0] - input[1];
        input[0] = input[0] - input[1];

        return input;


    }
    
}
