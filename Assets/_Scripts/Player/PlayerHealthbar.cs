using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the UI display of the player healthbar
/// </summary>
public class PlayerHealthbar : MonoBehaviour
{

    public PlayerHealth playerHealthScript;

    public Slider playerHealth;
    public Slider possesedHealth;
    //possedHealth display will be overlayed player health like a shield? otherwise just a different color.


    // Start is called before the first frame update
    void Start()
    {
        playerHealth.maxValue = playerHealthScript.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.value = playerHealthScript.currentHealth;

        if(playerHealthScript.noPossessedHealth)
        {
            possesedHealth.gameObject.SetActive(false);
        }
        else
        {
            possesedHealth.gameObject.SetActive(true);
            possesedHealth.maxValue = playerHealthScript.possesedHealthMax;
            possesedHealth.value = playerHealthScript.possesedHealth;
            
        }
        
    }
}
