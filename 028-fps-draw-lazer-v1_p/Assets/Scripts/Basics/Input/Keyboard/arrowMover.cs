using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowMover : MonoBehaviour
{

    
    public char up_downArrowsAxis='z';
    public bool reverse1=false;
    public char right_lefArrowsAxis='x';
    public bool reverse2=false;
    public float speed = 20;
    private float baseSpeed = 0.01f;
    
    public bool useLimitationX =false; 
    public float[] xLimits=new float[2];
    
    public bool useLimitationY =false ;
    public float[] yLimits=new float[2];
    
    public bool useLimitationZ =false ;
    public float[] zLimits=new float[2];

    public bool specialLimitations = false;
    public Transform specialLObject;
    public float specOBJLow;
    public float specOBJHigh;
    

    private Transform ourObject;
    
    void Start()
    {
        ourObject = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        validateLimitations(); 
        
        Vector3 additionToVector = getInput();
        additionToVector = checkLimitations(additionToVector);
        
        Vector3 target = ourObject.position + additionToVector;
        ourObject.position = target;
    }

    private Vector3 getInput()
    {
        float x = 0, y = 0, z = 0;

        int nP1 = reverse1 ? -1:1;
        int nP2 = reverse2 ? -1:1;
      
        if (Input.GetKey(KeyCode.UpArrow))
        {
            
            if (up_downArrowsAxis == 'x')
                x += baseSpeed * speed * nP1;
            if (up_downArrowsAxis == 'y')
                y += baseSpeed * speed * nP1;
            if (up_downArrowsAxis == 'z')
                z += baseSpeed * speed * nP1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
               
            if (up_downArrowsAxis == 'x')
                x += -1*baseSpeed * speed * nP1;
            if (up_downArrowsAxis == 'y')
                y += -1*baseSpeed * speed * nP1;
            if (up_downArrowsAxis == 'z')
                z += -1*baseSpeed * speed * nP1;
        }
          
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (right_lefArrowsAxis == 'x')
                x += baseSpeed * speed * nP2;
            if (right_lefArrowsAxis == 'y')
                y += baseSpeed * speed * nP2;
            if (right_lefArrowsAxis == 'z')
                z += baseSpeed * speed * nP2;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (right_lefArrowsAxis == 'x')
                x += -1*baseSpeed * speed * nP2;
            if (right_lefArrowsAxis == 'y')
                y += -1*baseSpeed * speed * nP2;
            if (right_lefArrowsAxis == 'z')
                z += -1*baseSpeed * speed * nP2;
        }
        return new Vector3(x,y,z);
    }
    private void validateLimitations()
    {
        useLimitationX = !(xLimits[0]>=xLimits[1])&&useLimitationX;
         
        useLimitationY = !(yLimits[0]>=yLimits[1])&&useLimitationY;
        
        useLimitationZ = !(zLimits[0]>=zLimits[1])&&useLimitationZ;;
    }

    private Vector3 checkLimitations(Vector3 input)
    {
        float X = ourObject.position.x;
        float Y = ourObject.position.y;
        float Z = ourObject.position.z;
        if (useLimitationX)
        {
            input.x= (X<xLimits[0]) ?( (input.x>0 )? input.x:0)  : input.x ;
            input.x= (X>xLimits[1]) ?( (input.x<0 )? input.x:0)  : input.x ; 
        }

        if (useLimitationY)
        {
            input.y= (Y<yLimits[0]) ?( (input.y>0 )? input.y:0)  : input.y ;
            input.y= (Y>yLimits[1]) ?( (input.y<0 )? input.y:0)  : input.y ;
        }

        if (specialLimitations)
        {
            Y = specialLObject.position.y;
              
            input.y= (Y<specOBJLow) ?( (input.y>0 )? input.y:0)  : input.y ;
            input.y= (Y>specOBJHigh) ?( (input.y<0 )? input.y:0)  : input.y ;
                    
                    
        }
        if (useLimitationZ)
        { 
            input.z= (Z<zLimits[0]) ?( (input.z>0 )? input.z:0)  : input.z ;
            input.z= (Z>zLimits[1]) ?( (input.z<0 )? input.z:0)  : input.z ; 
        }
        return input ;
    }
}
