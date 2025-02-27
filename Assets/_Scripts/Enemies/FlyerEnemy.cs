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

        /*
         Some thoughts/Plans for movement:
        1. patrols in a orbit (like a vulture)
        2. once detected a player, becomes bug like? shadows of evil weird bug enemies. stops to shoot.
            would be easier to not do that ^.
        3. maybe attempt to go above player and shoot straight down at them, incentivize player movement. while also not getting accidentally killed.
            ^would also match better with player possession mode if it just becomes a 1st person turret that shoots.
            ^ a cooler version would be bombing run manuever.

        */
    }

    public override void Combat()
    {
        
    }

    //remember this enemy shoots non-damaging projectiles
    public override void Attack()
    {
        
    }

    
}
