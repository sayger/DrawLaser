using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorActivator : MonoBehaviour
{
    public GameObject cursor;
    void Start()
    {
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            cursor.SetActive(true);
        }
    }
}
