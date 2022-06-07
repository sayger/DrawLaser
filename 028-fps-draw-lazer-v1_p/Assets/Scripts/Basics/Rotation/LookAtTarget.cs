using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public bool _Active=true;
    public Transform subjectObj;
    public Transform target;
    void Start()
    {
         
        adjustments();
    }

    private void adjustments()
    {
        if (subjectObj==null)
        {
            subjectObj = transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_Active)
        {
            subjectObj.LookAt(target);
        }
    }

    public void activate()
    {
        _Active = true;
    }
}
