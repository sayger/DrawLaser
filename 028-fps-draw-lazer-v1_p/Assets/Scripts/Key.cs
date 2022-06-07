using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Key : MonoBehaviour
{
    [SerializeField] private ParticleSystem stars;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float growingScale;
    [SerializeField] private float moveScale;
    [SerializeField] GameObject doorLock;
    [SerializeField] private GameObject doorChain;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;

    private PlayerControl playerControl;

    private void Awake()
    {
        playerControl = FindObjectOfType<PlayerControl>();
    }

    private void OnEnable()
    {
        Vector3 firstScale = transform.localScale;
        Vector3 firstPosition = transform.position;
        Vector3 toGrow = new Vector3(transform.localScale.x * growingScale, transform.localScale.y * growingScale, transform.localScale.z* growingScale);
        stars.Play();
        transform.DOScale(toGrow, moveDuration);
        doorRight.gameObject.GetComponent<MeshCollider>().enabled = false;
        doorLeft.gameObject.GetComponent<MeshCollider>().enabled = false;
        
        transform.DOMove(doorLock.transform.position, moveDuration).SetEase(moveEase)
            .OnComplete(() =>
            {
                doorChain.gameObject.SetActive(false);
                doorLock.gameObject.SetActive(false);
                doorLeft.transform.DOMoveX(doorLeft.transform.position.x - 10, 0.15f);
                doorRight.transform.DOMoveX(doorRight.transform.position.x + 10, 0.15f);
                this.gameObject.SetActive(false);
                playerControl.running = true;
                playerControl.doorCrashed = false;

            });
       
    }

    
}
