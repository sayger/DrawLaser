using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerControl : MonoBehaviour
{

    [SerializeField] private GameObject redHit;
    [SerializeField] private GameObject cup;
    [SerializeField] private GameObject cupShine;
    [SerializeField] private ParticleSystem[] confettiParticles;

    public SplineFollower splineFollower;
    private EnemyControl enemyControl;
    private FindLazerHit lazerHit;

    private float firstSpeed;
    private float speedIncreaser=0;
    private bool started=false;
    
    public int enemyCount;

    public bool running = true;

    public int doorCrash = 0;

    public bool doorCrashed;
    // Start is called before the first frame update
    void Awake()
    {
        lazerHit = FindObjectOfType<FindLazerHit>();
        splineFollower = GetComponent<SplineFollower>();
        enemyControl = FindObjectOfType<EnemyControl>();
        // splineFollower.follow = false;
        firstSpeed = splineFollower.followSpeed;
        started = false;
        splineFollower.followSpeed = 0f;
        
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            started = true;
        }

        if (started)
        {
            if (enemyCount==0 && !lazerHit.drawing && !doorCrashed )
                                               
            {
                running = true;
            
                if (speedIncreaser<=firstSpeed)
                {
                    speedIncreaser += 1f;
                    splineFollower.followSpeed = speedIncreaser;
                
                }
                else
                {
                    speedIncreaser = firstSpeed;
                }
            }
            splineFollower.follow = running;
        
            RedBallControl();
        }
        if (enemyCount<0)
        {
            enemyCount = 0;
        }
        
        
    }

    void RedBallControl()
    {
        if (Input.GetMouseButton(0))
        {
            redHit.gameObject.SetActive(true);
        }
        else
        {
            redHit.gameObject.SetActive(false);
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        
       // Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Target") )
        {
            running = false;
           // speedIncreaser = 0;
            splineFollower.followSpeed = 0;
            other.gameObject.GetComponent<KillValue>().DecreaseEnemyValue();
            enemyCount = other.gameObject.GetComponent<KillValue>().enemyValue;
            
            if (other.gameObject.GetComponent<DestroyOtherRoom>() !=null)
            {
                other.gameObject.GetComponent<DestroyOtherRoom>().destroy();
            }
            
            Destroy(other.gameObject,1);
        }

        if (other.gameObject.CompareTag("CupTarget"))
        {
           // running = false;
            cup.gameObject.SetActive(true);
            cupShine.gameObject.SetActive(true);
            foreach (var VARIABLE in confettiParticles)
            {
                VARIABLE.Play();
            }
            Invoke("LateNextPanel",2f);
        }
        if (other.gameObject.CompareTag("Door"))
        {
            splineFollower.followSpeed = 0; 
            doorCrash++;

            if (doorCrash==1)
            {
            StartCoroutine(TurnBack());    
            }
            
        }

    }

    IEnumerator TurnBack()
    {
        splineFollower.direction = Spline.Direction.Backward;
        yield return new WaitForSeconds(1f);
        splineFollower.direction = Spline.Direction.Forward;
        doorCrashed = true;
        running = false;
        
        
    }
    void LateNextPanel()
    {
        LevelSystem.Instance.DidYouNextLevelPanel = true;
    }
}
