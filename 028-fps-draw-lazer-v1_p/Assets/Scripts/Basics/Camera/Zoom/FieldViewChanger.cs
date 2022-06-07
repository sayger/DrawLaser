using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldViewChanger : MonoBehaviour
{
    [SerializeField] private bool working=true;
    
    [SerializeField] private bool useReference ;
    [SerializeField] private bool getDelayedPercentage;
    
    [SerializeField] private DistanceBasedOperation referenceOperation;
    
    [SerializeField] private Camera _camera;
    
    [SerializeField] private Transform ChangeCenter;
    [SerializeField] private Transform EffectMaker;
    [SerializeField] private float currentTargetView;
    [SerializeField] private float changeSpeed;
    [SerializeField] private bool lerpElseMoveTowards;
    
    [SerializeField] private float closeView ;
    [SerializeField] private float distanceView;
    [SerializeField] private float minDistanceForEffect;
    
    [SerializeField] private float effectDistance ;
    [SerializeField] private float currentDistance ;

    [SerializeField] private bool takeDistanceX;
    [SerializeField] private bool takeDistanceY;
    [SerializeField] private bool takeDistanceZ;
    
        
    void Start()
    {
        adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (working)
        {
            AllProcess();
        }
        
    }

    private void adjustments()
    {
        if (_camera==null)
        {
            _camera=Camera.main;
            
        }

        if (referenceOperation==null)
        {
            useReference = false;
        }
    }

    private void AllProcess()
    {
       
        if (!useReference)
        {
            calculations();
        }
        else
        {
            getReferences();
        }
        
        changeView();

    }

    private void changeView()
    {
        Vector3 current = new Vector3(_camera.fieldOfView, 0, 0);
        Vector3 currentTarget = new Vector3(currentTargetView, 0, 0);
        
        
        
            Vector3 currentEffective = lerpElseMoveTowards
            ? Vector3.Lerp(current, currentTarget, (changeSpeed/10)*Time.deltaTime) // TODO TIME DELTA TIME MAYBe MAKING THIS LESS GOOD JUST MAYBE..
            : Vector3.MoveTowards(current, currentTarget, (changeSpeed)*Time.deltaTime);
            
            
        _camera.fieldOfView = currentEffective.x;
    }

    private void calculations()
    {
        Vector3 target = ChangeCenter.position;
        if (takeDistanceX) target.x = EffectMaker.position.x;
        if (takeDistanceY) target.y = EffectMaker.position.y;
        if (takeDistanceZ) target.z = EffectMaker.position.z;
        
        currentDistance = (Vector3.Distance(ChangeCenter.position, target));

        currentTargetView = closeView +
                            ((distanceView - closeView) * ((currentDistance - minDistanceForEffect) / effectDistance));
        if (currentDistance>minDistanceForEffect+effectDistance)
        {
            currentTargetView = distanceView;
        }
        else  if (currentDistance<minDistanceForEffect)
        {
            currentTargetView = closeView;
        }
    }

    private void getReferences()
    {
        float percent =getDelayedPercentage ? referenceOperation.delayedPercentage() : referenceOperation.currentPercentage();
        currentTargetView = closeView + (distanceView - closeView) * (percent/100);
    }
}
