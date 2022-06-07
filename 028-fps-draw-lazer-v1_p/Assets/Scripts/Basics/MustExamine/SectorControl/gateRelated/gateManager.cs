using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateManager : MonoBehaviour
{
  //  public   int[] enemySectors0 ;
 //   public   int[] enemySectorsNumbers ;

 //public bool crashPreventive;
 
  
 
   // public   List<int[]> enemySectors=new List<int[]>();
    public IDictionary<int, List<GameObject>> sectorDoors = new Dictionary<int, List<GameObject>>();
    public IDictionary<int, List<GameObject>> sectorEnemys = new Dictionary<int, List<GameObject>>();
   // public List<int> sectorNumbersInOrder = new List<int>();
    
    
   // private bool firstTime = true;
    //  public int numberOfEnemySectors;
    
    private static gateManager _instance;
    public static gateManager Instance => _instance;
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

    private void LateUpdate()
    {
        //numberOfEnemySectors = enemySectors.Count;
      /*  if (firstTime)
        {
            installSectors();
            firstTime = false;
        }*/
        
    }

    private void debug()
    {/*
        enemySectors0 = new int[enemySectors.Count];
        enemySectorsNumbers= new int[enemySectors.Count];
        for (int i = 0; i < enemySectors.Count; i++)
        {
            enemySectors0[i] = enemySectors[i][0];
            enemySectorsNumbers[i] = enemySectors[i][1];
        }
*/

    }

  /*  private void installSectors() // delete before moving this to plain project
    {
        
        
        int numberOfSectors = enemySectors.Count;
        bool [] result = new bool [numberOfSectors];
      //  GameManager.Instance.sectorComplete = result;                 //*----------------*************************************************************--------------------***********
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
        
        GameManager.Instance.ProgressCounterSetAllEnemyCount(numberOfAllEnemy);

        if (crashPreventive)
        {
            deActivateAllEnemys();
        }

    }*/
//--------------------------------------------------------!!!!!--------------------!!!!------------------
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
   /* public void activateTheSectorEnemys(int sectorNumber)
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
    }*/
  /*  public void deactivateTheSectorEnemys(int sectorNumber)
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
    }*/

   /* private void deActivateAllEnemys()
    {

        foreach (var VARIABLE in enemySectors)
        {
            if (VARIABLE[0]!=1)//ERROR POTENTIAL FIRST 1 SENSITIVE
            {
                deactivateTheSectorEnemys(VARIABLE[0]);
            }
            
        }


    }*/

   

    //--------------------------------------------------------!!!!!--------------------!!!!------------------
    
    public void addMeToTheSectorDoor(int sectorNumber,GameObject theDoor)
    {
        if (sectorDoors.ContainsKey(sectorNumber))
        {
            foreach (var VARIABLE in sectorDoors)
            {
                if (VARIABLE.Key==sectorNumber)
                {
                    VARIABLE.Value.Add(theDoor);
                    break;
                }
            }
        }
        else// Anew
        {
            sectorDoors.Add(sectorNumber,new List<GameObject>());
            
                foreach (var VARIABLE in sectorDoors)
                {
                    if (VARIABLE.Key==sectorNumber)
                    {
                        VARIABLE.Value.Add(theDoor);
                        break;
                    }
                }
             
        }
        

    }

    
   /* public void addMeToTheSectorEnemy(int sectorNumber)
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
            
        }
        else
        {
             
             
            enemySectors[findedAdress][1]++;
            
             
        }


    }*/

   /* public void removeMeFromTheSector(int sectorNumber)
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
            openTheSectorDoor(sectorNumber);
            sectorComplete(sectorNumber); //TO DO DELETE LATER
        }

        GameManager.Instance.oneEnemyDown();
         
    }*/

   /* private void sectorComplete(int sectorNumber)//TO DO DELETE LATER
    
    {
        for (int i = 0; i < sectorNumbersInOrder.Count; i++)
        {
            if (sectorNumbersInOrder[i]==sectorNumber)
            {
                GameManager.Instance.sectorComplete[i] = true;   
                break;
            }
        }

        bool allCompleated = true;
         foreach (var VARIABLE in GameManager.Instance.sectorComplete)   
        {
            if (!VARIABLE)
            {
                allCompleated = false;
                break;
            }
        } 

        if (allCompleated)
        {
             GameManager.Instance.allSectorsComplete = true;                     
            //maybe some finishing method in game manager can be called here
        }



    }*/

    public void openTheSectorDoor(int sectorNumber)
    {

        foreach (var VARIABLE in sectorDoors)
        {
            if (VARIABLE.Key==sectorNumber)
            {
                foreach (var e in VARIABLE.Value)
                {
                    e.GetComponent<doorGuard>().openIt();
                    
                }
                break;
            }
        }
    }
    public void closeTheSectorDoor(int sectorNumber)
    {

        foreach (var VARIABLE in sectorDoors)
        {
            if (VARIABLE.Key==sectorNumber)
            {
                foreach (var e in VARIABLE.Value)
                {
                    e.GetComponent<doorGuard>().closeIt();
                    
                }
                break;
            }
        }
    }
    public void activateTheSectorDoor(int sectorNumber)
    {

        foreach (var VARIABLE in sectorDoors)
        {
            if (VARIABLE.Key==sectorNumber)
            {
                foreach (var e in VARIABLE.Value)
                {
                    e.GetComponent<doorGuard>().activateIt();
                    
                }
                break;
            }
        }
    }
    
    
}
