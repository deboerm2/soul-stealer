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

    protected Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        animator = GetComponentInChildren<Animator>();
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
        Movement();
    }

    public virtual void Movement()
    {
        if (bodyTakeover.isPossessed)
        {
            gameObject.transform.forward = Vector3.forward;
            return;
        }

        moveDir = player.transform.position - gameObject.transform.position;
        moveDir.y = 0;
        if (bodyTakeover.restrictMovement)
            moveDir = Vector3.zero;

        //going too fast, slow down
        if (Mathf.Sqrt((rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z)) >= bodyTakeover.maxSpeed)
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

        //rb.velocity += Vector3.up * GRAVITY;

        animator.gameObject.transform.forward = new Vector3(moveDir.x, 0, moveDir.z);
    }
    /// <summary>
    /// logic for when the enemy will attack
    /// </summary>
    public virtual void Combat()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 2 && bodyTakeover.acceptAttackInputs)
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
}
