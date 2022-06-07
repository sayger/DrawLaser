using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeToMiddle : MonoBehaviour //TODO CHANGES BEEN MADE
{
    [SerializeField] private bool work;

    public Transform objA;
    public Transform objB;

    [SerializeField] private bool useX;
    [SerializeField] private bool useY;
    [SerializeField] private bool useZ;

    [Range(0.1f, 100)]
    public float PositionBetween=50;

    public bool fixedOffsetToOneOfThem;
    public bool AIsOffsetElseB;
    [Range(-50, 50)]
    public float fixedOffset;

    private bool UsePositionLimiter;
    private PositionAdjusterLimiter positionAdjusterLimiter;

    public bool UseTransitionSpeed;
    public bool useLerpElseMoveTowards;
    public float transitionSpeed;


    private FindLazerHit lazerHit;
    
    void Start()
    {
        lazerHit = FindObjectOfType<FindLazerHit>();
        
        positionAdjusterLimiter = GetComponent<PositionAdjusterLimiter>();
        if (positionAdjusterLimiter!=null)
        {
            UsePositionLimiter = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        work = !lazerHit.drawing;
        
        if (work)
        {
            Vector3 executePos = new Vector3();
            var positionA = objA.position;
            Vector3 bPos = objB.position;
            if (!useX)
            {
                bPos.x = positionA.x;
            }
            if (!useY)
            {
                bPos.y = positionA.y;
            }
            if (!useZ)
            {
                bPos.z = positionA.z;
            }

            if (fixedOffsetToOneOfThem)
            {
                float originalDistance = Vector3.Distance(bPos, positionA);
                float requiredOffsetPercent = fixedOffset / originalDistance;
                Vector3 basePos = AIsOffsetElseB ? positionA : bPos;
                
                executePos = basePos  +(requiredOffsetPercent* (bPos  - positionA )  );

            }
            else
            {
                executePos  = positionA  +(PositionBetween* (bPos  - positionA ) / 100);
            }

            if (UseTransitionSpeed)
            {
                if (useLerpElseMoveTowards)
                {
                    executePos = Vector3.Lerp(transform.position, executePos, transitionSpeed * Time.deltaTime);
                }
                else
                {
                    executePos = Vector3.MoveTowards(transform.position, executePos, transitionSpeed * Time.deltaTime);
                }
            }

            if (UsePositionLimiter)
            {
                executePos = positionAdjusterLimiter.LimitThePos(executePos,transform);
            }
            transform.position  = executePos;
            
        }
    }
}
