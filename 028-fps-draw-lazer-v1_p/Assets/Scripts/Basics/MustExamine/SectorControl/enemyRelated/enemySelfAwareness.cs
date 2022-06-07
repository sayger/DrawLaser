using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySelfAwareness  : MonoBehaviour
{
    [SerializeField] private bool itsDead;
    
    [SerializeField] private bool dewIt;
    
    [SerializeField] private int belongToSector=1;
    private bool externalControl;

    private bool itsFirstTime = true;
   
    public enemySelfAwareness( )
    {
         
    }

    private void Update()
    {
        if (dewIt)
        {
            dewIt = false;
            removeFromSector();
        }
    }

    public enemySelfAwareness(int sector)
    {
        belongToSector = sector;
    }

    private void Awake()
    {
      //  if(!externalControl)
      //      addToSector();
    }

    void Start()
    {
        if(!externalControl)
            addToSector();
    }

     
    public void addToSector()
    {
//         Debug.Log("ENEMY ADDED");
        enemyManager.Instance.addMeToTheSectorEnemy(belongToSector);
        enemyManager.Instance.addMeToTheSectorEnemyActivator(belongToSector,gameObject);
        
        itsFirstTime = false;
    }
    public void removeFromSector()
    {
        Debug.Log("REMOVE ME FROM SECTOR");
//        enemyManager.Instance.removeMeFromTheSector(belongToSector);
    }

    public void setSector(int sector)
    {
         if (!itsFirstTime)
         {
             removeFromSector();
             
         }
         
         belongToSector = sector;
         addToSector();
         externalControl = true;
    }

    public void killIt()
    {
        if (!itsDead)
        {
            itsDead = true;
            removeFromSector();
        }
        

    }
}
