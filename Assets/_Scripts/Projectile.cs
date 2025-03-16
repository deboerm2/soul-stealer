using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Weapon
{
    [HideInInspector]
    public BodyTakeover projBodyTakeover;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        col = gameObject.GetComponent<Collider>();

        //projBodyTakeover gets set when instatiated by enemy script
        bodyTakeover = projBodyTakeover;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += transform.forward.normalized * speed * Time.deltaTime;
    }
}
