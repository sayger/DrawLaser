using UnityEngine;

 
 
    public class position_DirectionBasedLimiter : MonoBehaviour
    {
        [SerializeField] private bool ItsAcceptable;
    
        [SerializeField] private Transform limiter;
        [SerializeField] private Transform subject;
        [SerializeField] private bool reverseIt;
        [SerializeField] private float offset;

        [SerializeField] private float angle;
    
    
    
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ValidateAcceptance();
        }

        public bool ValidateAcceptance()
        {
            bool result = true;

            var forward = limiter.forward;
            Vector3 limiterForward = reverseIt ? forward * -1 : forward;
        
            Vector3 subjectPos = subject.position;
            if (reverseIt)
            {
                subjectPos -= offset * (limiter.forward*-1) ;
            }
            else
            {
                subjectPos -= offset * limiter.forward ;
            }
        
            Vector3 directionHeading = subject.position - limiter.position;
            angle = Vector3.Angle(limiterForward, directionHeading.normalized);

            result = angle<=90;
            ItsAcceptable=result;
        
         
        
            return result;
        }
    }
 
