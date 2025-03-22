using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// gets set by the AttackDamageBehavior script on the attack states in the animator
    /// </summary>
    public float damage;
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
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        //used to determine what the weapon can hit
        currentTag = bodyTakeover.isPossessed ? "Player" : "Enemy";
        bodyTakeover.tag = currentTag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null && !other.CompareTag(currentTag))
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
