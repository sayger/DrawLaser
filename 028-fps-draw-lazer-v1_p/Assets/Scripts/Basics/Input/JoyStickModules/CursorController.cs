using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float speed;
    public Joystick variableJoystick;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
         float xMovement = -variableJoystick.Horizontal;
         float zMovement = -variableJoystick.Vertical;
        transform.position += new Vector3(xMovement, 0f, zMovement) * (speed * Time.deltaTime);
        Debug.Log(xMovement);
    }
}
