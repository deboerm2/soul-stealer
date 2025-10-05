using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryingDurationAttack : Attack
{
    // Start is called before the first frame update
    public override void Start()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
        bodyTakeover = GetComponentInParent<BodyTakeover>();

        //used to determine what the weapon can hit
        currentTag = bodyTakeover.tag;

        TurnOn();
    }

    public void TurnOn()
    {
        col.enabled = true;
        bodyTakeover.restrictMovement = true;
        //any other effects needed for attack
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            if (other.GetComponent<EffectHandler>() != null && other.GetComponent<EffectHandler>().activeEffects.cursed)
            {
                FindObjectOfType<SoulEnergy>().AddEnergy(2);
            }

            GetComponentInParent<JumperEnemy>().StartJumpCooldown();
        }
    }

    public void TurnOff()
    {
        col.enabled = false;
        GetComponent<Renderer>().material.color = Color.blue;
        StartCoroutine(DoEndlag());
        //if any effects haven't turned off already do it here

        
    }
    IEnumerator DoEndlag()
    {
        yield return new WaitForSeconds(endlag);
        bodyTakeover.restrictMovement = false;
        Destroy(gameObject);
    }
}
