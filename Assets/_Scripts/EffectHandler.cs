using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [HideInInspector]
    public Effects activeEffects;

    //references needed to apply effects
    private BodyTakeover _bodyTakeover;
    private Health _health;

    // Start is called before the first frame update
    void Start()
    {
        _bodyTakeover = GetComponent<BodyTakeover>();
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyEffects();
    }

    public void ActivateEffects(Effects areaEffects)
    {
        if(!activeEffects.slow && activeEffects.slow != areaEffects.slow)
        {
            activeEffects.slow = true;
        }
        if(!activeEffects.damage && activeEffects.damage != areaEffects.damage)
        {
            activeEffects.damage = true;
        }
    }

    void ApplyEffects()
    {
        if (activeEffects.slow && _bodyTakeover != null)
        {
            _bodyTakeover.SlowDown(0.7f);
        }
        if(activeEffects.damage && _health != null)
        {
            //_health.TakeDamage(0);
        }
    }
    public void RemoveEffects()
    {
        activeEffects.slow = false;
        _bodyTakeover.ReturnNormalSpeed();
        activeEffects.damage = false;
    }
}
