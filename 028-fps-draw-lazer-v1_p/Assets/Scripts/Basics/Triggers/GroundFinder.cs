using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinder : MonoBehaviour
{
    [SerializeField] private bool ShowCheckLine=false;
    private bool inGameMode = false;
    [Header("  ")] 
    
    [SerializeField] private bool work=true;
    
    
    public bool GroundTouch;
    public bool NoGround;
    public GameObject TouchedGround;
    
    [SerializeField] private Transform Observer;
    [SerializeField] private float TouchCheckDistance;
    [SerializeField] private LayerMask AcceptedLayers;
    
    [SerializeField] private Vector3 FindDirection;
    private Vector3 _FindDirection;
    [SerializeField] private bool DirectionEffectLocal;
    [SerializeField] private float FindDistance=500;
    public GameObject ClosestGround;
    public float DistanceToGround;

     
    [SerializeField]  private Collider[] touchedObjects = new Collider[20];

    private bool TransformCheck;
    void Start()
    {
        Adjustments();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AllProcess();
    }

    private void Adjustments()
    {
        inGameMode = true;
        checkEssentials();
    }

    private bool CheckGroundTouch(Transform position,LayerMask layerMask,float distance)
    {
        return Physics.CheckSphere(position.position, distance, layerMask);
    }

    private void AllProcess()
    {
        checkGroundTouch();

        findGround();



    }

    private void findGround()
    {
        _FindDirection = !DirectionEffectLocal ? FindDirection : getLocalDirection(FindDirection, Observer);
        
        if (Physics.Raycast (Observer.position,_FindDirection, out var hit, FindDistance, AcceptedLayers))
        {
            ClosestGround =  (hit.collider.gameObject);
            NoGround = false;
            DistanceToGround = Vector3.Distance(Observer.position, hit.point);
            if (ShowCheckLine)
            {
                Gizmos.DrawLine(Observer.position,hit.point);
            }
        }
        else
        {
            ClosestGround = null;
            NoGround = true;
        }



    }

    private void checkGroundTouch()
    {
        GroundTouch = CheckGroundTouch(Observer, AcceptedLayers,TouchCheckDistance );
        if (GroundTouch)
        {
            var size = Physics.OverlapSphereNonAlloc(Observer.position, TouchCheckDistance, touchedObjects);
            foreach (var VARIABLE in touchedObjects)
            {
                GameObject tempObj = VARIABLE.gameObject;
                if ( ((AcceptedLayers.value & (1 <<tempObj.layer)) > 0))
                {
                    TouchedGround = tempObj;
                    break;
                }
            }
        }
    }

    private void checkEssentials()
    {
        if (Observer==null)
        {
            Observer = transform;
        }

        if (TouchCheckDistance<=0)
        {
            TouchCheckDistance = Observer.localScale.y / 2;
        }
        
    }
    private Vector3 getLocalDirection(Vector3 planedDirection, Transform subject,float power = 1)//surely there is a better way for this...
    {
        Vector3 localYDirection = subject.TransformDirection(Vector3.up)*planedDirection.y;
        Vector3 localXDirection = subject.TransformDirection(Vector3.right)*planedDirection.x;
        Vector3 localZDirection = subject.TransformDirection(Vector3.forward)*planedDirection.z;

        return  power * (localYDirection + localXDirection + localZDirection) ;//to make it effect same as non local option
        //if power is given it can directly calculate velocity factor//TODO i find the reason local power low power effect " /3 " were right up here
    }
    
    private void OnDrawGizmos()
    {
        if (!work) return;
        
        if (!TransformCheck)
        {
            TransformCheck = true;
            checkEssentials();
        }
        if (ShowCheckLine&&!inGameMode)
        {
            AllProcess();
        }



    }
}
