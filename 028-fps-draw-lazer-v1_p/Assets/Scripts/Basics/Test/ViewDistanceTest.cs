using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDistanceTest : MonoBehaviour
{

    [SerializeField] private Transform ObjectA;
    [SerializeField] private Transform objectB;
    [SerializeField] private float CurrentDistance;
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CurrentDistance = Vector3.Distance(ObjectA.position, objectB.position);
    }
}
