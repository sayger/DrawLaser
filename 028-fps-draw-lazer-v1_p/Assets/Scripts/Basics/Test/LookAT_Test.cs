using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAT_Test : MonoBehaviour
{
    
    [SerializeField] private bool lookAtBool;

    [SerializeField] private GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (lookAtBool)
        {
            this.transform.LookAt(new Vector3(cursor.transform.position.x, this.transform.position.y,cursor.transform.position.z));   
        }
    }
}
