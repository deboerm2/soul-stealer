using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonArea : MonoBehaviour
{
    private SoulEnergy soulEnergy;

    private SoulSiphon soulSiphon;

    private void Start()
    {
        soulEnergy = FindObjectOfType<SoulEnergy>();
        soulSiphon = GetComponentInParent<SoulSiphon>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soulEnergy.isSiphoned = true;
            soulEnergy.activeSiphon = soulSiphon;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PlayerController.Instance.bodyTakeover.mainBody)
                PlayerController.Instance.BodySwap();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (soulEnergy.activeSiphon == soulSiphon)
            {
                soulEnergy.isSiphoned = false;
                soulEnergy.activeSiphon = null;
            }
        }
            
    }
}
