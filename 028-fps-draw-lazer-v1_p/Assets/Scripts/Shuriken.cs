using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
public class Shuriken : MonoBehaviour
{
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float growingScale;
    [SerializeField] private Transform target;


    
    private void OnEnable()
    {
        Vector3 toGrow = new Vector3(transform.localScale.x * growingScale, transform.localScale.y * growingScale, transform.localScale.z* growingScale);
        transform.DOScale(toGrow, moveDuration);
        transform.DOMove(target.position, moveDuration).SetEase(moveEase);
        transform.DORotate(new Vector3(transform.eulerAngles.x, 180, 0), moveDuration);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyParent"))
        {
            
            if (other.gameObject.GetComponent<BoxCollider>() !=null
                && other.gameObject.GetComponentInChildren<Animator>()!=null )
            {
                Animator animator;

                animator = other.gameObject.GetComponentInChildren<Animator>();
                animator.enabled = false;
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                other.gameObject.GetComponent<NavMeshAgent>().speed = 0;
                other.gameObject.GetComponent<EnemyControl>().fill = 0;
                other.gameObject.GetComponent<EnemyControl>().attackRun = false;
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
                other.gameObject.transform.position = this.transform.position;
                other.gameObject.transform.parent = this.transform;
                // this.gameObject.GetComponent<BoxCollider>().enabled = false;
                // blood.Play();
            }

            other.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
