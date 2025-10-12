using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulEnergy : MonoBehaviour
{
    //display
    private Slider slider;
    public TextMeshProUGUI tmp;

    public float currentEnergy { get; private set; }
    private float maxEnergy = 100;

    public bool healMode = false;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    private void Update()
    {
        slider.value = currentEnergy / 100;
        tmp.text = currentEnergy.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="energy">energy the enemy will give upon death</param>
    public void AddEnergy(float energy)
    {
        if(!healMode)
        {
            if (currentEnergy < maxEnergy)
                currentEnergy += energy;
            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;
        }
        else
        {
            FindObjectOfType<PlayerHealth>().HealPlayer(energy-2);
        }
    }
}
