using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectAttack : Attack
{
    public override void Start()
    {
        base.Start();
        GetComponentInParent<Health>().invulnerable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if an attack froman enemy
        if(other.gameObject.GetComponent<Attack>() && !other.CompareTag(currentTag))
        {//reflect damage
            other.GetComponentInParent<Health>().TakeDamage(other.GetComponent<Attack>().damage);
            if(other.GetComponent<AttackOnHitEffect>())
            {
                //apply any onhit effect the attack may have
                other.GetComponent<AttackOnHitEffect>().ApplyOnHitEffects(other.GetComponentInParent<EffectHandler>());
            }
        }
    }

    public override IEnumerator AttackDuration()
    {
        yield return StartCoroutine(base.AttackDuration());
        if (GetComponentInParent<Health>())
            GetComponentInParent<Health>().invulnerable = false;
    }
}
