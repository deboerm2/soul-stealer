using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBodySpecialAttack : Attack
{
    //these two list correlate to each other, healthTargets is the health script of each enemy in range and
    //damageTicks is their respective timer until the next damage they take
    private List<float> damageTicks = new List<float>();
    private List<Health> healthTargets = new List<Health>();

    public float soulPerTick = 2f;


    protected override void Update()
    {
        base.Update();
        HandleDamage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            healthTargets.Add(other.GetComponent<Health>());
            damageTicks.Add(0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            int index = healthTargets.IndexOf(other.GetComponent<Health>());
            healthTargets.RemoveAt(index);
            damageTicks.RemoveAt(index);
        }
    }

    void HandleDamage()
    {
        for (int i = 0; i < healthTargets.Count; i++)
        {
            if (damageTicks[i] > 0)
                damageTicks[i] -= Time.deltaTime;
            else if (damageTicks[i] <= 0)
            {
                healthTargets[i].TakeDamage(damage);
                damageTicks[i] = 0.5f;
                FindObjectOfType<SoulEnergy>().AddEnergy(soulPerTick);
            }
        }
    }
}
