using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteEnemy : Enemy
{
    public override void Combat()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 2.7f && bodyTakeover.acceptAttackInputs)
        {
            if (Random.Range(0, 5) == 0)
            {
                AltAttack();
            }
            else
                Attack();
        }
    }
}
