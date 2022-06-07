using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollStateControl : MonoBehaviour
{
    [SerializeField] private bool explodeWithActivation   ;
    [SerializeField] private Vector3  explodeDirection=new Vector3();
    [SerializeField] private float  explodePower=1;
    [SerializeField] private Rigidbody explodeRg;
    
    
    
    [SerializeField] private bool activateTest;
    [SerializeField] private bool deActivateTest;
    [SerializeField] private List<Rigidbody> allRigidBodies=new List<Rigidbody>();
    [SerializeField] private List<Collider> allColliders=new List<Collider>();
    
    
    void Start()
    {
        explodeDirection = explodeDirection.normalized;
        allRigidBodies = GetComponentsInChildren<Rigidbody>().ToList();
        allColliders = GetComponentsInChildren<Collider>().ToList();
    }

    void FixedUpdate()
    {
        if (deActivateTest)
        {
            deActivateTest = false;
            DeActivateIt();
        }
        if (activateTest)
        {
            activateTest = false;
            ActivateIt();
        }
        if (explodeWithActivation)
        {
            explodeWithActivation = false;
            explode(explodePower*explodeDirection);
        }
    } 
    public  void explode(Vector3 powerAndDirection)
    {
         ActivateIt();
         explodeRg.AddForce(powerAndDirection);
    }
    private  void DeActivateIt()
    {
        foreach (var VARIABLE in allRigidBodies)
        {
            VARIABLE.detectCollisions = false;
            VARIABLE.isKinematic = true;
           // VARIABLE.useGravity = false;
            
        }

        foreach (var VARIABLE in allColliders)
        {
            VARIABLE.isTrigger = true;
        }
    }
    
    private  void ActivateIt()
    {
        foreach (var VARIABLE in allRigidBodies)
        {
            VARIABLE.detectCollisions = true;
            VARIABLE.isKinematic = false;
            // VARIABLE.useGravity = true;
            
        }
        foreach (var VARIABLE in allColliders)
        {
            VARIABLE.isTrigger = false;
        }
    }
}
