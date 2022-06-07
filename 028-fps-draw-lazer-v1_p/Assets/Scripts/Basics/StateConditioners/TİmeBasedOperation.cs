using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TİmeBasedOperation : MonoBehaviour
{
    [SerializeField] private float _currentPercentage;
    private float _maxPercentage;
    [SerializeField] private float MaxPercentage=100;
    private float _executionTime;
    public float executionTime1=1.2f;
    [SerializeField] private bool started;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (started)
        {
            _currentPercentage += Time.deltaTime * (MaxPercentage / executionTime1);
            if (_currentPercentage>=MaxPercentage)
            {
                _currentPercentage = MaxPercentage;
            }
        }
        
    }
    public float currentPercentage()
    {
        return _currentPercentage;
    }

    public void  startCounter(bool reset=false)
    {
        started = true;
        _currentPercentage = 0;
        if (reset)
        {
            executionTime1 = _executionTime;
            MaxPercentage = _maxPercentage;
        }
    }
    public void  startCounter(float time)
    {
        executionTime1 = time;
        startCounter();

    }
    public void  startCounter(float time,float maxPercentage)
    {
        MaxPercentage = maxPercentage;
        startCounter(time);

    }
}
