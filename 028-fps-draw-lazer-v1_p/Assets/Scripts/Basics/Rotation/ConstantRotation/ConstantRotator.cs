using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotator : MonoBehaviour
{
    public bool working=true;
    
    public Vector3 speeds=new Vector3(1,1,1);
    public Vector3  turningDirections=new Vector3(1,1,1);

    [SerializeField] private bool turnX;
    [SerializeField] private bool turnY;
    [SerializeField] private bool turnZ;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (working)
        {
            Vector3 current = transform.localRotation.eulerAngles;

            if (turnX)
            {
                current.x += speeds.x * turningDirections.x * Time.deltaTime;
            }

            if (turnY)
            {
                current.y += speeds.y * turningDirections.y * Time.deltaTime;
            }

            if (turnZ)
            {
                current.z += speeds.z * turningDirections.z * Time.deltaTime;
            }
            
            transform.localRotation=Quaternion.Euler(current);
        }
    }
}
