using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LazerMove : MonoBehaviour
{

    public Transform hitPoint;
    [SerializeField] private float moveDuration = 10f;
    [SerializeField] private float decreaseFillAmount;
    [SerializeField] private ParticleSystem bulletExplosion;
    public int count = 1;
    private Transform temp;

    private void Start()
    {
        goEnemy();
    }

    public void goEnemy()
    {
     
         
            transform.DOMove(hitPoint.position, moveDuration).OnComplete(() =>
            {
                hitPoint = null;
                Destroy(this.gameObject);
            });
        
       

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyParent"))
        {
            collision.gameObject.GetComponent<EnemyControl>().fill -= decreaseFillAmount;
            collision.gameObject.GetComponent<EnemyControl>().hittingReverse();
            collision.gameObject.GetComponent<EnemyControl>().lazerHit.hiting = true;
            collision.gameObject.GetComponent<EnemyControl>().attackRun = true;
            bulletExplosion.Play();

        }
    }
}
