using UnityEngine;

namespace Scenes.Levels.example.tracking
{
    public class TrackRotation : MonoBehaviour
    {
        [SerializeField] private bool work=true;
        private  GameObject objectA;
         
        [SerializeField] private  GameObject objectB;
         
        [SerializeField] private bool willTrack_elseCarry_ =true;
        
        
        [SerializeField] private bool mimicTargetRotation = false;
        [SerializeField] private bool faceTheTarget = true;
        [SerializeField] private bool tiltToGivenRotation = false;
        [SerializeField] private float tiltAngle=60f;
        
        public Vector3 targetFixedRotation;
        private Vector3 _targetFixedRotation;
        
        
        [SerializeField] private string acceptMovementInDimensions="xyz";
        [SerializeField] private float facingSpeed = 20;

       
        
        
        
        
        private bool faceX;
        private bool faceY;
        private bool faceZ;
        
        private bool _tiltToGivenRotation = false;
        private bool _faceTheTarget = false;
        private bool _mimicTargetRotation = false;
        
        void Start()
        {
            
            adjustments();

            exceptAxes(acceptMovementInDimensions);

        }

        
        void FixedUpdate()
        {
           // targetFixedRotation = objectB.transform.rotation.eulerAngles;
          //  _targetFixedRotation = targetFixedRotation;
           
            
            resolveContrast();

            facingTarget(_faceTheTarget);
            mimicTarget(_mimicTargetRotation);
            tiltToMatchRotations(_tiltToGivenRotation);


        }


        private void resolveContrast()
        {

            if (_mimicTargetRotation!=mimicTargetRotation)
            {
                if (mimicTargetRotation)
                {
                    faceTheTarget = false;
                    _faceTheTarget = false;
                    tiltToGivenRotation = false;
                    _tiltToGivenRotation = false;

                }
                _mimicTargetRotation = mimicTargetRotation;
            }
            if (_faceTheTarget!=faceTheTarget)
            {
                if (faceTheTarget)
                {
                    mimicTargetRotation = false;
                    _mimicTargetRotation = false;
                    tiltToGivenRotation = false;
                    _tiltToGivenRotation = false;
                    
                }
                _faceTheTarget = faceTheTarget;
            }

            if (_tiltToGivenRotation!=tiltToGivenRotation)
            {
                if (tiltToGivenRotation)
                {
                    mimicTargetRotation = false;
                    _mimicTargetRotation = false;
                    faceTheTarget = false;
                    _faceTheTarget = false;
                }

                _tiltToGivenRotation = tiltToGivenRotation;
            }


        }

        private void exceptAxes(string input)
        {
            bool x = false, y = false, z = false;
            

            char[] charList = input.ToCharArray();
            foreach (var e in charList)
            {
                switch (e)
                {
                    case 'x':
                    case 'X':
                        x = true;
                        break;
                    case 'y':
                    case 'Y':
                        y = true;
                        break;
                    case 'z':
                    case 'Z':
                        z = true;
                        break;
                }
            }

            faceX = x;
            faceY = y;
            faceZ = z;

        }

        private void adjustments()
        {
            if (objectA == null)
                objectA = this.gameObject; 
            
            if (!willTrack_elseCarry_ )
            {
                objectA = objectB;
                objectB=this.gameObject;
            }
            
            //---------------------------------------
            
            if (mimicTargetRotation)
            {
                _mimicTargetRotation = true;
                faceTheTarget = false;
            }
                
            _faceTheTarget = faceTheTarget;


            targetFixedRotation = objectA.transform.rotation.eulerAngles;

        }

        private void mimicTarget(bool doOrNot)
        {

            if (doOrNot)
            {
                float speed = facingSpeed;
                
               

                objectA.transform.rotation = Quaternion.Slerp(objectA.transform.rotation, objectB.transform.rotation, Time.deltaTime * speed);  ;
            }
                


        }

        private void facingTarget(bool doOrNot)
        {

            if (doOrNot)
            {
                float speed = facingSpeed;
                Vector3 from = objectA.transform.position;
                Vector3 to = objectB.transform.position;

                if (!faceX)
                    to=new Vector3(from.x,to.y,to.z);
                if (!faceY)
                    to=new Vector3(to.x,from.y,to.z);
                if (!faceZ)
                    to=new Vector3(to.x,to.y,from.z);

                Quaternion lookRotation = Quaternion.LookRotation((to - from).normalized);

          

                objectA.transform.rotation = Quaternion.Slerp(objectA.transform.rotation, lookRotation, Time.deltaTime * speed);
            }
        
        
        }

        private void tiltToMatchRotations(bool doOrNot)
        {
            targetFixedRotation = objectB.transform.rotation.eulerAngles;
            _targetFixedRotation = targetFixedRotation;
            
            float smooth = facingSpeed;
            //float tiltAngle = 60.0f;
            
            // Smoothly tilts a transform towards a target rotation.
            float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
            float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

            // Rotate the cube by converting the angles into a quaternion.
            Quaternion target = Quaternion.Euler(tiltAroundX+_targetFixedRotation.x, _targetFixedRotation.y, tiltAroundZ+_targetFixedRotation.z);

            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
        }

    }
    
   



}
