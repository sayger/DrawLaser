using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Hoe : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float growingScale;
    [SerializeField] private float moveScale;
    
    private void OnEnable()
    {
        
        Vector3 toGrow = new Vector3(transform.localScale.x * growingScale, transform.localScale.y * growingScale, transform.localScale.z* growingScale);
        Vector3 toDown = new Vector3(transform.position.x, transform.position.y+moveScale, transform.position.z);
        transform.DOScale(toGrow, moveDuration);
        transform.DOMove(toDown, moveDuration).SetEase(moveEase);
        transform.DORotate(new Vector3(0, 0, 180), moveDuration);
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
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
               // this.gameObject.GetComponent<BoxCollider>().enabled = false;
                blood.Play();
            }
        }
    }


    /*.SetEase(moveEase)
            .OnComplete(() =>
            {
                // text.transform.localScale = Vector3.one;
                // transform.localScale = firstScale;
                transform.DOScale(toGrow, moveDuration)
                    .OnComplete(()=> 
                    {
                        transform.localScale = firstScale;
                        this.gameObject.SetActive(false);
                    
                    });
                
            });*/
}
