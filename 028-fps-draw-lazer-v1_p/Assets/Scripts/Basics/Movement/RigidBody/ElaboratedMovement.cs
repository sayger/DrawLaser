using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElaboratedMovement : MonoBehaviour
{
    private bool movementActive  = true;
    [SerializeField] private bool noCondition = false;
    [SerializeField] private bool moveWithStart = true;
    [SerializeField] private float playersSpeed = 10f;

    public bool xMove = false;
    public bool yMove = false;
    public bool zMove = false;

    public bool useLerp = false;
    public bool useForce = false;
    public int forceMode3 = 0;



    [SerializeField] private float newPlayersSpeed = 0;
    public float updatingFactor = 0.001f;
    private bool updateSpeed = false;
    private Vector3 desiredPosition;
    private Vector3 targetVector;
    private Rigidbody rb;

    public bool movePositive = true; 
    public float falseTarget = 10;

    public bool tryToUpdateSpeed = false;
    
    //------------------------------------------------------
    
    [SerializeField] private bool useSpecialConditions = false;
    
    //------------------------------------------------------------
    
    
    [SerializeField] private bool usePositiveObject = false;
    [SerializeField] private GameObject positiveObject;
    [SerializeField] private bool touchToActivate = false;
    [SerializeField] private bool passInXToActivate = false;
    [SerializeField] private char lessOrMoreXtoActivate = 'E';
    [SerializeField] private bool passInYToActivate = false;
    [SerializeField] private char lessOrMoreYtoActivate = 'E';
    [SerializeField] private bool passInZToActivate = false; 
    [SerializeField] private char lessOrMoreZtoActivate = 'E';
    
    //----------------------------------------------------------------
        
    [SerializeField] private bool useNegativeObject = false;
    [SerializeField] private GameObject negativeObject;
    [SerializeField] private bool touchToDeactivate = false;
    [SerializeField] private bool passInXToDeactivate = false;
    [SerializeField] private char lessOrMoreXtoDeactivate = 'E';
    [SerializeField] private bool passInYToDeactivate = false;
    [SerializeField] private char lessOrMoreYtoDeactivate = 'E';
    [SerializeField] private bool passInZToDeactivate = false; 
    [SerializeField] private char lessOrMoreZtoDeactivate = 'E';
    
    
    
    
    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        if (!movePositive) falseTarget *= -1;

        validateObjects();
        
        movementActive = moveWithStart;

         



    }

     

    private void validateObjects()
    {
        if (positiveObject == null)
            usePositiveObject = false;
        if (negativeObject == null)
            useNegativeObject = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (tryToUpdateSpeed)
        {
            setPlayerSpeed(newPlayersSpeed);
            tryToUpdateSpeed = false;
        }

        if (useSpecialConditions)
            checkSpecialConditions();
        
        if (movementActive || noCondition)
        {
            updatingSpeed();
            if (xMove)
                move('x');
            if (yMove)
                move('y');
            if (zMove)
                move('z');
        }
        
         
        
        
    }

    private void handBreak(bool makeStatic)
    {
        handBreak();
        //check velocity then makeStatic here;
        
    }
    private void handBreak()
    {
        throw new System.NotImplementedException();
        // reset velocity
        // find the velocity find reverse direction and counter power
        //oppose the velocity and  stop it 
        //check it again
    }

    private void checkVelocity()
    {
        throw new System.NotImplementedException();
        
        // find power and the direction of the velocity
    }

  

    
    
    public void stop()
    {
        noCondition = false;
        movementActive = false;
    }
    public void fullStop()
    {
        noCondition = false;
        movementActive = false;
        useSpecialConditions = false;
    }
    private void checkSpecialConditions()
    {
        if (usePositiveObject)
        {
           /* if (expr)
            {
                
            }*/
        }

        if (useNegativeObject)
        {
             
        }
    }

    private void activity(bool outcome)
    {
        if (moveWithStart)
        {
            movementActive = moveWithStart;
        }
        else
        {
            movementActive = outcome;
        }
    }

    private void move(char A)
    {
        var position = transform.position;
        var speed = playersSpeed;
        
        if (useLerp)
            speed /= 100;

        switch (A)
        {
            case 'x':
            case 'X':
            {
                float target = position.x + falseTarget;
                desiredPosition = new Vector3(target, transform.position.y, position.z);
                if (useLerp)
                {
                    desiredPosition = new Vector3(target , transform.position.y, position.z);
                    targetVector = Vector3.Lerp(position, desiredPosition, speed * Time.deltaTime);
                }
                else if (useForce)
                {
                    forceMove(0, speed);
                }
                else
                    targetVector = Vector3.MoveTowards(transform.position, desiredPosition, speed * Time.deltaTime);

                if (!useForce)
                    transform.position = targetVector;
                break;
            }
            case 'y':
            case 'Y':
            {
                float target = position.y + falseTarget;
                desiredPosition = new Vector3(transform.position.x, target, position.z);
                if (useLerp)
                {
                    desiredPosition = new Vector3(transform.position.x, target , position.z);
                    targetVector = Vector3.Lerp(position, desiredPosition, speed * Time.deltaTime);
                }
                else if (useForce)
                {
                    forceMove(1, speed);
                }
                else
                    targetVector = Vector3.MoveTowards(transform.position, desiredPosition, speed * Time.deltaTime);

                if (!useForce)
                    transform.position = targetVector;
                break;
            }
            case 'z':
            case 'Z':
            {
                float target = position.z + falseTarget;
                desiredPosition = new Vector3(transform.position.x, position.y, target);
                if (useLerp)
                {
                
                    desiredPosition = new Vector3(position.x, position.y, target);
                    targetVector = Vector3.Lerp(position, desiredPosition, speed * Time.deltaTime);
                }
                else if (useForce)
                {
                    forceMove(2,speed);
                }
                else
                    targetVector = Vector3.MoveTowards(transform.position, desiredPosition, speed * Time.deltaTime);

                if(!useForce)
                    transform.position = targetVector;
                break;
            }
        }
    }

    private void forceMove(int input,float speed)
    {
        var direction = new Vector3();
        switch (input)
        {
            case 0:
                direction = new Vector3(1f, 0f, 0f);
                break;
            case 1:
                direction = new Vector3(0f, 1f, 0f);
                break;
            case 2:
                direction = new Vector3(0f, 0f, 1f);
                break;
        }

        switch (forceMode3)
        {
            case 0:
                rb.AddForce(direction*speed,ForceMode.Impulse);
                break;
            case 1:
                rb.AddForce(direction*speed,ForceMode.Acceleration);
                break;
            case 2:
                rb.AddForce(direction*speed,ForceMode.Force);
                break;
            case 3:
                rb.AddForce(direction*speed,ForceMode.VelocityChange);
                break;
        }
    }
    
    
    public float getPlayerSpeed()
        {
            return this.playersSpeed;
        }

    public void setPlayerSpeed(float newSpeed)
    {
        if (!(newSpeed > playersSpeed + 1) && !(newSpeed < playersSpeed - 1)) return;
        newPlayersSpeed = newSpeed;
        updateSpeed = true;
         

    }

    private void updatingSpeed()
    {
        if (!updateSpeed) return;
        if (playersSpeed < newPlayersSpeed-1)
            playersSpeed += updatingFactor;
        else if (playersSpeed > newPlayersSpeed+1)
            playersSpeed -= updatingFactor;
        else updateSpeed = false;
    }
    }
