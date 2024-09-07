using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float inputX;
    private bool inputY;
    private float inputZ;
    private Vector3 movementDir;
    private float rotY;
    [SerializeField]
    private bool isGrounded;
    private bool canJump;
    private Collider playerCollider;

    public Rigidbody playerRB;
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetKeyDown(KeyCode.Space);
        inputZ = Input.GetAxis("Vertical");
        movementDir = new Vector3(inputX, 0, inputZ);
        rotY = GetComponentInChildren<CameraMovement>().camRotY;

        isGrounded = Physics.Raycast(playerRB.ClosestPointOnBounds(playerCollider.bounds.center + (Vector3.down * 10))
            + (Vector3.up * 0.1f), Vector3.down, 0.2f);
        //Debug.DrawRay(playerRB.ClosestPointOnBounds(playerCollider.bounds.center + (Vector3.down * 10)), Vector3.down, Color.blue);
        if (isGrounded)
        {
            if (inputY)
            {
                canJump = true;
            }
        }
        else
        {
            canJump = false;
        }
    }

    private void FixedUpdate()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        //if player velocity is greater than max speed. slow down
        if (Mathf.Sqrt((playerRB.velocity.x * playerRB.velocity.x) + (playerRB.velocity.z * playerRB.velocity.z)) >= maxSpeed)
        {
            playerRB.AddForce(new Vector3(-playerRB.velocity.x * 6, 0, -playerRB.velocity.z * 6), ForceMode.Acceleration);
        }
        //if(Mathf.Abs(inputX) <= 0.1f)
        //{
        //    playerRB.AddRelativeForce(new Vector3(-playerRB.velocity.x * 6, 0, 0), ForceMode.Acceleration);
        //}
        //if(Mathf.Abs(inputZ) <= 0.1f)
        //{
        //    playerRB.AddRelativeForce(new Vector3(0, 0, -playerRB.velocity.z * 6), ForceMode.Acceleration);
        //}
        //if there is input, move
        if (movementDir != Vector3.zero)
        {
            playerRB.AddRelativeForce(movementDir.normalized * acceleration, ForceMode.Acceleration);
        }
        if (canJump)
        {
            playerRB.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            canJump = false;

        }
    }
}
