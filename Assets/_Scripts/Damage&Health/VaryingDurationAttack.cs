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
        bodyTakeover.restrictMovement = true;
        //any other effects needed for attack
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
