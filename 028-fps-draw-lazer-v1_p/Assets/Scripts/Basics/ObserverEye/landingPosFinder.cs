using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landingPosFinder : MonoBehaviour
{
    public bool work=true;
    public float detectionDistance=100;
    public Vector3 offset = new Vector3();
    public Transform originOfTheGaze;
    public Vector3 lookDirection=new Vector3(0,-1,0);
    public LayerMask landingLayers;
    public GameObject subject;
    private Vector3 target;
    void Start()
    {
        if (subject==null)
        {
            subject = this.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (work)
        {
             findLanding(originOfTheGaze, lookDirection, landingLayers);
            subject.transform.position = target;
        }
    }

    private void findLanding(Transform _originOfTheGaze,Vector3 _lookDirection,LayerMask _landingLayers)
    {
         

         
        
        if (Physics.Raycast (originOfTheGaze.position,lookDirection, out var hit, detectionDistance, landingLayers))
        {
            target = positionAline(hit.point);
        } 
        


         

    }
    private Vector3 positionAline(Vector3 input)
    {

        Vector3 result = new Vector3(input.x, input.y, input.z);
        result += offset;
            
        /*    
        if (xIssFixed)
        {
            input.x = targetFixedPositions.x;
        } 
        if (yIssFixed)
        {
            input.y = targetFixedPositions.y;
        } 
        if (zIssFixed)
        {
            input.z = targetFixedPositions.z;
        } */
        
        return result;
    }
}
