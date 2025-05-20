using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 1;
    public float currentHealth;
    public float energyOnDeath;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            Die();
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }
    /// <summary>
    /// called to first before Death() to initiate death animations and handle any cleanup and setup before destroying the gameobject.
    /// </summary>
    public virtual void Die()
    {
        //need to ensure player un-possesses a body here
        FindObjectOfType<PlayerController>().RemoveBodyInRange(gameObject);
        if(gameObject.GetComponent<BodyTakeover>().isPossessed)
        {
            FindObjectOfType<PlayerController>().BodySwap();
        }
        else
            FindObjectOfType<SoulEnergy>().AddEnergy(energyOnDeath);
        Death();
    }

    /// <summary>
    /// called when the game object gets destroyed
    /// </summary>
    protected void Death()
    {
        Destroy(gameObject);
    }
}
