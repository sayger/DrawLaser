using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamControll : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 value;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = value;
    }
}
