using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumperEnemy : Enemy
{
    [Tooltip("Max distance of jump")]
    public float jumpDist;
    [Tooltip("Speed of enemy during jump in units/second")]
    public float jumpSpeed;
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
        if(!isJumping && player != null)
        {
            base.Movement();
        }
    }

    public override void Combat()
    {
        if (!bodyTakeover.isPossessed && player != null)
        {

            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < jumpDist && bodyTakeover.acceptAttackInputs)
            {
                Attack();
            }
        }
    }
    public override void Attack()
    {
        bodyTakeover.acceptAttackInputs = false;
        isJumping = true;
        jumpTime = 0;
        Instantiate(bodyTakeover.attackGO, transform);
        if(!bodyTakeover.isPossessed)
        {
            SetPoints();
        }
        else
        {
            SetPoints(gameObject.transform.position + (animator.gameObject.transform.forward.normalized * 10));
        }
        
    }

    public void MoveAlongCurve()
    {
        if (jumpTime < 1f)
        {
            rb.velocity = (Bezier2(p1, p2, p3, jumpTime) - rb.position).normalized * jumpSpeed;
            //jumpTime is affected by current timescale, but is also changed based on the distance of the jump with the jump speed
            jumpTime += Time.deltaTime * bodyTakeover.currentTimeScale * (jumpSpeed / Vector3.Distance(p1, p3));
        }
        else if(CheckGrounded())
        {
            isJumping = false;
            StartCoroutine(JumpCooldown());
        }
        else //continue along curve until grounded
        {
            rb.velocity = (Bezier2(p1, p2, p3, jumpTime) - rb.position).normalized * jumpSpeed;
            jumpTime += Time.deltaTime * bodyTakeover.currentTimeScale * (jumpSpeed / Vector3.Distance(p1, p3));
        }
    }

    void SetPoints()
    {
        p1 = transform.position;
        p3 = player.transform.position;
        p2 = (p1 + p3) / 2f;
        //height of apex, minimum of 0.5 units
        p2.y = Mathf.Max(Vector3.Distance(p1, p3) * jumpHeight, Mathf.Max(p1.y, p3.y) + 1f);
    }
    void SetPoints(Vector3 point3)
    {
        p1 = transform.position;
        p3 = point3;
        p2 = (p1 + p3) / 2f;
        //height of apex, minimum of 2 units
        p2.y = Mathf.Max(Vector3.Distance(p1, p3) * jumpHeight/2, Mathf.Max(p1.y, p3.y) + 2f);
    }

    //found at https://discussions.unity.com/t/moving-an-object-along-a-bezier-curve/1965/4
    public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
    {
        return (((1 - t) * (1 - t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
    }

    IEnumerator JumpCooldown()
    {
        if(GetComponentInChildren<VaryingDurationAttack>()) GetComponentInChildren<VaryingDurationAttack>().TurnOff();
        yield return new WaitForSeconds(jumpCooldown);
        bodyTakeover.acceptAttackInputs = true;
    }
}
