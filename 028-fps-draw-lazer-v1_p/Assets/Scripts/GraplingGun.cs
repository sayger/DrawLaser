using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class GraplingGun : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] Transform lazerBeganPoint;
    [SerializeField] private Transform redTarget;
    [SerializeField] private float lazerSpawnTime;
    [SerializeField] private GameObject lazer;
    public Transform hitpoint;
    private float currentSpawnTime;
    private float timeBoundary;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        ResetTimes();
    }

    private FindLazerHit lazerHit;

    private void Start()
    {
        lazerHit = FindObjectOfType<FindLazerHit>();
    }


    private void Update()
    {

        
            if (Input.GetMouseButton(0) && !lazerHit.drawing)
            {
                StartGrapple();
                
                currentSpawnTime += Time.deltaTime;
                if (currentSpawnTime > timeBoundary)
                {
                    Spawn();
                    ResetTimes();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopGrapple();
                ResetTimes();
            }
        
        
    }

    

    void StartGrapple()
    {
        _lineRenderer.positionCount = 2;
        DrawLazer();
    }

    void StopGrapple()
    {
        _lineRenderer.positionCount = 0;
    }

    void DrawLazer()
    {
        _lineRenderer.SetPosition(0,lazerBeganPoint.position);
        _lineRenderer.SetPosition(1,redTarget.position);
    }

    void ResetTimes()
    {
        currentSpawnTime = 0f;
        timeBoundary = lazerSpawnTime;
        
    }

    void Spawn()
    {
     GameObject templazer=  Instantiate(lazer, lazerBeganPoint);
     templazer.GetComponent<LazerMove>().hitPoint = hitpoint;
     templazer.transform.parent = null;
     // templazer.GetComponent<LazerMove>().goEnemy();
    }
}
