using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    private GameObject player;
    private Rigidbody rb;
    private BodyTakeover bodyTakeover;
    private Animator animator;
    private NavMeshAgent navAgent;

    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
        animator = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bodyTakeover.isPossessed)
            return;

        if (moveDir != Vector3.zero)
            bodyTakeover.SetAnimatorParam("MovementInput", true);
        else
            bodyTakeover.SetAnimatorParam("MovementInput", false);

        Combat();
    }

    private void FixedUpdate()
    {
        if (bodyTakeover.isPossessed)
            return;
        Movement();
    }

    public virtual void Movement()
    {
        moveDir = player.transform.position - gameObject.transform.position;
        //rb.AddForce(new Vector3(moveDir.x, 0, moveDir.z).normalized * 5, ForceMode.Acceleration);
        navAgent.SetDestination(player.transform.position);
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
