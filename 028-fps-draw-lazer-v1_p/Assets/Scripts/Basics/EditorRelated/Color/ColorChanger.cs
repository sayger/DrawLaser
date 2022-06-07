using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = System.Object;

public class ColorChanger : MonoBehaviour
{
    [TextArea]
    public string PURPOSE = "";
    [SerializeField] private bool ChangeColorsNow;
    [SerializeField] private bool DetectTargetMeshRenderersNow;
    [SerializeField] private bool SearchInOnlyOneObject;
    [SerializeField] private Transform SelectedObject;
    [SerializeField] private bool onlyRegisterActiveObjects;
    
    
    [SerializeField] private bool SelectWithTag;
    [SerializeField] private string Tag;
    [SerializeField] private bool SelectWithLayer;
    [SerializeField] private LayerMask Layers;
    private LayerMask _Layers;
    
    [SerializeField] private bool SelectWithColorType;
    [SerializeField] private int  _type;
    
    
    [SerializeField] private Material NewMaterial;
    [SerializeField] private int MaterialOrderInRenderer;
    [SerializeField] private List<MeshRenderer> DetectedMeshRenderers=new List<MeshRenderer>();
    private List<Transform> tempList=new List<Transform>();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (SelectWithLayer)
        {
            if (Layers !=_Layers)
            {
                DetectTargetMeshRenderersNow = true;
                _Layers = Layers;
            }
        }
        if (ChangeColorsNow)
        {
            ChangeColorsNow = false;
            ChangeColors();
        }

        if (DetectTargetMeshRenderersNow)
        {
            DetectTargetMeshRenderersNow = false;
            DetectRenderers();
        }
    }

    private void ChangeColors()
    {
        if (DetectedMeshRenderers.Count==0)
        {
            return;
        }

        foreach (var VARIABLE in DetectedMeshRenderers)
        {
            if (VARIABLE.materials.Length<=MaterialOrderInRenderer)
            {
                continue;
            }

          //  VARIABLE.materials[MaterialOrderInRenderer] = NewMaterial;
          VARIABLE.material = NewMaterial;
        }
        
    }
    private void DetectRenderers()
    {
        if (!SelectWithTag&&!SelectWithLayer&&!SelectWithColorType)
        {
            return;
        }
        List<MeshRenderer> allDetected = new List<MeshRenderer>();
          tempList = new List<Transform>();
        if (SearchInOnlyOneObject)
        {
            tempList = SelectedObject.GetComponentsInChildren<Transform>().ToList();
        }
        else
        {
            tempList = FindObjectsOfType<Transform>().ToList();
        }

        foreach (var VARIABLE in tempList)
        {
            
            if (ValidateTransform(VARIABLE))
            {
                MeshRenderer temp = VARIABLE.GetComponent<MeshRenderer>();
                if (temp!=null)
                {
                    allDetected.Add(temp);
                }
                
            }
        }

        DetectedMeshRenderers =allDetected ;
        if (DetectedMeshRenderers.Count==0)
        {
            Debug.Log(" NO MESH RENDERER COULD BE FOUND");
        }
    }

    private bool ValidateTransform(Transform candidate)
    {
        if (onlyRegisterActiveObjects)
        {
            if (!candidate.gameObject.activeSelf)
            {
                return false;
            }
        }
        if (SelectWithTag)
        {
            if (!candidate.CompareTag(Tag))
            {
                return false;
            }
        }
        if (SelectWithLayer)
        {
            if( ! (_Layers == (_Layers | (1 << candidate.gameObject.layer))))
            {
                return false;
            }
        }
        if (SelectWithColorType)
        {
            ColorStamp temp = candidate.GetComponent<ColorStamp>();
            if (temp==null)
            {
                return false;
            }
            if( temp.ColorId!=_type)
            {
                return false;
            }
        }
        
        
         
        return true;



    }
}
