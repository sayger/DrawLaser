using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public DynamicJoystick variableJoystick;
    public Rigidbody rb;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        anim.SetFloat("horizontalprm", variableJoystick.Horizontal);
        anim.SetFloat("verticalprm", variableJoystick.Vertical);
    }
}