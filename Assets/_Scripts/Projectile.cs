using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Weapon
{
    [HideInInspector]
    public BodyTakeover projBodyTakeover;

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
        //move foward
    }
}
