using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillValue : MonoBehaviour
{
   public int enemyValue;
   [SerializeField] private List<GameObject> enemies = new List<GameObject>();

   

  public void DecreaseEnemyValue()
   {
      foreach (var VARIABLE in enemies)
      {
        
         if (VARIABLE.GetComponent<EnemyControl>().fill <0.01f)
         {
            enemyValue--;
            
         }
      }
   }
}
