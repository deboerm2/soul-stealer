using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 1;
    public float currentHealth;
    public float energyOnDeath;
    private bool isPossessed;

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
    /// called to initiate death animations and other such things
    /// </summary>
    public virtual void Die()
    {
        //need to ensure player un-possesses a body here
        FindObjectOfType<PlayerController>().RemoveBody(gameObject);
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
    void Death()
    {
        Destroy(gameObject);
    }
}
