using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Health healthScript;
    public Slider healthBar;


    private void Start()
    {
        healthBar = gameObject.GetComponent<Slider>();
        healthBar.maxValue = healthScript.maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        healthBar.value = healthScript.currentHealth;

        transform.LookAt(Camera.main.transform, Vector3.down);
    }
}
