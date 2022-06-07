using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSwitchFromStock : MonoBehaviour
{
    [SerializeField] private Transform TargetParent ;
    [SerializeField] private bool DoAvatarSwitch;
    [SerializeField] private Animator TargetAnimator;
    [SerializeField] private Avatar BotsAvatar;
    [SerializeField] private Vector3 ToBeLocalPosition=Vector3.zero;
    private bool dontSetLocalPos=true ;
    
    
    [Header(" ACTIVATION ")]
    [SerializeField] private bool SwitchNow;
    
    [SerializeField] private int BotSwitchTo;
    
    [SerializeField] private List<GameObject> BotsList;
    [SerializeField] private bool TakeBotList;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (TakeBotList)
        {
            TakeBotList = false;
            BotsList= getChildrenAsObject(transform,1);
            Debug.Log("LIST HAS TAKEN");

        }
        if (SwitchNow)
        {
            SwitchNow = false;
            if (DoAvatarSwitch)
            {
                if (TargetAnimator==null)
                {
                    TargetAnimator = TargetParent.GetComponent<Animator>();
                }

                if (TargetAnimator==null)
                {
                    DoAvatarSwitch = false;
                }
            }

            if (BotSwitchTo<0||BotsList.Count<=BotSwitchTo)
            {
                return;
            }
            foreach (var VARIABLE in BotsList)
            {
                VARIABLE.SetActive(false);
                VARIABLE.transform.parent = transform;
            }
            GameObject TargetBot= BotsList[BotSwitchTo];
            TargetBot.transform.parent = TargetParent;
            
            if (!dontSetLocalPos) TargetBot.transform.localPosition = ToBeLocalPosition;

            TargetBot.transform.SetSiblingIndex(0);
            TargetBot.SetActive(true);

            if (DoAvatarSwitch)
            {
                BotsAvatar=TargetBot.GetComponent<AvatarHolder>()._Avatar;
                if (BotsAvatar!=null)
                {
                    TargetAnimator.avatar = BotsAvatar;
                }
                
            }
            
            specialSettings();

        }
    }

    private void specialSettings()
    {
        //--------------------------------------------------------------------------------------
      //  RagdollStateControl targetStateControl = TargetBot.GetComponent<RagdollStateControl>();
     //   charScript.RagdollStateControler = targetStateControl;
        //   _animator.avatar=TargetBot.GetComponent<AvatarHolder>()._Avatar;
    }

    private List<Transform> getChildrenAsTransforms(List<Transform> Subjects , int NumberOfLayerToSearch)
    {
        NumberOfLayerToSearch--;
         List<Transform> firstDepths = new List<Transform>();
         foreach (var VARIABLE in Subjects)
         {
              
             firstDepths.AddRange(getFirstDepthOfChildren(VARIABLE));
         }

         if (NumberOfLayerToSearch>0)
         {
             firstDepths.AddRange( getChildrenAsTransforms(firstDepths, NumberOfLayerToSearch));
         }

         return firstDepths;


    }
    private List<Transform> getFirstDepthOfChildren( Transform  Subject )
    {
        Transform [] allChild = Subject.GetComponentsInChildren<Transform>();
        List<Transform> firstDepth = new List<Transform>();
        foreach (var VARIABLE2 in allChild)
        {
            if (VARIABLE2.parent==Subject)
            {
                firstDepth.Add(VARIABLE2);
            }
        }

        return firstDepth;

    }
    private List<GameObject> getChildrenAsObject(Transform Subject , int NumberOfLayerToSearch)
    {
        List<GameObject> result = new List<GameObject>();
        List<Transform> temp =  getChildrenAsTransforms(Subject, NumberOfLayerToSearch);
        foreach (var VARIABLE in temp)
        {
            result.Add(VARIABLE.gameObject);
        }

        return result;
    }
    private List<Transform> getChildrenAsTransforms( Transform   Subject , int NumberOfLayerToSearch)
    {
        List<Transform> input =  new List<Transform>();
        input.Add(Subject);
        return  getChildrenAsTransforms(input, NumberOfLayerToSearch);
    }
}
