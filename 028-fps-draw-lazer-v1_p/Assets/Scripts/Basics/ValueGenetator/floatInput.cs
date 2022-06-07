using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatInput : MonoBehaviour
{
    public bool workOnce;
    private bool Once;
    
    public bool Work;
    public float saidFloat=0;
    public float Direction=-1;
    public float changeSpeed;
    public float start=0;
    public float stop=100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0)&&!Work)
        {
            if (Once)
            {
                return;
            }
            Work = true;
            Direction *= -1;
        }
        if (Work)
        {
            saidFloat += Time.deltaTime * changeSpeed*Direction;
            if (saidFloat<start)
            {
                if (workOnce)
                {
                    Once = true;
                }
                    Work = false;
                saidFloat = start;
                return;
            }
            if (saidFloat>stop)
            {
                if (workOnce)
                {
                    Once = true;
                }
                Work = false;
                saidFloat = stop;
                return;
            }
        }
    }
}
