using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Weapon
{
    [HideInInspector]
    public BodyTakeover projBodyTakeover;
    public float speed = 1;
    public float lifetime = 5f;
    public GameObject slowArea;

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();

        //projBodyTakeover gets set when instatiated by enemy script
        bodyTakeover = projBodyTakeover;

        //used to determine what the weapon can hit
        currentTag = bodyTakeover.isPossessed ? "Player" : "Enemy";
        bodyTakeover.tag = currentTag;

        StartCoroutine(ProjectileLifetime(lifetime));
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += transform.forward.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.GetComponent<Health>() != null && !other.CompareTag(currentTag)) || other.CompareTag("Environment"))
        {
            Burst();
        }
    }

    private void Burst()
    {
        GameObject burstArea = Instantiate(slowArea);
        burstArea.transform.position = gameObject.transform.position;

        Destroy(gameObject);
    }

    IEnumerator ProjectileLifetime(float _lifetime)
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
