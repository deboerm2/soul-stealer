using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : Enemy
{
    public override void Combat()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 5f && bodyTakeover.acceptAttackInputs)
        {
            if (Random.Range(1, 8) > 1)
                Attack();
            else
                AltAttack();
        }
    }

    
}
