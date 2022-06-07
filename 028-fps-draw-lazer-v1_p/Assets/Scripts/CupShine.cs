using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShine : MonoBehaviour
{
    [SerializeField] private float rotationFactor;
    void Update()
    {
        transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z+rotationFactor);
    }
}
