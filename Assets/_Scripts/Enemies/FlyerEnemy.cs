using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerEnemy : Enemy
{

    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        //animator is here so no warning/errors occur
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Movement()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * 2;
        bodyTakeover.bodyModel.transform.forward = bodyTakeover.followTarget.forward;
    }

    public override void Combat()
    {
        
    }

    //remember this enemy shoots non-damaging projectiles
    public override void Attack()
    {
        
    }

    
}
