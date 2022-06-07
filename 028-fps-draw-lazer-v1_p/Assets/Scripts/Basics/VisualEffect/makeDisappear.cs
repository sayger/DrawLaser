using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeDisappear : MonoBehaviour
{
    [SerializeField] private bool ForTheChild;
    
    [SerializeField] private bool makeInvisible=true;
    
    void Start()
    {
        makeAdjustments();
    }
    private void makeAdjustments()
    {
        
        if (makeInvisible)
        {

            MakeInvisible();

        }

    }
     
    void Update()
    {
        
    }

    public void MakeInvisible()
    {
        if (ForTheChild)
        {
            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (var VARIABLE in childs)
            {
                Renderer temp = VARIABLE.GetComponent<Renderer>();
                if (temp!=null)
                {
                    temp.enabled = false;
                }
            }
        }
        else
        {
            Renderer temp = transform.GetComponent<Renderer>();
            if (temp!=null)
            {
                temp.enabled = false;
            }
                  
        }
    }
    public void MakeVisible()
    {
        if (ForTheChild)
        {
            Transform[] childs = GetComponentsInChildren<Transform>();
            foreach (var VARIABLE in childs)
            {
                Renderer temp = VARIABLE.GetComponent<Renderer>();
                if (temp!=null)
                {
                    temp.enabled = true;
                }
            }
        }
        else
        {
            Renderer temp = transform.GetComponent<Renderer>();
            if (temp!=null)
            {
                temp.enabled = true;
            }
                  
        }
    }
}
