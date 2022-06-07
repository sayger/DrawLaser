
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineCreator : MonoBehaviour
{
    [SerializeField] private GameObject hoe;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject duck;
    [SerializeField] private GameObject TNT;
    [SerializeField] private GameObject littleDuck;
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject[] shurikens;
    [SerializeField] private GameObject explosionForce;
    [SerializeField] private ParticleSystem[] lightnings;
    [SerializeField] private Transform[] enemyTargetsForLightnings;
    
    
    public GameObject linePrefab;
    public GameObject tntArea;
    public ParticleSystem[] tntFXs;
    
    private List<GameObject> lines;
    private FindLazerHit lazerHit;
    private PlayerControl playerControl;
    
    private Line activeLine;

    private Vector3 firstPosition, distance, currentPosition;
    private Vector3 lazerPosAfterDraw;
    private float camSizeReferance;

    private GameObject line;

    public Transform lineTarget;
    
    public bool draw;
    public bool useLittleDuck;
    public bool useShuriken;
    public bool useLightning;

    public Camera renderCam;
    public Canvas canvas;
    private void Awake()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        lazerHit = FindObjectOfType<FindLazerHit>();
        lines = new List<GameObject>();
        lazerPosAfterDraw = new Vector3(0, 0, lazerHit.transform.position.z);
        camSizeReferance = renderCam.orthographicSize;
        float s = canvas.scaleFactor;
        renderCam.orthographicSize = camSizeReferance * (s / 1.452f);
    }


    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            line = Instantiate(linePrefab,lineTarget.position,Quaternion.identity);
            line.transform.parent = lineTarget;
           // line.transform.localScale = new Vector3(line.transform.localScale.x / 2, line.transform.localScale.y / 2,line.transform.localScale.z);
           // line.gameObject.SetActive(false);
            activeLine = line.GetComponent<Line>();
            draw=true;
            
                
            lines.Add(line);
            
                
        }
        if (activeLine != null) /*&&
            Input.mousePosition.x >(Screen.width/10.3f) && Input.mousePosition.x<(Screen.width/1.1f) &&
            Input.mousePosition.y> (Screen.height/13.8f) && Input.mousePosition.y <(Screen.height/3.3f) )*/
        {
            
            /*Vector3 screenPos = cam.WorldToScreenPoint(Input.mousePosition);
            float h = Screen.height;
            float w = Screen.width;
            float x = screenPos.x - (w / 2);
            float y = screenPos.y - (h / 2);
            
            //activeLine.UpdateLine(new Vector2(x, y) / s);
            activeLine.UpdateLine(-screenPos);*/
            
            //Debug.Log(renderCam.orthographicSize);
            
            //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition = (Input.mousePosition);
            //Debug.Log("x"+mousePosition.x);
            //Debug.Log("y"+mousePosition.y);
            activeLine.UpdateLine(mousePosition);
            lazerHit.Working = false;
            cursor.gameObject.SetActive(false);

        }

        if (Input.GetMouseButtonUp(0))
        {
            
            activeLine = null;
            cursor.gameObject.SetActive(true);
            if (draw)
            {
                
                
                if (lazerHit.drawingScale==1)
                {
                    if (useLightning)
                    {
                        explosionForce.gameObject.SetActive(true);
                        if (enemyTargetsForLightnings[0] != null)
                        {
                            lightnings[0].transform.position = new Vector3(enemyTargetsForLightnings[0].position.x,
                                lightnings[0].transform.position.y,
                                enemyTargetsForLightnings[0].position.z);
                        }

                        if (enemyTargetsForLightnings[1] !=null)
                        {
                            lightnings[1].transform.position = new Vector3(enemyTargetsForLightnings[1].position.x,
                                lightnings[1].transform.position.y,
                                enemyTargetsForLightnings[1].position.z);
                        }
                        
                        foreach (var VARIABLE in lightnings)
                        {
                            VARIABLE.Play();
                        }
                        
                    }
                    else
                    {
                        if (!useLittleDuck)
                        {
                            duck.gameObject.SetActive(true);
                        }
                        else
                        {
                            littleDuck.gameObject.SetActive(true);
                            useLittleDuck = false;
                        }    
                    }
                    
                      
                }
                if (lazerHit.drawingScale==2) 
                    
                {
                    key.gameObject.SetActive(true);  
                }

                if (lazerHit.drawingScale==3)
                {
                    if (useShuriken)
                    {
                        foreach (var VARIABLE in shurikens)
                        {
                            VARIABLE.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        hoe.gameObject.SetActive(true);
                    }
                    
                }

                if (lazerHit.drawingScale==4)
                {
                    TNT.gameObject.SetActive(false);
                    PlayFXs();
                    
                }
                
                lazerHit.Working = true;
                lazerHit.lineObject.gameObject.SetActive(false);
                lazerHit.drawPanel.gameObject.SetActive(false);
                lazerHit.drawPanel2.gameObject.SetActive(false);
                lazerHit.drawPanel3.gameObject.SetActive(false);
                lazerHit.drawPanel4.gameObject.SetActive(false);
                lazerHit.drawing = false;
                playerControl.splineFollower.followSpeed = 0;
                
               // lineTarget.gameObject.SetActive(false);
                playerControl.running = true;
                draw = false;
                drawFalser();
                Destroy(line);
                //lazerHit.transform.position = lazerPosAfterDraw;

            }

        }

    }

    void drawFalser()
    {
        if (!draw)
        {
            
            foreach (var VARIABLE in GameObject.FindGameObjectsWithTag("EnemyParent"))
            {
                if (VARIABLE.GetComponent<EnemyControl>().attackRun)
                {
                    VARIABLE.GetComponent<EnemyControl>().hittingReverse();
                }
                        
            }
        }
        
    }
    public void PlayFXs()
    {
       
            for (int i = 0; i < tntFXs.Length; i++)
            {
                tntFXs[i].Play();
                tntArea.gameObject.SetActive(true);
            }
          //  tntArea.gameObject.SetActive(false);
           
    }
   
}
