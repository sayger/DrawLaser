using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinnishLine : MonoBehaviour
{
    public bool finnishOnAction = false;

    [SerializeField] private bool callSpecialFinnish;
    [SerializeField] private bool ifSpecialCallNextLevel=true;
    

    [SerializeField] private float waitingTime=3f;

    [SerializeField] private int checkLayerNumber=9;

    [SerializeField] private bool forceToBe=true;
    
    
    
    private void Start()
    {
        if (forceToBe)
        {
            GetComponent<Renderer>().enabled=false;  
            GetComponent<Collider>().isTrigger = true;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!finnishOnAction && other.gameObject.layer == checkLayerNumber)
        {

            if (callSpecialFinnish)
            {
                //GameManager.Instance.specialFinnish(waitingTime, ifSpecialCallNextLevel);
                SectoralLevelManager.Instance.endIsNow = true;
            }
            else
            {
              //  GameManager.Instance.LoadNextLevel(waitingTime);
             

                SectoralLevelManager.Instance.endIsNow = true;
            }





            finnishOnAction = true;
        }
    }
}
