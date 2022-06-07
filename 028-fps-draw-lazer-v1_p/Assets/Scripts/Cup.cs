using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    Vector3 _startingPosition;
    [SerializeField] Vector3 _movementVector;
    float _movementFactor;
    [SerializeField] float _period = 2f;
    [SerializeField] private float rotationFactor = 0;

    void Start()
    {
        _startingPosition = transform.position;
    }

    void Update()
    {
        if (_period == 0f) 
        {
            return;
        }

        float cycles = Time.time / _period; 

        const float tau = Mathf.PI * 2; 
        float rawSinWave = Mathf.Sin(cycles * tau); 

        _movementFactor = (rawSinWave + 1f) / 2f; 

        Vector3 offset = _movementVector * _movementFactor;
        transform.position = _startingPosition + offset;
        transform.eulerAngles=new Vector3(transform.eulerAngles.x,transform.eulerAngles.y+rotationFactor,transform.eulerAngles.z);
    }
}
