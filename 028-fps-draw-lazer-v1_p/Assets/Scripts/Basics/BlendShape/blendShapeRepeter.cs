using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blendShapeRepeter : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public float speed;
    public float currentBlend;
    public bool reverseMode;
    void Start()
    {
        currentBlend = meshRenderer.GetBlendShapeWeight(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentBlend = meshRenderer.GetBlendShapeWeight(0);
        var oldWeight = meshRenderer.GetBlendShapeWeight(0);
            var newWeight = Mathf.MoveTowards(oldWeight , 100, speed*Time.deltaTime );
        meshRenderer.SetBlendShapeWeight(0, newWeight);
        if (currentBlend>=100)
        {
            reverseMode = true;
            speed *= -1;
            meshRenderer.SetBlendShapeWeight(0, 100);
        }
        if (currentBlend<= 0&&reverseMode )
        {
            speed *= -1;
            meshRenderer.SetBlendShapeWeight(0, 0);
            reverseMode = false;
        }
    }
}
