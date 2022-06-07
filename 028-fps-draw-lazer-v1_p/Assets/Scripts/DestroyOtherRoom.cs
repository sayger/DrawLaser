using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherRoom : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

  

    public void destroy()
    {
        
        foreach (var VARIABLE in enemies)
        {
            Destroy(VARIABLE);
        }
        
    }
}
