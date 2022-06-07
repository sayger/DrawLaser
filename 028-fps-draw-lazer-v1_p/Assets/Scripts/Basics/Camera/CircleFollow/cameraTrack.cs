using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrack : MonoBehaviour
{

    [SerializeField] private bool reCenteringActive;
    [SerializeField] private bool onTheMoveTrackActive;
    [SerializeField] private float centerRadius;
    
    [SerializeField] private bool outTrueCircle;// ????
    [SerializeField] private bool outTheCircle;
    [SerializeField] private bool wereOutTheCircle;
    
    [SerializeField] private bool currentlyMoving;
    [SerializeField] private bool wereMoving;
    
    
    
    [SerializeField] private bool lerpReCenter;
    [SerializeField] private bool moveForwardReCenter;
    [SerializeField] private float reCenteringSpeed;
    
    [SerializeField] private bool absoluteTracking;
    [SerializeField] private bool lerpTracking;
    [SerializeField] private bool moveForwardTracking;
    [SerializeField] private Vector3 firstEverOffsets;


    [SerializeField] private OldJoystickMover movementMaker;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        outTheCircle=  checkIfItOut();
        currentlyMoving=updateMovement();

       /* if (movementChanged()&&outTheCircle)
        {
            if (!currentlyMoving)//make the position new center now
            {
                
            }
            else
            {
                
            }
        }*/

       
        if (currentlyMoving&&!outTheCircle)// in the circle
        {
            // on the move track
        }
        if (currentlyMoving&&outTheCircle)// out the circle
        {
            
        }
        if (!currentlyMoving&&!outTheCircle)// in the circle
        {
            
        }
        if (!currentlyMoving&&outTheCircle)// out the circle
        {
            
        }
        
        
        
    }

    /*private bool movementChanged()
    {




    }*/

    

    private bool checkIfItOut()
    {
        bool result=false;

        return result;

    }

    private bool updateMovement()
    {
       return movementMaker.onTheMove;

    }
}
