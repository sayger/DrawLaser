using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserRagdollActivator : MonoBehaviour
{
    public Vector3 addAlways = new Vector3(0, 0, -10);
    public bool applyPower=true;
    public Rigidbody targetRigidbody;
    public float powerFactor=1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateIt(Transform position, Vector3 driftPower_Direction)
    {
        transform.position = position.position;
        transform.rotation = position.rotation;
        gameObject.SetActive(true);
        Vector3 temp = driftPower_Direction;
        temp += addAlways;
        if (applyPower)
        {
            targetRigidbody.AddForce(temp*powerFactor,ForceMode.Impulse);
        }
        
    }
}
