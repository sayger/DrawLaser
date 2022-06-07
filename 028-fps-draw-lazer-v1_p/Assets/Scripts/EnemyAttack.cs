using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject bloodOverlay;
    
    
    public void Attack()
    {
        StartCoroutine(Blood());
    }

    IEnumerator Blood()
    {
        bloodOverlay.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        bloodOverlay.gameObject.SetActive(false);
    }
}
