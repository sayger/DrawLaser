using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class cameraFinnishZoom : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [SerializeField] private bool test;
    [SerializeField] private bool lookTest;
    [SerializeField] private float zoomTime=3;
    
    [SerializeField] private bool zoomLevelChanger;
    [SerializeField] private float newZoom=80;
    [SerializeField] private float baseZoomLevel;
    [SerializeField] private float tempZoom;
     public bool lookingTowards;
    [SerializeField] private GameObject lookingTarget;
    [SerializeField] private float rotationSpeed=5000;
    
    
    
    
    [SerializeField] private Camera theCamera;


   // [SerializeField] private RopeStrainCalculator ropeStrainCalculator;
    private bool zoomable = true;
    void Start()
    {
        theCamera = GetComponent<Camera>();
        //LevelEvents.Instance.JumpLimitationActions += changeZoomLevel ;
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
             
            changeZoomLevel(newZoom);
        }

        if (lookTest)
        {
            lookTest = false;
            turnToPlayer(player);

        }

        if (zoomLevelChanger)
        {
             
            assignZoomLevel(tempZoom);
        }

        if (lookingTowards)
        {
            var q = Quaternion.LookRotation(lookingTarget.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
        }

       /* if (ropeStrainCalculator.strain > 1f && ropeStrainCalculator.strain < 1.4f && zoomable )
        {
            changeZoomLevel(90);
            zoomable = false;
        }
        if (ropeStrainCalculator.strain > 1.4f && ropeStrainCalculator.strain < 2f && zoomable )
        {
            changeZoomLevel(80);
            zoomable = false;
        }

        if (ropeStrainCalculator.strain < 1f && zoomable)
        {
            changeZoomLevel(70);
            zoomable = false;
        }*/
    }

    private void assignZoomLevel(float value)
    {
        theCamera.fieldOfView = value;

    }

    public void turnToPlayer(GameObject target,float rotateSpeed)
    {
        lookingTowards = true;
        rotationSpeed = rotateSpeed;
        lookingTarget = target;


    }
    public void turnToPlayer(GameObject target )
    {
        turnToPlayer(target, rotationSpeed);


    }

    public void changeZoomLevel(float newZoomLevel)
    {
        
        baseZoomLevel = theCamera.fieldOfView;
        tempZoom = baseZoomLevel;
        
        zoomLevelChanger = true;
        
        DOTween.To(()=>tempZoom, x=>tempZoom=x, newZoomLevel, zoomTime).OnComplete(()=>
        {
            zoomLevelChanger = false;
            zoomable = true;
        });


    }

    public void changeZoomLevel()
    {
        changeZoomLevel(newZoom);
    }
    
    
    
    
    
    
    
    
    
    
    
}
