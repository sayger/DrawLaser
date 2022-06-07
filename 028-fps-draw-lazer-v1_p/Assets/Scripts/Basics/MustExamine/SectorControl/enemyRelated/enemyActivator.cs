using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyActivator : MonoBehaviour
{
    [SerializeField] private bool work=true;
    [SerializeField] private bool triggerInvisible=true;
    [SerializeField] private int EnemyActivateSector ;
    [SerializeField] private List<string> activationTags=new List<string>();
    [SerializeField] private List<GameObject>  activationObjects=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        makeAdjustments();
    }
    private void makeAdjustments()
    {
        checkValidityOfVariables(); 
        if (triggerInvisible)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
    private void checkValidityOfVariables()
    {
        if( activationTags.Count==0&&activationObjects.Count==0)
        {
            work = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!work) return;
        GameObject otherObj = other.gameObject;
        if (checkTags(otherObj)||checkGameObjects(otherObj))
        {
             
            if (work)
            {
                work = false;
                enemyManager.Instance.activateTheSectorEnemys(EnemyActivateSector);
            }
            //  itsTriggered = true;
            //   Invoke(nameof(makeTriggeredFalse),2f);
        }


    }
    private bool checkTags(GameObject subject)
    {
        foreach (var expr in activationTags)
        {
            if (subject.CompareTag(expr))
            {
                return true;
            }
        }
        return false;
    }

    

    private bool checkGameObjects(GameObject subject)
    {
        foreach (var expr in activationObjects)
        {
            if (subject==expr)
            {
                return true;
            }
        }
        return false;



    }
}
