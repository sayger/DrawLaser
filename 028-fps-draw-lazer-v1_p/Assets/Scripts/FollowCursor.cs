using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCursor : MonoBehaviour
{
    public bool Work = true;
 //   public Vector2 
    public Vector2 TrueMousePosition;
    public bool TouchIsActive;
    public RectTransform _rectTransform;
    public bool activateDeactivateIMAGE=true;
    public Image _image;
    //----------------------------------------
    public bool useWorldObjToScreenReference;
    public bool useScreenLimits=true;
    public bool useCustomLimits;
    public Vector2 HLimits;
    public Vector2 VLimits;
    
    public bool useScreenLimitPercentages;
    public Vector2 HLimitsPercentages;
    public Vector2 VLimitsPercentages;
    
    public Camera _camera;
    public Transform referenceWorldObj;
    
    public Vector2 ReferencedScreenPos;
    public Vector2 Offset;
    public Vector2 RequestedScreenPos;
    
    public bool AddOffsetToReference;
    public bool OffsetWithScreenSizePercent;
    public Vector2 OffsetPercent;
    public bool useExactTransition=true;
    public bool UseLerpElseMoveForward;
    public float transitionSpeed;

    public bool updateScreenLimitsAlways;
    
    void Start()
    {
        adjustments();
    }

    private void adjustments()
    {
        resolveInconsistency();
        calculateLimits();
        
        
        
       
       
    }

    private void resolveInconsistency()
    {
        if (_rectTransform==null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        if (_image==null)
        {
            _image = GetComponent<Image>();
        }

        if (_rectTransform==null||(activateDeactivateIMAGE&&_image==null))
        {
            Work = false;
        }
        //-----------------
        if (referenceWorldObj==null)
        {
            useWorldObjToScreenReference = false;
        }
        if (_camera==null)
        {
            _camera=Camera.main;
            
        }
        if (_camera==null)
        {
            useWorldObjToScreenReference = false;
        }

    }
    private void calculateLimits()
    {
        if (!useScreenLimits) return;
         
        if (useCustomLimits)
        {
         
        }
        else
        {
            HLimits[0] = 0;
            HLimits[1] = Screen.width;
            VLimits[0] = 0;
            VLimits[1] = Screen.height;
            
        }
    }
     
    void LateUpdate()
    {
        UpdateValues();
        if (Work)
        {
            if (TouchIsActive)
            {
                TouchActiveActions();
            }
        }

    }

    private void UpdateValues()
    {
        if (updateScreenLimitsAlways&&(useCustomLimits))
        {
            calculateLimits();
        }
        TrueMousePosition = Input.mousePosition;
        if (useWorldObjToScreenReference)
        {
            ReferencedScreenPos = _camera.WorldToScreenPoint(referenceWorldObj.position);
        }
        TouchState();
    }
    private void TouchState()
    {
        if (!TouchIsActive &&Input.GetMouseButton(0))
        {
            TouchIsActive = true;
            TouchActiveStartActions();
        }
        if (TouchIsActive &&!Input.GetMouseButton(0))
        {
            TouchIsActive = false;
            TouchActiveStateEndActions();
        }
    }

    private void TouchActiveActions()
    {
        if (useWorldObjToScreenReference)
        {
            if (useExactTransition)
            {
                // ReferencedScreenPos = _camera.WorldToScreenPoint(referenceWorldObj.position);
                _rectTransform.position = ReferencedScreenPos ;
            }
            else
            {
                if (UseLerpElseMoveForward)
                {
                    _rectTransform.position = Vector3.Lerp(_rectTransform.position,ReferencedScreenPos,transitionSpeed*Time.deltaTime);
                }
                else
                {
                    _rectTransform.position = Vector3.MoveTowards(_rectTransform.position,ReferencedScreenPos,transitionSpeed*Time.deltaTime);
                }
            }

        }
        
    }


    private void TouchActiveStartActions()
    {
        if (!Work) return;
        if (activateDeactivateIMAGE)
        {
            _image.enabled = true;
        }
        
    }

    private void TouchActiveStateEndActions()
    {
        if (!Work) return;
        if (activateDeactivateIMAGE)
        {
            _image.enabled = false;
        }
        
    }

    public void StateChange(bool ActivateElseDeActivate)
    {
        if (ActivateElseDeActivate)
        {
            Work = true;
        }
        else
        {
            TouchActiveStateEndActions();
            Work = false;
            
        }
        
    }
}
