using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ragdoll : MonoBehaviour
{
    public bool RagdollActivator;
    public Animator ThiefCharacterAnim;
   
    public bool addforce;
    public List<Rigidbody> RB = new List<Rigidbody>(); 
  
    void Start()
    {
     
        ragdolClose();
      //  parentCol(true);
        
    }

    private void Update()
    {
        if (RagdollActivator == true)
        {
            ragdollOpen();
        }
        else
        {
            ragdolClose();
        }


    }

    public void falss()
    {

        addforce = false;
    }

    void ragdolClose()
    {
        physiccState(true);
        isKinematic(true);
        capsuleTrigger(true);
        boxColliderTrigger(true);
        capsuleColl(false);
        boxCollider(false);
        SphereTrigger(true);
        SphereCollider(false);
    }
    
    void physiccState(bool state)
    {
        Rigidbody[] rg = GetComponentsInChildren<Rigidbody>();

        foreach (var VARIABLE in rg)
        {
            VARIABLE.useGravity = state;
        }
   
    }   
    void isKinematic(bool state)
    {
        Rigidbody[] rg = GetComponentsInChildren<Rigidbody>();

        foreach (var VARIABLE in rg)
        {
            VARIABLE.isKinematic = state;
        }
   
    }
    public void AdForceee(bool state)
    {
        Rigidbody[] rg = GetComponentsInChildren<Rigidbody>();

        foreach (var VARIABLE in rg)
        {
            VARIABLE.AddForce(new Vector3(-1f, 2f, -1));
        }
   
    }  
    
    void capsuleColl(bool state)
    {
        CapsuleCollider[] col = GetComponentsInChildren<CapsuleCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.enabled = state;
            
        }

    }
    void capsuleTrigger(bool state)
    {
        CapsuleCollider[] col = GetComponentsInChildren<CapsuleCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.enabled = state;
            
        }

    } 
    void SphereCollider(bool state)
    {
        SphereCollider[] col = GetComponentsInChildren<SphereCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.enabled = state;
            
        }

    } 
    void SphereTrigger(bool state)
    {
        SphereCollider[] col = GetComponentsInChildren<SphereCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.isTrigger = state;
            
        }

    } 
   
    void parentCol(bool state)
    {
        CapsuleCollider[] col = GetComponentsInParent<CapsuleCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.isTrigger = state;
        }

    } 

    void RigidbodyParentClosed(bool state)
    {
        Rigidbody rb = GetComponentInParent<Rigidbody>();

        Destroy(rb);


    } 
    void boxCollider(bool state)
    {
        BoxCollider[] col = GetComponentsInChildren<BoxCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.enabled = state;
        }

    }
    void boxColliderTrigger(bool state)
    {
        BoxCollider[] col = GetComponentsInChildren<BoxCollider>();

        foreach (var VARIABLE in col)
        {
            VARIABLE.isTrigger = state;
        }

    }

    public void ragdollOpen()
    {
        ThiefCharacterAnim.enabled = false;
        boxColliderTrigger(false);
        capsuleTrigger(false);
       // NavmeshControllerClosed(true);
       if (addforce == true)
       {
          AdForceee(true);
       }
       RigidbodyParentClosed(true);
       
      //  NavmeshClosed(true);
      
      
        parentCol(false);
        physiccState(true);
        capsuleColl(true);
        boxCollider(true);
        isKinematic(false);
        SphereTrigger(false);
        SphereCollider(true);
    }
    
}
