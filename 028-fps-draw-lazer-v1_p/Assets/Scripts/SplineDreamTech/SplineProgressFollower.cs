using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SplineProgressFollower : MonoBehaviour
{
    [SerializeField] private SplineFollower _splineFollower;
    [SerializeField] private SplineProjector playerProgress;
    [SerializeField] private bool exactFollow;
    [SerializeField] private double currentPercent;
    [SerializeField] private float SplineLenght ;

    [SerializeField] private bool autoOffset;
    
    [SerializeField] private float offset;
    
    [SerializeField] private double targetPercent;
    [SerializeField] private float followSpeed;
    
    private float _followSpeed;
    [SerializeField] private double discardDistance;
    
    
    
    
    
    
    void Start()
    {
        SplineLenght =  playerProgress.spline.CalculateLength();
        if (autoOffset)
        {
            offset = (float)(_splineFollower.result.percent - playerProgress.result.percent) * SplineLenght;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateVariable();
        if (exactFollow)
        {
            _splineFollower.follow = false;
            _splineFollower.SetPercent(targetPercent);
        }
        else
        {
            _splineFollower.follow = true;

            if (_splineFollower.result.percent<targetPercent-(discardDistance/SplineLenght))
            {
                _splineFollower.direction = Spline.Direction.Forward;
                _splineFollower.followSpeed = _followSpeed;
            }
            else if (_splineFollower.result.percent>targetPercent/*+(discardDistance/SplineLenght)*/)
            {
                _splineFollower.direction = Spline.Direction.Backward;
                _splineFollower.followSpeed =  _followSpeed*2;
            }
            else
            {
                _splineFollower.follow = false;
            }
            
        }
        
    }

    private void updateVariable()
    {
        currentPercent = playerProgress.result.percent;
        targetPercent = currentPercent;
        targetPercent += offset / SplineLenght;
        if (targetPercent<currentPercent)
        {
            targetPercent = currentPercent;
        }
        if (targetPercent<currentPercent)
        {
            targetPercent = currentPercent;
        }

        _followSpeed = (float)(followSpeed * Mathf.Abs((float)(currentPercent - _splineFollower.result.percent) * 10));
        _followSpeed = Mathf.Clamp(_followSpeed, 0, followSpeed * 3);

    }
}
