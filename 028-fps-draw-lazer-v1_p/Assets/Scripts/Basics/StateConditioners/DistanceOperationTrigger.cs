using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceOperationTrigger : MonoBehaviour
{
    [SerializeField] private DistanceBasedOperation _operation;
    [Range(0, 100)]
    [SerializeField] private float TriggerTime=100;

    [SerializeField] private bool BeenTriggered;

    [SerializeField] private bool destroyAfter;
    
    void Start()
    {
         
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(_operation.currentPercentage() -TriggerTime)<0.01f)
        {
            TriggerActivate();
        }
    }

    private void TriggerActivate()
    {
        if (BeenTriggered)
        {
            return;
        }
        BeenTriggered = true;
        if (destroyAfter)
        {
            Invoke($"destroySelf",4);
        }

        deleteAfterEachGame();

    }

    private void destroySelf()
    {
        Destroy(this);
    }

    private void deleteAfterEachGame()
    {
        //TODO ? events maybe
    }
}
