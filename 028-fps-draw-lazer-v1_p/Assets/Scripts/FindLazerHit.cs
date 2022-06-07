using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;

public class FindLazerHit : MonoBehaviour
{
    [SerializeField] public bool Working=true;
    [SerializeField] private bool EditorActive=true;
    [SerializeField] private LayerMask layerMask;
    [Range(0, 1)] [SerializeField]
    private float IndicatorSphereSize=0.25f;

    [SerializeField] private Transform SourcePoint;
    
    [SerializeField] private bool PlaceIndicatorObj;
    [SerializeField] private Transform IndicatorObj;
    [SerializeField] private GameObject shotTargetParticle;
    [SerializeField] private Transform[] targets;
    [SerializeField] private GameObject cloud;
    
    public GameObject drawPanel;
    public GameObject drawPanel2;
    public GameObject drawPanel3;
    public GameObject drawPanel4;
    public GameObject lineObject;
    private PlayerControl playerControl;
    private BarrelExplosion barrelExplosion;
    private LineCreator lineCreator;
    private float resetIndicatorPoint =15f;
    private bool started=false;
    

    public bool hiting = false;
    public bool drawing;
    public bool useCloud;
    public int drawingScale;
    
    
    
    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        barrelExplosion = FindObjectOfType<BarrelExplosion>();
        lineCreator = FindObjectOfType<LineCreator>();

        started = false;
        
        IndicatorObj.position=new Vector3(resetIndicatorPoint,IndicatorObj.position.y,IndicatorObj.position.z+15f);
        SourcePoint.rotation=Quaternion.identity;
        
        
        EditorActive = false;
        if (IndicatorObj==null)
        {
            PlaceIndicatorObj = false;
        }

        if (SourcePoint==null)
        {
            SourcePoint = transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            started = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (started)
        {
            if (Working && !drawing)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(SourcePoint.position, SourcePoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(SourcePoint.position, SourcePoint.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
               // Debug.Log("Did Hit");
                if (PlaceIndicatorObj)
                {
                    IndicatorObj.transform.position = hit.point;
                }

                /*if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<EnemyControl>().hittingReverse();
                }
                if (hit.collider.CompareTag("EnemyParent"))
                {
                    hiting = true;

                }*/
                if (hit.collider.CompareTag("Draw"))
                {
                    foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("EnemyParent"))
                    {
                        if (VARIABLE.GetComponent<EnemyControl>().attackRun)
                        {
                            VARIABLE.GetComponent<EnemyControl>().hitting();
                        }
                        
                    }
                    playerControl.running = false;
                    drawPanel.gameObject.SetActive(true);
                    playerControl.splineFollower.followSpeed = 0;

                    Instantiate(shotTargetParticle, targets[0].position,Quaternion.identity);
                    IndicatorObj.position=new Vector3(resetIndicatorPoint,IndicatorObj.position.y,IndicatorObj.position.z);
                    SourcePoint.rotation=Quaternion.identity;
                    
                    lineObject.gameObject.SetActive(true);
                    drawing = true;
                    drawingScale = 1;
                    if (useCloud)
                    {
                        cloud.gameObject.SetActive(true);
                        cloud.gameObject.transform.DOScale(new Vector3(100f, 100f, 100f),1);  
                    }
                    
                    
                }
                if (hit.collider.CompareTag("Draw1"))
                {
                    foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("EnemyParent"))
                    {
                        if (VARIABLE.GetComponent<EnemyControl>().attackRun)
                        {
                            VARIABLE.GetComponent<EnemyControl>().hitting();
                        }
                        
                    }
                    playerControl.running = false;
                    playerControl.splineFollower.followSpeed = 0;
                    drawPanel2.gameObject.SetActive(true);
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                    lineObject.gameObject.SetActive(true);
                    drawing = true;
                    drawingScale = 2;
                    Instantiate(shotTargetParticle, targets[1].position,Quaternion.identity);
                }

                if (hit.collider.CompareTag("Draw2"))
                {
                    foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("EnemyParent"))
                    {
                        if (VARIABLE.GetComponent<EnemyControl>().attackRun)
                        {
                            VARIABLE.GetComponent<EnemyControl>().hitting();
                        }
                        
                    }
                    playerControl.running = false;
                    playerControl.splineFollower.followSpeed = 0;
                    drawPanel3.gameObject.SetActive(true);
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                    lineObject.gameObject.SetActive(true);
                    drawing = true;
                    drawingScale = 3;
                    Instantiate(shotTargetParticle, targets[2].position,Quaternion.identity);
                }

                if (hit.collider.CompareTag("Draw3"))
                {
                    foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("EnemyParent"))
                    {
                        if (VARIABLE.GetComponent<EnemyControl>().attackRun)
                        {
                            VARIABLE.GetComponent<EnemyControl>().hitting();
                        }
                        
                    }
                    playerControl.running = false;
                    playerControl.splineFollower.followSpeed = 0;
                    drawPanel4.gameObject.SetActive(true);
                    hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;
                    lineObject.gameObject.SetActive(true);
                    drawing = true;
                    drawingScale = 4;
                    Instantiate(shotTargetParticle, targets[3].position,Quaternion.identity);
                    
                }
                if (hit.collider.CompareTag("Barrel") && Input.GetMouseButton(0))
                {
                    barrelExplosion.fire = true;
                    barrelExplosion.PlayFXs();
                    hit.collider.gameObject.SetActive(false);
                    
                }
               
            }
             
           
        }
        }
     
        

    }
  
    private void OnDrawGizmos()
    {
        if (EditorActive)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(SourcePoint.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(SourcePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
               // Debug.Log("Did Hit");
            }
            Gizmos.DrawSphere(hit.point,IndicatorSphereSize);
          
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            
        }
      

    }

    
}
