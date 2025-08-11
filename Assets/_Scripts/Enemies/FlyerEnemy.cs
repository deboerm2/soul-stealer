using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerEnemy : Enemy
{
    [Tooltip("number of attacks per second")]
    /// <summary>
    /// number of attacks per second.
    /// </summary>
    public float attackSpeed = 1;
    public float attackRange = 10;

    [Header("Prefabs")]
    public GameObject slowingProjectile;

    [HideInInspector]
    public Transform orbitPoint;
    [HideInInspector]
    public float orbitRadius;
    [HideInInspector]
    public float orbitSpeed;

    [Header("Dive Bomb")]
    [SerializeField]
    private float diveSpeed;
    [SerializeField]
    private float startupDelay;
    [SerializeField]
    private float diveDamage;

    private bool doOrbit = true;
    private bool doAttack = true;

    private Vector3 storeRot = Vector3.zero;



    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        //animator is here so no warning/errors occur
        animator = GetComponentInChildren<Animator>();
        gameObject.transform.position = orbitPoint.transform.position + (Vector3.right * orbitRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (!bodyTakeover.isPossessed && player != null)
        {
            bodyTakeover.bodyModel.transform.rotation = gameObject.transform.rotation;
            Combat();
        }
        else
        {
            gameObject.transform.forward = Vector3.forward;
            bodyTakeover.bodyModel.transform.rotation = bodyTakeover.followTarget.transform.rotation;
        }
    }
    
    //called in Enemy FixedUpdate()
    public override void Movement()
    {
        if (bodyTakeover.isPossessed)
        {
            return;
        }
        else if (doOrbit)
        {
            transform.rotation = Quaternion.Euler(storeRot);
            transform.RotateAround(orbitPoint.position, transform.up, -orbitSpeed);
            storeRot = transform.rotation.eulerAngles;
        }
    }

    public override void Combat()
    {

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < attackRange)
        {
            doOrbit = false;
            transform.LookAt(player.transform.position, transform.up);
            TryAttack();
        }
        else
            doOrbit = true;
    }

    public void TryAttack()
    {
        if (!doAttack)
            return;
        else
        {
            Attack();
            StartCoroutine(AttackCooldown());
        }
    }

    //remember this enemy shoots non-damaging projectiles
    public override void Attack()
    {
        GameObject projectile = Instantiate(slowingProjectile);
        projectile.GetComponent<Projectile>().projBodyTakeover = bodyTakeover;
        projectile.transform.position = gameObject.transform.position;
        projectile.transform.forward = bodyTakeover.bodyModel.transform.forward;
        //flyer doesn't really care about attacks restricting movement, handles it on its own
        bodyTakeover.restrictMovement = false;
    }

    private IEnumerator AttackCooldown()
    {
        doAttack = false;
        yield return new WaitForSeconds(1 / attackSpeed);
        doAttack = true;
    }

    
}
