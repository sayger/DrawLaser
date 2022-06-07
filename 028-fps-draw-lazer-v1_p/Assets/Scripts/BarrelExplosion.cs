using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplosion : MonoBehaviour
{

    [SerializeField] private ParticleSystem[] FXs;
    private ExplosionForce explosionForce;
    public GameObject area;
   
    
    public bool fire = false;

    private void Awake()
    {
        area.SetActive(false);
        explosionForce = FindObjectOfType<ExplosionForce>();
    }


    public void PlayFXs()
    {
        if (fire)
        {
            for (int i = 0; i < FXs.Length; i++)
            {
                FXs[i].Play();
              area.SetActive(true);
            }
            
            
            fire = false;
        }
    }
}
