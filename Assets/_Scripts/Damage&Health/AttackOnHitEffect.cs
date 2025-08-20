using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnHitEffect : MonoBehaviour
{
    [Tooltip("Duration of any lingering effects in seconds")]
    public float duration;
    public Effects onHitEffects;

    private Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() != null && !other.CompareTag(attack.currentTag))
        {
            ApplyOnHitEffects(other.GetComponent<EffectHandler>());
        }
    }

    public void ApplyOnHitEffects(EffectHandler target)
    {
        if(!target.GetComponent<Health>().invulnerable)
            target.ActivateOnHitEffects(onHitEffects, duration);
    }
}
