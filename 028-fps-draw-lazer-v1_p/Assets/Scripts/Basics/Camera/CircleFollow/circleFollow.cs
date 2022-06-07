using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleFollow : MonoBehaviour
{
    [SerializeField] private bool inTheCircle;
     
    
    [SerializeField] private GameObject target;
    [SerializeField] private float circleRadius=5f;
    [SerializeField] private float correctionSpeed ;
    [SerializeField] private GameObject referenceCircle;
    

    private void Awake()
    {
        transform.position = target.transform.position;
        if (referenceCircle!=null)
        {
           // referenceCircle.GetComponent<Renderer>().enabled = false;
            circleRadius = referenceCircle.transform.localScale.x / 2;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inTheCircle = checkIfItInTheCircle();
        if (!inTheCircle)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
                correctionSpeed * Time.deltaTime);
        }

    }

    private bool checkIfItInTheCircle()
    {
        if (Vector3.Distance(target.transform.position,transform.position)>circleRadius)
        {
            return false;
        }

        return true;

    }

}
