using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static float GRAVITY = -9.81f;

    protected GameObject player;
    protected Rigidbody rb;
    private Collider col;
    protected BodyTakeover bodyTakeover;
    protected Animator animator;

    protected float maxSpeed;
    protected float aggroTime;
    protected float restPeriod;
    protected bool giveChase = true;

    protected Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        animator = GetComponentInChildren<Animator>();
        maxSpeed = bodyTakeover.maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward);
        if (bodyTakeover.isPossessed)
            return;
        else
        {
            if (moveDir != Vector3.zero)
                bodyTakeover.SetAnimatorParam("MovementInput", true);
            else
                bodyTakeover.SetAnimatorParam("MovementInput", false);

            Combat();
        }
        
    }

    private void FixedUpdate()
    {
        if (bodyTakeover.isPossessed)
        {
            gameObject.transform.forward = Vector3.forward;
            return;
        }
        Movement();
    }

    public virtual void Movement()
    {

        //too far, catch up and close distance
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > 15 && giveChase)
        {
            maxSpeed = bodyTakeover.maxSpeed * 2;
        }
        else
        {
            maxSpeed = bodyTakeover.maxSpeed;
            //occasionally slow down to "rest", mostly to keep from similar enemies becoming stacked on one another.
            if (giveChase)
            {
                
                aggroTime += Time.fixedDeltaTime;
                giveChase = StillAngry(aggroTime);
            }
            else
            {
                
                restPeriod += Time.fixedDeltaTime;
                maxSpeed = bodyTakeover.maxSpeed * 0;
                if (restPeriod >= 2f)
                {
                    giveChase = true;
                    restPeriod = 0;
                    aggroTime = 0;
                }
            }
        }


        Move();
    }
    private void Move()
    {
        moveDir = player.transform.position - gameObject.transform.position;
        moveDir.y = 0;
        if (bodyTakeover.restrictMovement)
        {
            moveDir = Vector3.zero;
        }

        //going too fast, slow down
        if (Mathf.Sqrt((rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z)) >= maxSpeed)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * 6, 0, -rb.velocity.z * 6), ForceMode.Acceleration);
        }

        //change direction if it doesn't match input
        if (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized != moveDir.normalized)
        {
            rb.AddForce((moveDir.normalized - new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized), ForceMode.VelocityChange);
        }

        //if there is input, move
        if (moveDir != Vector3.zero)
        {
            rb.AddForce(moveDir.normalized * bodyTakeover.acceleration, ForceMode.Acceleration);
        }

        Vector3 faceForward = player.transform.position - gameObject.transform.position;
        faceForward.y = 0;
        animator.gameObject.transform.forward = faceForward;
    }
    /// <summary>
    /// logic for when the enemy will attack
    /// </summary>
    public virtual void Combat()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 1.7f && bodyTakeover.acceptAttackInputs)
        {
            Attack();
        }
        
    }
    public virtual void Attack()
    {
        Instantiate(bodyTakeover.attackGO, bodyTakeover.bodyModel.transform);
        bodyTakeover.acceptAttackInputs = false;
    }

    public bool CheckGrounded()
    {
        return Physics.Raycast(rb.ClosestPointOnBounds(col.bounds.center + (Vector3.down * col.bounds.extents.y))
           + (Vector3.up * 0.1f), Vector3.down, 0.3f);
    }

    public bool StillAngry(float _aggroTime)
    {
        float deAggroChance = Mathf.InverseLerp(5, 10, _aggroTime);

        if (Random.Range(0, 100) / 100 < deAggroChance)
        {
            return false;
        }
        else
            return true;
    }
}
