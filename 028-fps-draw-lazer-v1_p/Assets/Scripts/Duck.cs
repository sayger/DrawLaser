using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
public class Duck : MonoBehaviour
{
    [SerializeField] private ParticleSystem blood;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float growingScale;
    [SerializeField] private float moveScale;
    [SerializeField] private GameObject littleDuck;
    [SerializeField] private GameObject drawTarget;
    [SerializeField] private Transform target;
    private void OnEnable()
    {
        if (target !=null)
        {
            transform.position = new Vector3(target.position.x, target.position.y + 47f, target.position.z);
        }
        
        Vector3 firstScale = transform.localScale;
        Vector3 firstPosition = transform.position;
        Vector3 toGrow = new Vector3(transform.localScale.x * growingScale, transform.localScale.y * growingScale, transform.localScale.z* growingScale);
        Vector3 toDown = new Vector3(transform.position.x, transform.position.y+moveScale, transform.position.z);
        transform.DOScale(toGrow, moveDuration);
        transform.DOMove(toDown, moveDuration).SetEase(moveEase);
        littleDuck.gameObject.SetActive(false);
        drawTarget.gameObject.SetActive(false);
        

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
                blood.Play();
            }
        }
    }
}
