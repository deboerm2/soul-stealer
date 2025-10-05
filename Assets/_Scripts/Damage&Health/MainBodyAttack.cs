using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBodyAttack : Attack
{
    public float soulOnHit = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            FindObjectOfType<SoulEnergy>().AddEnergy(soulOnHit);
        }
    }
}
