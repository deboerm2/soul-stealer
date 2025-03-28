using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAnchor : Health
{
    public GameObject flyer;
    public override void Die()
    {
        //need to ensure player un-possesses a body here
        FindObjectOfType<PlayerController>().RemoveBody(flyer);
        if (flyer.GetComponent<BodyTakeover>().isPossessed)
        {
            FindObjectOfType<PlayerController>().BodySwap();
        }
        else
            FindObjectOfType<SoulEnergy>().AddEnergy(energyOnDeath);
        Death();
    }
}
