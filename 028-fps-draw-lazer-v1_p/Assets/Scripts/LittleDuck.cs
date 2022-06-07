using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LittleDuck : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyParent"))
        {
            
            other.gameObject.GetComponent<EnemyControl>().LaughingAnimation(true);
            other.gameObject.GetComponent<NavMeshAgent>().speed = 0;

        }
       
    }
}
