using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    private Rigidbody playerRB;
    private CameraMovement bodyCamera;
    private BodyTakeover bodyTakeover;
    private float maxSpeed;
    private float acceleration;
    private float jumpStrength;
    private HashSet<GameObject> bodiesInRange = new HashSet<GameObject>();
    private GameObject closestTakeOver;

    public float takeoverRadius;
    public GameObject mainBody;
    public SphereCollider takeoverArea;
    [SerializeField]
    private GameObject currentBody;

    

    // Start is called before the first frame update
    void Start()
    {
        BodySwap();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement Input
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetKeyDown(KeyCode.Space);
        inputZ = Input.GetAxis("Vertical");
        movementDir = new Vector3(inputX, 0, inputZ);
        rotY = bodyCamera.camRotY;

        isGrounded = Physics.Raycast(playerRB.ClosestPointOnBounds(playerCollider.bounds.center + (Vector3.down * playerCollider.bounds.extents.y))
            + (Vector3.up * 0.1f), Vector3.down, 0.3f);
        Debug.DrawRay(playerRB.ClosestPointOnBounds(playerCollider.bounds.center + (Vector3.down * playerCollider.bounds.extents.y)), Vector3.down, Color.blue);
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
        #endregion
        #region Takeover detection
        if(bodiesInRange.Count > 0)
        {
            foreach(GameObject body in bodiesInRange)
            {
                if (!body.GetComponent<BodyTakeover>().isPossesable)
                {
                    continue;
                }
                else if (closestTakeOver == null)
                    closestTakeOver = body;
                else
                {
                    if ((closestTakeOver.transform.position - currentBody.transform.position).magnitude > 
                        (body.transform.position - currentBody.transform.position).magnitude)
                    {
                        closestTakeOver = body;
                    }
                }
            }
            Debug.DrawLine(currentBody.transform.position, closestTakeOver.transform.position, Color.red);
        }

        #endregion
    }

    private void FixedUpdate()
    {
        #region Movement
        currentBody.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        //if player velocity is greater than max speed. slow down
        if (Mathf.Sqrt((playerRB.velocity.x * playerRB.velocity.x) + (playerRB.velocity.z * playerRB.velocity.z)) >= maxSpeed)
        {
            playerRB.AddForce(new Vector3(-playerRB.velocity.x * 6, 0, -playerRB.velocity.z * 6), ForceMode.Acceleration);
        }
        
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
        #endregion

    }
    void BodySwap()
    {
        playerRB = mainBody.GetComponent<Rigidbody>();
        playerCollider = mainBody.GetComponent<Collider>();
        bodyCamera = mainBody.GetComponentInChildren<CameraMovement>();
        bodyCamera.enabled = true;
        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        currentBody = mainBody;
    }
    void BodySwap(GameObject target)
    {
        bodyCamera.enabled = false;

        if (target.GetComponent<Rigidbody>() == null)
            Debug.LogError("the target " + target.name + " does not have a rigidbody attached");
        else
            playerRB = target.GetComponent<Rigidbody>();

        if (target.GetComponent<Collider>() == null)
            Debug.LogError("the target " + target.name + " does not have a Collider attached");
        else
            playerCollider = target.GetComponent<Collider>();

        bodyCamera = target.GetComponentInChildren<CameraMovement>();
        bodyCamera.enabled = true;
        bodyTakeover = target.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        currentBody = target;

    }
    public void AddBody(GameObject body)
    {
        bodiesInRange.Add(body);
    }
    public void RemoveBody(GameObject body)
    {
        bodiesInRange.Remove(body);
    }
}
