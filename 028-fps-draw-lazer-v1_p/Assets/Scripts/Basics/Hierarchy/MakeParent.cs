using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeParent : MonoBehaviour
{
    [SerializeField] private Transform toParent;
    [SerializeField] private Transform toChild;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void makeParent()
    {
        toChild.parent = toParent;
    }
}
