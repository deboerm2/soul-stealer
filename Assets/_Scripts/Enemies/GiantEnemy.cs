using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : Enemy
{
    public override void Combat()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 5f && bodyTakeover.acceptAttackInputs)
        {
            Attack();
        }
    }

    public override void Attack()
    {
        if(Random.Range(1,8) > 1)
            base.Attack();
        else
        {
            Instantiate(bodyTakeover.altAttackGO, bodyTakeover.bodyModel.transform);
            bodyTakeover.acceptAttackInputs = false;
        }
    }
}
