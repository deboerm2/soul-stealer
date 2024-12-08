using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumperEnemy : Enemy
{
    [Tooltip("Max distance of jump")]
    public float jumpDist;
    [Tooltip("multiplier to determine the length of time to get to the end. 1 is base")]
    public float jumpSpeed; //need to make this based on actual speed like mph and not a lerp
    [Tooltip("determines the height of the jump's apex, multiplied by distance (ex. 0.5 will jump half as high as far")]
    public float jumpHeight;
    [Tooltip("time in seconds for the enemy to be able to jump again")]
    public float jumpCooldown;
    private float jumpTime;

    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;
    private bool isJumping;

   
    // Update is called once per frame
    void Update()
    {
        Combat();
        if(isJumping)
        {
            MoveAlongCurve();
        }
    }

    public override void Movement()
    {
        if (bodyTakeover.isPossessed)
        {
            gameObject.transform.forward = Vector3.forward;
            return;
        }
        else if(!isJumping && navAgent.isOnNavMesh)
        {
            navAgent.SetDestination(player.transform.position);
        }
    }

    public override void Combat()
    {

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < jumpDist && bodyTakeover.acceptAttackInputs)
        {
            Attack();
        }
    }
    public override void Attack()
    {
        navAgent.enabled = false;
        bodyTakeover.acceptAttackInputs = false;
        isJumping = true;
        SetPoints();
        jumpTime = 0;
        //rb.AddForce((player.transform.position - transform.position).normalized * jumpStrength, ForceMode.Impulse);
    }

    public void MoveAlongCurve()
    {
        transform.position = Bezier2(p1, p2, p3, jumpTime * jumpSpeed);
        jumpTime += Time.deltaTime;

        if(jumpTime >= 1 || Vector3.Distance(transform.position, p3) <= 0.3)
        {
            isJumping = false;
            navAgent.enabled = true;
            StartCoroutine(JumpCooldown());
        }
    }

    void SetPoints()
    {
        p1 = transform.position;
        p3 = player.transform.position;
        p2 = (p1 + p3) / 2f;
        p2.y = Vector3.Distance(p1, p3) * jumpHeight;
    }

    //found at https://discussions.unity.com/t/moving-an-object-along-a-bezier-curve/1965/4
    public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
    {
        return (((1 - t) * (1 - t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        bodyTakeover.acceptAttackInputs = true;
    }
}
