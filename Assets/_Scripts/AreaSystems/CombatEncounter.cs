using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounter : AreaEncounter
{
    /// <summary>
    /// the container of the enemies needed to complete the combat encounter. needs to be manually set.
    /// </summary>
    public GameObject enemiesParent;

    // Start is called before the first frame update
    void Start()
    {
        if (enemiesParent == null)
            Debug.LogError("enemiesParent not set for " + gameObject.name);

        enemiesParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesParent.transform.childCount == 0) // and no enemies will spawn)
        {
            areaComplete = true;
        }
        if (areaComplete)
            AreaComplete();
    }

    public override void AreaComplete()
    {
        base.AreaComplete();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LockDoors();
            enemiesParent.SetActive(true);
        }
    }
}
