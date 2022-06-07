using System;
using UnityEngine;

namespace Scenes.Levels.example.movement
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private bool working=true;
        [SerializeField] private bool rotating;
        //[SerializeField] private bool negativeTurn;// not working
        [SerializeField] private Vector3 desiredReachTimes=new Vector3(1,1,1);
        [SerializeField] private Vector3 desiredAngles=new Vector3(0,0,0);
        [SerializeField] private bool turnX;
        [SerializeField] private bool turnY;
        [SerializeField] private bool turnZ;
     
        [SerializeField] private float xSpeed;
        [SerializeField] private float ySpeed;
        [SerializeField] private float zSpeed;
 
        public float playersXPosition;
 
        void Start()
        {
            findSpeed();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (working)
            {
             
                if (rotating&&(turnX||turnY||turnZ))
                {
                    rotate();
                }
            
            }
        }


        private void rotate()
        {
            Quaternion currentRotation = transform.rotation; 
        
            if (turnX)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(desiredAngles.x, currentRotation.y, currentRotation.z), xSpeed * Time.deltaTime);
        
            if (turnY)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(currentRotation.x, desiredAngles.y, currentRotation.z), ySpeed * Time.deltaTime);
        
            if (turnZ)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(currentRotation.x, currentRotation.y, desiredAngles.z), zSpeed * Time.deltaTime);

            if (Math.Abs(currentRotation.x - desiredAngles.x) < 0.01f)
                turnX = false;
            if (Math.Abs(currentRotation.y - desiredAngles.y) < 0.01f)
                turnY = false;
            if (Math.Abs(currentRotation.z - desiredAngles.z) < 0.01f)
                turnZ = false;
                 
        }

        private void findSpeed()
        {

            desiredReachTimes.x = desiredReachTimes.x < 0.1f ? 0.1f : desiredReachTimes.x;
            desiredReachTimes.y = desiredReachTimes.y < 0.1f ? 0.1f : desiredReachTimes.y;
            desiredReachTimes.z = desiredReachTimes.z < 0.1f ? 0.1f : desiredReachTimes.z;

            Quaternion currentRotation = transform.rotation;

            xSpeed = (desiredAngles.x - currentRotation.x) / desiredReachTimes.x;
            ySpeed = (desiredAngles.y - currentRotation.y) / desiredReachTimes.y;
            zSpeed = (desiredAngles.z - currentRotation.z) / desiredReachTimes.z;



        }
 
        public void rotateOn()
        {
            rotating = true;
        }
    
        public void rotateOff()
        {
            rotating = false;
        }
    
    
    }
}
