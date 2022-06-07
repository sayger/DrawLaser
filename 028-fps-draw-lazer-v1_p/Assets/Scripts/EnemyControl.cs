using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Image Bar;
    [SerializeField] private float fillingAmount = 0.1f;
    [SerializeField] private float emptyFillingAmount = 0.1f;
    [SerializeField] private float hitForce;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private int materialIndex;
    
    public float fill;
    public FindLazerHit lazerHit;
    public int waitPointIndex=0;
    private int temp;
    private Animator animator;
    public NavMeshAgent agent;
    private Rigidbody rigidbody;
    private PlayerControl playerControl;
    private SkinnedMeshRenderer meshRenderer;
    public Material[] materials;

    private float firstSpeed;
    private bool isDead = false;
    public bool attackRun;
    private bool healthBarFilling;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        lazerHit = FindObjectOfType<FindLazerHit>();
        rigidbody = GetComponent<Rigidbody>();
        playerControl = FindObjectOfType<PlayerControl>();

        attackRun = false;
        fill = 1f;
        healthBarFilling=true;
        firstSpeed = agent.speed;

    }

    // Update is called once per frame
    void Update()
    {
        if (attackRun)
        {
            agent.SetDestination(player.transform.position);
          
              
        }
        
        HealthBar();
    }

    
    
    public void RunningAnimation(bool isRunning)
    {
        if (isRunning == animator.GetBool("isRunning")) return;

        animator.SetBool("isRunning", isRunning);
    }
    public void AttackingAnimation(bool isAttacking)
    {
        if (isAttacking == animator.GetBool("isAttacking")) return;

        animator.SetBool("isAttacking", isAttacking);
    }

    public void LaughingAnimation(bool isLaughing)
    {
        if (isLaughing == animator.GetBool("isLaughing")) return;

        animator.SetBool("isLaughing", isLaughing);
    }


    void HealthBar()
    {
        if (!isDead)
        {
            if (!lazerHit.hiting || !Input.GetMouseButton(0) && healthBarFilling)
            {
                fill += Time.deltaTime * fillingAmount;
            }
            else if(lazerHit.hiting && !healthBarFilling )
            {
                fill -= Time.deltaTime * emptyFillingAmount;
            }
        
            fill = Mathf.Clamp(fill, 0f, 1f); 
            Bar.fillAmount = fill;
            if (fill<=0.1)
            {
                isDead = true;
                animator.enabled = false;
                rigidbody.AddForce(Vector3.back*Time.deltaTime*hitForce);
                rigidbody.isKinematic = true;
                agent.speed = 0;
                healthBar.gameObject.SetActive(false);
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
                meshRenderer.material = materials[materialIndex];
                playerControl.enemyCount--;
                hitting();
                Destroy(this.gameObject,3f);

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lazerHit.hiting = false;
        }

        
    }

    public void hitting()
    {
            RunningAnimation(false);
            agent.speed = 0;
        
    } 
    public void hittingReverse()
    {
            LaughingAnimation(false);
            RunningAnimation(true);
            agent.speed = firstSpeed;
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitPoint"))
        {
            LaughingAnimation(false);
            healthBarFilling = false;
            attackRun = true;
            RunningAnimation(true);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
           
            RunningAnimation(false);
            AttackingAnimation(true);
           // attackRun = false;
            
        }
        
    }


    private void OnCollisionExit(Collision other)
    {
        healthBarFilling = true;
    }
}
