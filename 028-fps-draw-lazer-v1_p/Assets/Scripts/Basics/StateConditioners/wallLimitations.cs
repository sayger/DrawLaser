using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallLimitations : MonoBehaviour
{
    public int id;
     private BoxCollider thisCollider;
    void Start()
    {
         
      
        adjustments();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void adjustments()
    {
        thisCollider = gameObject.GetComponent<BoxCollider>();
    }

    public void WallState(bool wallImpassableElseNot)
    {
        thisCollider.isTrigger = !wallImpassableElseNot;
    }
    public void MakeWallSolid( )
    {
        WallState(true);
    }
    public void MakeWallNotSolid( )
    {
        WallState(false);
    }
    public void MakeWallSolid(int _id )
    {
        if (id==_id)
        {
            WallState(true);
        }
    }
    public void MakeWallNotSolid(int _id )
    {
        if (id==_id)
        {
            WallState(false);
        }
        
    }
}
