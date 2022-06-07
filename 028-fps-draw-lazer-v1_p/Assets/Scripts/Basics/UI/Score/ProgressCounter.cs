using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ProgressCounter : MonoBehaviour
{
    #region Singleton class: UIManager

    public static ProgressCounter Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion
    public  int collectibleCount;
     
    public float progress = 0f;
    [Header("Level Progress UI")] 
    //[SerializeField] int sceneOffset =1;

    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private Image progressFillImage;
    [SerializeField] private TMP_Text levelCompletedText;
    [SerializeField] private TMP_Text levelFailedText;
    public bool oneTime = true;
    public int levelIdentity;

    [SerializeField] private float levelFailUITime=1;
    [SerializeField] private float levelCompleteUITime=1;
    
    
    void Start()
    {
        levelIdentity = SceneManager.GetActiveScene().buildIndex;
        progressFillImage.fillAmount = 0f;
         
        SetLevelProgressText();
        oneTime = true;
    }

    public void setCollectibleCount(int enemyCount)
    {

        collectibleCount = enemyCount;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetLevelProgressText()
    {
        int level = levelIdentity + ( PlayerPrefs.GetInt("fakeLevels")  );
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }
    
    public void UpdateLevelProgress()
    {
        progress += 1f / collectibleCount;
        progressFillImage.DOFillAmount(progress,.4f);

    }
    public void ShowLevelCompletedUI()
    {
        
        Invoke(nameof(_ShowLevelCompletedUI),levelCompleteUITime);
    }

    private void _ShowLevelCompletedUI()
    {
        levelCompletedText.DOFade(100f, .6f).From(0f);
    }

    public void ShowLevelFailedUI()
    {
        Invoke(nameof(_ShowLevelFailedUI),levelFailUITime);
         
    }
    public void _ShowLevelFailedUI()
    {
        levelFailedText.DOFade(100f, .6f).From(0f);
         
    }
}
