using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class learnVertical_Test : MonoBehaviour
{
    [SerializeField] private bool print=false;
    
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    [SerializeField] private bool showVertices=false;
    [SerializeField] private bool showTriangles=false;
    
    
    void Start()
    {
        

    }
    void Update()
    {
        if (print)
        {
            print = false;
            mesh = GetComponent<MeshFilter>().mesh;
            vertices = DuplicateArray(mesh.vertices) ;
            triangles = DuplicateArray(mesh.triangles);
        
            if(showVertices)
                debugPrintVector3Array(vertices,"verticles");
            if(showTriangles)
                debugPrintTriangles(triangles, "triangles");
        }
    }

    private Vector3 [] DuplicateArray(Vector3 [] input)
    { 
        Vector3 [] output =new Vector3[input.Length];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = input[i];
        }

        return output;
    }
    private int [] DuplicateArray(int [] input)
    { 
        int [] output =new int[input.Length];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = input[i];
        }

        return output;
    }

    private void debugPrintVector3Array(Vector3 [] input, String name)
    {
        for (int i = 0; i < input.Length; i++)
        {
            Debug.Log(name+" respectively "+i+"  : "+input[i] );
        }
    }

    private void debugPrintTriangles(int[] input, String name)
    {
        int x = 0; 
        Debug.Log(name+" Total number of  elements : "+ input.Length );
        Debug.Log( " The triple group size :  "+ (input.Length%3==0 ? input.Length/3 : ((input.Length/3))+1 ) );
        if(input.Length!=0)
        Debug.Log( " The first  : "+ input[0] + "  the last  : "+(input[input.Length-1] ));
            
        
        for (int i = 0; i < input.Length; i++)
        {
            x++;
            if ((i + 3) > input.Length)
            {   
                if((i + 2) == input.Length) 
                {
                    Debug.Log(name+" respectively in order of triplets Group "+x+"  : ( "+input[i]+", "+input[i+1]+",     ) " );
                    break;
                }
                if((i + 1) == input.Length) 
                {
                    Debug.Log(name+" respectively in order of triplets Group "+x+"  : ( "+input[i]+",     ,     ) " );
                    break;
                }
                 
            }
            
            
            Debug.Log(name+" respectively in order of triplets Group "+x+"  : ( "+input[i]+", "+input[i+1]+", "+input[i+2]+" ) " );
            if ((i + 3) == input.Length)
                break;
            else i += 2;
        }
    }
    
  
}
