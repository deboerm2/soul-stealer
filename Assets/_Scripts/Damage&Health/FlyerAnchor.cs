using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAnchor : Health
{
    public GameObject flyerPrefab;
    [Tooltip("delay in seconds that a new flyer will spawn after the current one has been destroyed")]
    public float spawnDelay;

    [Header("Flyer Orbit")]
    public Transform orbitPoint;
    public float orbitRadius;
    public float orbitSpeed;

    private GameObject flyer;
    private bool needFlyer = false;
    private bool spawning = false;

    private void Start()
    {
        currentHealth = maxHealth;
        SpawnNewFlyer();
    }

    public void SpawnNewFlyer()
    {
        flyer = Instantiate(flyerPrefab, orbitPoint);
        FlyerEnemy flyEnemy = flyer.GetComponent<FlyerEnemy>();
        flyEnemy.orbitRadius = orbitRadius;
        flyEnemy.orbitSpeed = orbitSpeed;
        flyEnemy.orbitPoint = orbitPoint;
    }

    public override void Update()
    {
        base.Update();

        if(flyer == null && !spawning)
        {
            needFlyer = true;
        }
        if (needFlyer)
            StartCoroutine(SpawnDelay(spawnDelay));
    }

    IEnumerator SpawnDelay(float delay)
    {
        spawning = true;
        yield return new WaitForSeconds(delay);
        SpawnNewFlyer();
        needFlyer = false;
    }

    public override void Die()
    {
        if (flyer != null)
        {//need to ensure player un-possesses a body here
            PlayerController.Instance.RemoveBodyInRange(flyer);
            if (flyer.GetComponent<BodyTakeover>().isPossessed)
            {
                PlayerController.Instance.BodySwap();
            }
            else
                FindObjectOfType<SoulEnergy>().AddEnergy(energyOnDeath);
        }
        Death();
    }
}
