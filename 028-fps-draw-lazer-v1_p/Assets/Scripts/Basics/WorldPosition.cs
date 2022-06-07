using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPosition : MonoBehaviour
{
    public Vector3 World_Position = new Vector3();
    public Vector3 World_Rotation = new Vector3();
    public Vector3 World_Scale = new Vector3();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        var transform1 = transform;
        World_Position = transform1.position;
        World_Rotation = transform1.rotation.eulerAngles;
        World_Scale = transform1.lossyScale;
    }
}
