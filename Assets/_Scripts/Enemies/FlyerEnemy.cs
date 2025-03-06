using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerEnemy : Enemy
{
    //what the player attacks. the "creaking heart"
    public GameObject beacon;

    [Header("Orbit")]
    [SerializeField]
    private Transform orbitPoint;
    [SerializeField]
    private float orbitRadius;
    [SerializeField]
    private float orbitSpeed;

    [Header("Dive Bomb")]
    [SerializeField]
    private float diveSpeed;
    [SerializeField]
    private float startupDelay;
    [SerializeField]
    private float diveDamage;

    public bool doOrbit = true;

    private Vector3 storeRot = Vector3.zero;



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
        Combat();
    }

    public override void Movement()
    {
        if (doOrbit)
        {
            transform.rotation = Quaternion.Euler(storeRot);
            transform.RotateAround(orbitPoint.position, transform.up, -orbitSpeed);
            storeRot = transform.rotation.eulerAngles;
        }
    }

    public override void Combat()
    {
        if(Vector3.Distance(player.transform.position, gameObject.transform.position) < 10)
        {
            doOrbit = false;
            transform.LookAt(player.transform.position, transform.up);
        }
    }

    //remember this enemy shoots non-damaging projectiles
    public override void Attack()
    {

    }

    
}
