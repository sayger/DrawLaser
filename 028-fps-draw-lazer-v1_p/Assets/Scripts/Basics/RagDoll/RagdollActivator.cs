using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    [SerializeField] private bool beenActivated;
    
    [SerializeField] private bool Test;
    
    public bool useCertainForce =false;
    public Vector3 addAlways = new Vector3(0, 0, -10);
     
    public Rigidbody targetRigidbody;
    public float powerFactor=1;
    void Start()
    {
       
    }

    private void adjustments()
    {
        beenActivated = false;
        if (targetRigidbody==null)
        {
            targetRigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Test)
        {
            Test = false;
            ActivateIt();
        }
    }

   
    public void ActivateIt(Vector3 position,Quaternion rotation, Vector3 driftPower_And_Direction)
    {
        
        transform.position = position ;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        beenActivated = true;
        Vector3 temp = driftPower_And_Direction;
        if (useCertainForce)
        {
            temp += addAlways;
        }
        
        if (temp.magnitude>0)
        {
            targetRigidbody.AddForce(temp*powerFactor,ForceMode.Impulse);
        }
        
    }
    public void ActivateIt(Transform referenceTransform , Vector3 driftPower_And_Direction)
    {
        ActivateIt(referenceTransform.position, referenceTransform.rotation, driftPower_And_Direction);

    }
    public void ActivateIt(Transform referenceTransform )
    {
        ActivateIt(referenceTransform.position, referenceTransform.rotation, Vector3.zero);

    }
   
    public void ActivateIt(Vector3 position,Quaternion rotation )
    {
         
        ActivateIt( position,  rotation, Vector3.zero);
    }
    
    public void ActivateIt(Vector3 position,Vector3 driftPower_And_Direction)
    {
        ActivateIt(position, transform.rotation,driftPower_And_Direction);
    }
  
    public void ActivateIt(Quaternion rotation,Vector3 driftPower_And_Direction)
    {
        ActivateIt(transform.position,rotation ,driftPower_And_Direction);
    }

    public void ActivateIt( Vector3 driftPower_And_Direction)
    {
        ActivateIt(transform  , driftPower_And_Direction);
    }
    public void ActivateIt( )
    {
        ActivateIt(transform  ,Vector3.zero);
    }
}
