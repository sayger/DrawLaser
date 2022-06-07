using System;
using System.Collections;
using System.Collections.Generic;using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionForce : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyParent"))
        {
            EnemyControl enemyControl = other.GetComponent<EnemyControl>();
            if ( enemyControl!=null)
            {
                enemyControl.fill = 0;
                
                
            }
           
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyParent"))
        {
            EnemyControl enemyControl = other.GetComponent<EnemyControl>();
            if ( enemyControl!=null)
            {
                enemyControl.fill = 0;
                
                
            }
           
        }
        if (other.gameObject.CompareTag("Box"))
        {
            Destroy(other.gameObject);
        }
        
    }

   
}
