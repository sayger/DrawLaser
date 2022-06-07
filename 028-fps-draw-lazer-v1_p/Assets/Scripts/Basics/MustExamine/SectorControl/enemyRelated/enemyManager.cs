using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    public bool crashPreventive;
    
    public   List<int[]> enemySectors=new List<int[]>();
    public IDictionary<int, List<GameObject>> sectorEnemys = new Dictionary<int, List<GameObject>>();
    public List<int> sectorNumbersInOrder = new List<int>();



    private static enemyManager _instance;
    public static enemyManager Instance => _instance;
    private bool firstTime = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        
    }

    private void LateUpdate()
    {
         
        if (firstTime)
        {
            installSectors();
            firstTime = false;
        }
        
    } // wont start would do the same ???????????????????????????????
    private void installSectors() // delete before moving this to plain project
    {
        int numberOfSectors = enemySectors.Count;
        bool [] result = new bool [numberOfSectors];
        SectoralLevelManager.Instance.sectorComplete = result;                 //*----------------*************************************************************--------------------***********
        foreach (var VARIABLE in enemySectors)
        {
            sectorNumbersInOrder.Add(VARIABLE[0]);
        }
        sectorNumbersInOrder.Sort();

        int numberOfAllEnemy = 0;

        foreach (var VARIABLE in enemySectors)
        {
            numberOfAllEnemy += VARIABLE[1];
        }
        
        SectoralLevelManager.Instance.ProgressCounterSetAllEnemyCount(numberOfAllEnemy);

        if (crashPreventive)
        {
            deActivateAllEnemys();
        }

    }
    public void deactivateTheSectorEnemys(int sectorNumber)
    {

        foreach (var VARIABLE in sectorEnemys)
        {
            if (VARIABLE.Key==sectorNumber)
            {
                foreach (var e in VARIABLE.Value)
                {
                    //  e.transform.GetChild (2).gameObject.SetActive(false);
                    e.gameObject.SetActive(false);
                    
                }
                break;
            }
        }
    }               
    private void deActivateAllEnemys()
    {

        foreach (var VARIABLE in enemySectors)
        {
            if (VARIABLE[0]!=1)//ERROR POTENTIAL FIRST 1 SENSITIVE
            {
                deactivateTheSectorEnemys(VARIABLE[0]);
            }
            
        }


    }
    public void addMeToTheSectorEnemyActivator(int sectorNumber,GameObject theEnemy)
    {
        if (sectorEnemys.ContainsKey(sectorNumber))
        {
            foreach (var VARIABLE in sectorEnemys)
            {
                if (VARIABLE.Key==sectorNumber)
                {
                    VARIABLE.Value.Add(theEnemy);
                    break;
                }
            }
        }
        else// Anew
        {
            sectorEnemys.Add(sectorNumber,new List<GameObject>());
            
            foreach (var VARIABLE in sectorEnemys)
            {
                if (VARIABLE.Key==sectorNumber)
                {
                    VARIABLE.Value.Add(theEnemy);
                    break;
                }
            }
             
        }
    }
    
    public void activateTheSectorEnemys(int sectorNumber)
    {

        foreach (var VARIABLE in sectorEnemys)
        {
            if (VARIABLE.Key==sectorNumber)
            {
                foreach (var e in VARIABLE.Value)
                {
                    //  Debug.Log(" CHILD OBJECT NUMBER 0  NAME : "+e.transform. GetChild (0));
                    //  Debug.Log(" CHILD OBJECT NUMBER 1  NAME : "+e.transform. GetChild (1));
                    //   Debug.Log(" CHILD OBJECT NUMBER 2  NAME : "+e.transform. GetChild (2));
                    e.gameObject.SetActive(true);
                    //  e.transform.GetChild(2).gameObject.SetActive(true);
                    
                }
                break;
            }
        }
    }
    
    public void addMeToTheSectorEnemy(int sectorNumber)
    {
        
         
        bool itsAnEW = true;
        int findedAdress = 0;
        for (var index = 0; index < enemySectors.Count; index++)
        {
            var VARIABLE = enemySectors[index];
            if (VARIABLE[0] == sectorNumber)
            {
                itsAnEW = false;
                findedAdress = index;
                break;
            }
        }

        if (itsAnEW)
        {
             
            int[] newSector = {sectorNumber,1 };
            enemySectors.Add(newSector);
            Debug.Log("ENEMY ADDED TO A NEW SECTOR   enemySectors.Count  :"+enemySectors.Count);
            
        }
        else
        {
            enemySectors[findedAdress][1]++;
            Debug.Log("ENEMY ADDED TO A EXISTING SECTOR   enemySectors.Count  :"+enemySectors.Count);
            Debug.Log(findedAdress+"  SECTORS POPULATION  :"+enemySectors[findedAdress][1]);
        }


    }
    
    public void removeMeFromTheSector(int sectorNumber)
    {
        Debug.Log("REMOVE HAS BEEN CALLED");
        
        if (enemySectors.Count==0) return;
        
        int properIndex = 0;
        for (var index = 0; index < enemySectors.Count; index++)
        {
            var VARIABLE = enemySectors[index];
            if (VARIABLE[0] == sectorNumber)
            {
                properIndex = index;
            }
        }

        enemySectors[properIndex][1]--;
        if (enemySectors[properIndex][1]==0)
        {
            gateManager.Instance.openTheSectorDoor(sectorNumber);
            sectorComplete(sectorNumber); //TO DO DELETE LATER
        }

        SectoralLevelManager.Instance.oneEnemyDown();
         
    }
    private void sectorComplete(int sectorNumber)//TO DO DELETE LATER
    {
        for (int i = 0; i < sectorNumbersInOrder.Count; i++)
        {
            if (sectorNumbersInOrder[i]==sectorNumber)
            {
                SectoralLevelManager.Instance.sectorComplete[i] = true; 
                break;
            }
        }

        bool allCompleated = true;
        
        foreach (var VARIABLE in SectoralLevelManager.Instance.sectorComplete)  
        {
            if (!VARIABLE)
            {
                allCompleated = false;
                break;
            }
        } 

        if (allCompleated)
        {
            SectoralLevelManager.Instance.allSectorsComplete = true;              
            //maybe some finishing method in game manager can be called here
        }



    }
}
