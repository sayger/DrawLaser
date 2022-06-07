using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectoralLevelManager : MonoBehaviour
{
    public static SectoralLevelManager Instance; // ? // public static SectoralLevelManager Instance { get; private set; }

    #region Public Variables
    
    public bool allSectorsComplete;
    public bool[] sectorComplete;
    public bool endIsNow;

    [SerializeField] private bool useProcessCounter;
    
    
    public ProgressCounter _progressCounter;
    
    
    #endregion
    
    private void Awake()
    {

        if (Instance != null )
        {
            Debug.Log("EXTRA : "+this+"  SCRIPT DETECTED RELATED GAME OBJ DESTROYED");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allSectorsComplete&&!endIsNow)
        {
            //GameManager.Instance.levelComplete();
            endIsNow = true;
        }
    }

   
    public void _ShowLevelCompletedUI()
    {
        if (useProcessCounter) _progressCounter.ShowLevelCompletedUI();
       
        

    }
    public void _ShowLevelFailedUI()
    {

        if (useProcessCounter) _progressCounter.ShowLevelFailedUI();

    }
    
    public void ProgressCounterSetAllEnemyCount(int _numberOfAllEnemy)
    {
        if (useProcessCounter)  _progressCounter.setCollectibleCount(_numberOfAllEnemy);

    }
    public void oneEnemyDown()
    {

        if (useProcessCounter)  _progressCounter.UpdateLevelProgress();


    }

}
