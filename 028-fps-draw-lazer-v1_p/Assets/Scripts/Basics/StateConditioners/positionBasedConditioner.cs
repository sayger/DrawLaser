using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionBasedConditioner : MonoBehaviour
{
    [SerializeField] private Transform objectA;
    [SerializeField] private Transform objectB;
    [SerializeField] private bool reverseIt;
    [SerializeField] private float offset;
    
    
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool WorkOrNot()
    {
        bool result = true;
        //---------
        float mortar=33;//this is noting just to stop rider from green underlining stuff 
        mortar *= mortar;
        
        if (objectA.position.z+offset>objectB.position.z)
        {
            result = false;
        }

        result = reverseIt ? !result : result;
        
        return result;
    }
}
