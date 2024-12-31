using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class JumperEnemy : Enemy
{
    [Tooltip("Max distance of jump")]
    public float jumpDist;
    [Tooltip("Speed of enemy during jump in units/second")]
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
        if (bodyTakeover.isPossessed)
            navAgent.enabled = false;
        else 
            navAgent.enabled = !isJumping;

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
        if (!bodyTakeover.isPossessed)
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
        weapon.col.enabled = true;
        if(!bodyTakeover.isPossessed)
        {
            SetPoints();
            weapon.damage = 5;
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
            rb.velocity = (Bezier2(p1, p2, p3, jumpTime * (jumpSpeed / Vector3.Distance(p1, p3))) - rb.position).normalized * jumpSpeed;
            jumpTime += Time.deltaTime;
        }

        else
        {
            isJumping = false;
            StartCoroutine(JumpCooldown());
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
        //height of apex, minimum of 0.5 units
        p2.y = Mathf.Max(Vector3.Distance(p1, p3) * jumpHeight, Mathf.Max(p1.y, p3.y) + 1f);
    }

    //found at https://discussions.unity.com/t/moving-an-object-along-a-bezier-curve/1965/4
    public static Vector3 Bezier2(Vector3 Start, Vector3 Control, Vector3 End, float t)
    {
        return (((1 - t) * (1 - t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    IEnumerator JumpCooldown()
    {
        weapon.col.enabled = false;
        yield return new WaitForSeconds(jumpCooldown);
        bodyTakeover.acceptAttackInputs = true;
    }
}
