using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSiphon: Health
{
    private SoulEnergy soulEnergy;

    public float storedEnergy;

    //stationary, any soul gained by player in area instead goes to bank.
    //if possessed enemy enters area, player is forcibly unpossessed, enemy dies as usual and soul goes to bank
    //area grows with soul stored

    //detection done by SiphonArea script
    void Start()
    {
        soulEnergy = FindObjectOfType<SoulEnergy>();
        currentHealth = maxHealth;
    }

    public override void Die()
    {
        soulEnergy.AddEnergy(storedEnergy);
        if (soulEnergy.activeSiphon == this)
        {
            soulEnergy.isSiphoned = false;
            soulEnergy.activeSiphon = null;
        }
        Death();
    }
}
