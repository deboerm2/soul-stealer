using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles effects that should affect the object it's attached to
/// </summary>
public class EffectHandler : MonoBehaviour
{
    [HideInInspector]
    public Effects activeEffects;

    //references needed to apply effects
    private BodyTakeover _bodyTakeover;
    private Health _health;


    private float slowTimer;
    private float damageTimer;
    private float curseTimer;

    private bool slowOnHit = false;
    private bool damageOnHit = false;
    private bool curseOnHit = false;
    

    // Start is called before the first frame update
    void Start()
    {
        _bodyTakeover = GetComponent<BodyTakeover>();
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //cancel and deny curse effect if cursed enemy gets possessed
        if (GetComponentInParent<BodyTakeover>().isPossessed)
        {
            activeEffects.cursed = false;
        }
        ApplyEffects();
        HandleOnHitEffects();
    }

    public void ActivateEffects(Effects areaEffects)
    {
        //if not already activated and areaEffect has a slow effect
        if(!activeEffects.slow && activeEffects.slow != areaEffects.slow)
        {
            activeEffects.slow = true;
        }
        //if not already activated and areaEffect has a damage effect
        if (!activeEffects.damage && activeEffects.damage != areaEffects.damage)
        {
            activeEffects.damage = true;
        }
        //if not already activated and areaEffect has a curse effect
        if (!activeEffects.cursed && activeEffects.cursed != areaEffects.cursed)
        {
            activeEffects.cursed = true;
        }
    }
    public void ActivateOnHitEffects(Effects onHitEffects, float duration)
    {
        //if onHitEffects has a slow effect, overwrite timer if duration is larger than current amount
        if (onHitEffects.slow)
        {
            activeEffects.slow = true;
            slowOnHit = true;
            slowTimer = Mathf.Max(slowTimer, duration);
        }
        //if onHitEffects has a damage effect, overwrite timer if duration is larger than current amount
        if (onHitEffects.damage)
        {
            activeEffects.damage = true;
            damageOnHit = true;
            damageTimer = Mathf.Max(damageTimer, duration);
        }
        //if onHitEffects has a curse effect, overwrite timer if duration is larger than current amount
        if (onHitEffects.cursed)
        {
            activeEffects.cursed = true;
            curseOnHit = true;
            curseTimer = Mathf.Max(curseTimer, duration);
        }
    }

    void HandleOnHitEffects()
    {
        slowTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;
        curseTimer -= Time.deltaTime;

        if (slowTimer > 0) activeEffects.slow = true;
        else if(slowOnHit)
        {
            RemoveEffects();
            slowOnHit = false;
        }

        if (damageTimer > 0) activeEffects.damage = true;
        else if (damageOnHit)
        {
            RemoveEffects();
            damageOnHit = false;
        }

        if (curseTimer > 0) activeEffects.cursed = true;
        else if (curseOnHit)
        {
            RemoveEffects();
            curseOnHit = false;
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
        if(activeEffects.cursed)
        {
            //soul on hit gain due to curse effect is handled in attack script.
        }
    }
    public void RemoveEffects()
    {
        activeEffects.slow = false;
        _bodyTakeover.ReturnNormalSpeed();
        activeEffects.damage = false;
        activeEffects.cursed = false;
    }
}
