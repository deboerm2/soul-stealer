using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private GameObject player;
    private Rigidbody rb;
    private BodyTakeover bodyTakeover;

    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHealth>().gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
        bodyTakeover = gameObject.GetComponent<BodyTakeover>();
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
        rb.AddForce(new Vector3(moveDir.x, 0, moveDir.z).normalized * 5, ForceMode.Acceleration);
        GetComponentInChildren<Animator>().gameObject.transform.forward = new Vector3(moveDir.x, 0, moveDir.z);
    }
}
