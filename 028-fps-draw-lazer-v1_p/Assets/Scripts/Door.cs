using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] GameObject bloodOverlay;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            
            StartCoroutine(Blood());
           
        }
    }
    
    IEnumerator Blood()
    {
        bloodOverlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        bloodOverlay.gameObject.SetActive(false);
    }
}
