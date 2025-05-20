using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Attack Stats")]
    public float damage;
    [Tooltip("Time in seconds the hitbox remains in scene")]
    public float duration;
    [Tooltip("Time in seconds before hitbox spawns, the 'windup'")]
    public float startup;
    [Tooltip("Time in seconds after hitbox despawns and before the player regains proper control")]
    public float endlag;
    [Header("Additional references")]
    [HideInInspector]
    public Collider col;
    [HideInInspector]
    public string currentTag;
    protected BodyTakeover bodyTakeover;

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
        bodyTakeover = GetComponentInParent<BodyTakeover>();

        //used to determine what the weapon can hit
        currentTag = bodyTakeover.tag;

        StartCoroutine(AttackStartup());
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
    IEnumerator AttackStartup()
    {
        yield return new WaitForSeconds(startup);
        StartCoroutine(AttackDuration());
    }

    IEnumerator AttackDuration()
    {
        col.enabled = true;
        yield return new WaitForSeconds(duration);
        col.enabled = false;
        StartCoroutine(AttackEndlag());
    }

    IEnumerator AttackEndlag()
    {
        yield return new WaitForSeconds(endlag);
        bodyTakeover.acceptAttackInputs = true;
        //movement return to normal here
    }
}
