using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody playerRb;
    public bool MoveForXorZ = true; 
    public bool reverseControls = false;
    public float speedFactor = 1;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    { 
        float mirror = 1;
        if (reverseControls)
            mirror = -1;
        if (MoveForXorZ)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * (Time.deltaTime*speedFactor * mirror));
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * (Time.deltaTime*speedFactor* mirror));
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.forward * (Time.deltaTime*speedFactor* mirror));
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.back * (Time.deltaTime*speedFactor* mirror));
            }
        }
       
    }
}
