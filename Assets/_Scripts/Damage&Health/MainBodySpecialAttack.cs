using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBodySpecialAttack : Attack
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            if (other.GetComponent<EffectHandler>() != null && other.GetComponent<EffectHandler>().activeEffects.cursed)
            {
                other.GetComponent<EffectHandler>().RemoveEffect("cursed");
                FindObjectOfType<SoulEnergy>().AddEnergy(10);
            }
        }
    }
}
