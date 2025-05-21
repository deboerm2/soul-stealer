using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaryingDurationAttack : Weapon
{
    // Start is called before the first frame update
    void Start()
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
        //any other effects needed for attack
    }

    public void TurnOff()
    {
        col.enabled = false;
        bodyTakeover.restrictMovement = false;
        //if any effects haven't turned off already do it here

        Destroy(gameObject);
    }
}
