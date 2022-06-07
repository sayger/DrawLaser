using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshAgentTest : MonoBehaviour
{
    [SerializeField] private bool test;
    [SerializeField] private int agentNo;
    [SerializeField] private NavMeshAgent agent;

    
    public int id;
    public string name;
    
    void Start()
    {
          id = NavMesh.GetSettingsByIndex(1).agentTypeID;
         name = NavMesh.GetSettingsNameFromID(id);
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            test = false;
            agent.agentTypeID = agentNo;
        }
    }
}
