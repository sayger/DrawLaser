using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BETAFacingTheTarget : MonoBehaviour  // it makes the object face to given point all the times

{
     
    public GameObject reflectorsFacingPoint;

    [SerializeField] private bool _faceTheTarget;
    
     
    public float turn_speed = 100f;
    [SerializeField] private bool faceX;
    [SerializeField] private bool faceY;
    [SerializeField] private bool faceZ;


    void Update()
    {


        
            Vector3 to = reflectorsFacingPoint.transform.position;

            Quaternion lookRotation = Quaternion.LookRotation((to - transform.position).normalized);




        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);
        
        rotating(_faceTheTarget);
          
    }


private void rotating(bool doOrNot)
{

if (doOrNot)
{
    Vector3 from =transform.position;
    Vector3 to = reflectorsFacingPoint.transform.position;

    if (!faceX)
        to=new Vector3(from.x,to.y,to.z);
    if (!faceY)
        to=new Vector3(to.x,from.y,to.z);
    if (!faceZ)
        to=new Vector3(to.x,to.y,from.z);

    Quaternion lookRotation = Quaternion.LookRotation((to - from).normalized);




   transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);
}
        
        
}
}
//////-----
//Vector3 rot = lookRotation.eulerAngles; // making  Quaternio vector3

//        if (Vector3.Distance(transform.position, reflectorsFacingPoint.transform.position)==0)
//        {
//             to = mirroredobject.transform.position;
//            lookRotation = Quaternion.LookRotation((to - transform.position).normalized);

            


//        }
//        // rot.y = rot.y - transform.rotation.y;
//        // lookRotation = Quaternion.Euler(rot); // making vector3 Quaternio
//        ////----