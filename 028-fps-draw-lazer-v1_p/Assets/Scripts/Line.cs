using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Line : MonoBehaviour
{
    LineRenderer lineRenderer;
   
    List<Vector2> points;

    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject trailReferance;

    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
    }


    public void UpdateLine(Vector3 mousePosition)
    {
        
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(mousePosition);
            return;
        }

        if (Vector3.Distance(points.Last(), mousePosition) > .1f)
        {
            SetPoint(mousePosition);
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            trailReferance.gameObject.transform.position = lineRenderer.GetPosition(0);
            
        }
        trailReferance.gameObject.SetActive(true);
        //trailReferance.transform.localPosition=lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        trailReferance.transform.position = mousePosition;
        particle.transform.localPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
    }

    public void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        

    }
    

}
