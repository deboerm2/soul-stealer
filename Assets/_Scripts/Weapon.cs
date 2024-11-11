using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// gets set by the AttackDamageBehavior script on the attack states in the animator
    /// </summary>
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public Collider col;

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != null)
            other.GetComponent<Health>().TakeDamage(damage);
    }
}
