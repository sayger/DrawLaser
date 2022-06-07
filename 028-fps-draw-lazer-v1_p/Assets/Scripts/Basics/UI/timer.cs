using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    [SerializeField] private Text timeText
        ;
    [SerializeField] private float time=0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time +=  Time.deltaTime;
        timeText.text =  time.ToString();
    }
}
