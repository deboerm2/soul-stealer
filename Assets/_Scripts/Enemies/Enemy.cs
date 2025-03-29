using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected static float GRAVITY = -9.81f;

    protected GameObject player;
    protected Rigidbody rb;
    protected BodyTakeover bodyTakeover;
    protected Animator animator;
    protected Weapon weapon;

    protected Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<Weapon>();
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
        rb.AddForce(new Vector3(moveDir.x, 0, moveDir.z).normalized * 0.8f, ForceMode.VelocityChange);
        if (Mathf.Sqrt((rb.velocity.x * rb.velocity.x) + (rb.velocity.z * rb.velocity.z)) >= bodyTakeover.maxSpeed)
        {
            rb.AddForce(new Vector3(-rb.velocity.x * 6, 0, -rb.velocity.z * 6), ForceMode.Acceleration);
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
        animator.SetBool("Light", true);
        animator.SetBool("inCombo", true);
        bodyTakeover.acceptAttackInputs = false;
    }
}
