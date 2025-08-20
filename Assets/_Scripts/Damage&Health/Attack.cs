using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Stats")]
    public float damage;
    [Tooltip("Time in seconds the hitbox remains in scene")]
    public float duration;
    [Tooltip("Time in seconds before hitbox spawns, the 'windup'")]
    public float startup;
    [Tooltip("Time in seconds after hitbox despawns and before the player regains proper control")]
    public float endlag;

    [Header("Secondary Attacks")]
    [Tooltip("Any secondary attack to appear later")]
    public GameObject secondaryAttack;
    [Tooltip("Time in seconds the secondary attack will spawn after primary attack")]
    public float spawnDelay;

    [Header("Additional references")]
    [HideInInspector]
    public Collider col;
    [HideInInspector]
    public string currentTag;
    protected BodyTakeover bodyTakeover;

    private bool attackComplete = false;
    private bool secondaryAttackComplete = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
        GetComponent<Renderer>().enabled = false;
        bodyTakeover = GetComponentInParent<BodyTakeover>();

        //used to determine what the weapon can hit
        currentTag = bodyTakeover.tag;

        if (secondaryAttack == null) secondaryAttackComplete = true;

        StartCoroutine(AttackStartup());
    }

    private void Update()
    {
        if(attackComplete && secondaryAttackComplete)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            if(other.GetComponent<EffectHandler>() != null && other.GetComponent<EffectHandler>().activeEffects.cursed)
            {
                FindObjectOfType<SoulEnergy>().AddEnergy(3);
            }
        }
    }
    IEnumerator AttackStartup()
    {
        bodyTakeover.restrictMovement = true;
        yield return new WaitForSeconds(startup);
        StartCoroutine(AttackDuration());
    }

    public virtual IEnumerator AttackDuration()
    {
        col.enabled = true;
        GetComponent<Renderer>().enabled = true;
        if(secondaryAttack != null) StartCoroutine(SecondaryAttack());
        yield return new WaitForSeconds(duration);
        col.enabled = false;
        GetComponent<Renderer>().enabled = false;
        StartCoroutine(AttackEndlag());
    }

    IEnumerator AttackEndlag()
    {
        yield return new WaitForSeconds(endlag);
        bodyTakeover.acceptAttackInputs = true;
        bodyTakeover.restrictMovement = false;
        attackComplete = true;
    }

    IEnumerator SecondaryAttack()
    {
        yield return new WaitForSeconds(spawnDelay);
        GameObject _secondAttack = Instantiate(secondaryAttack);
        _secondAttack.transform.position += gameObject.transform.position;
        if (_secondAttack.GetComponent<Attack>() != null) _secondAttack.GetComponent<Attack>().currentTag = currentTag;
        if (_secondAttack.GetComponent<EffectArea>() != null) _secondAttack.GetComponent<EffectArea>().unaffectedTag = currentTag;

        secondaryAttackComplete = true;

    }
}
