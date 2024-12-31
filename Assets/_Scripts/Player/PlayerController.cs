using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    [SerializeField]
    private GameObject currentBodyModel;
    /// <summary>
    /// used by BodyDetection to know what takeovers are in range
    /// </summary>
    private HashSet<GameObject> bodiesInRange = new HashSet<GameObject>();
    private GameObject closestTakeOver;
    private bool isPossessing = false;
    private SoulEnergy soulEnergy;
    

    public CinemachineVirtualCamera cineCam;
    public GameObject orientation;
    public GameObject mainBody;
    public GameObject mainBodyModel;
    [SerializeField]
    private GameObject currentBody;
    

    

    // Start is called before the first frame update
    void Start()
    {
        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        soulEnergy = FindObjectOfType<SoulEnergy>();
        BodySwap();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement Input & rotations
        
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetKeyDown(KeyCode.Space);
        inputZ = Input.GetAxis("Vertical");
        rotY = bodyCamera.camRotY;
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);
        movementDir = inputX * orientation.transform.right + inputZ * orientation.transform.forward;

        if (movementDir != Vector3.zero)
        {
            //need to define a rotation speed to rotate by
            currentBodyModel.transform.forward = movementDir;
            bodyTakeover.SetAnimatorParam("MovementInput", true);
        }
        else
            bodyTakeover.SetAnimatorParam("MovementInput", false);

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
            if(closestTakeOver != null)
                Debug.DrawLine(currentBody.transform.position, closestTakeOver.transform.position, Color.red);
        }

        #endregion
        #region Possesion
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPossessing)
            {
                BodySwap();
            }
            else if (closestTakeOver != null)
            {
                BodySwap(closestTakeOver);
            }
        }
        if(isPossessing)
        {
            mainBody.transform.position = currentBody.transform.position;
        }
        #endregion
        #region Combat
        if (bodyTakeover.acceptAttackInputs)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                bodyTakeover.BodyAttack();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                bodyTakeover.AltAttack();
            }
        }
        //still need input for special attack
        #endregion
    }

    private void FixedUpdate()
    {
        #region Movement
        //if player velocity is greater than max speed. slow down
        if (Mathf.Sqrt((playerRB.velocity.x * playerRB.velocity.x) + (playerRB.velocity.z * playerRB.velocity.z)) >= maxSpeed)
        {
            playerRB.AddForce(new Vector3(-playerRB.velocity.x * 6, 0, -playerRB.velocity.z * 6), ForceMode.Acceleration);
        }
        
        //counter movement to reduce sliding
        //if the player is not moveing in the direction they should, apply force to get there.
        if (new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).normalized != movementDir.normalized)
        {
            playerRB.AddForce((movementDir.normalized - new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).normalized) * acceleration, ForceMode.Acceleration);
        }

        //if there is input, move
        if (movementDir != Vector3.zero)
        {
            playerRB.AddForce(movementDir.normalized * acceleration, ForceMode.Acceleration);
        }
        if (canJump)
        {
            playerRB.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            canJump = false;
        }
        #endregion

    }
    //swaps all necessary variables to the main player body
    public void BodySwap()
    {
        playerRB = mainBody.GetComponent<Rigidbody>();
        playerCollider = mainBody.GetComponent<Collider>();
        bodyCamera = mainBody.GetComponentInChildren<CameraMovement>();
        bodyCamera.enabled = true;
        bodyTakeover.isPossessed = false;
        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        cineCam.Follow = bodyTakeover.followTarget;
        cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = bodyCamera.armDist;
        currentBodyModel = bodyTakeover.bodyModel;
        bodyTakeover.isPossessed = true;
        currentBody = mainBody;
        currentBody.transform.position += Vector3.up *3;

        mainBodyModel.SetActive(true);
        mainBody.GetComponent<Collider>().enabled = true;
        isPossessing = false;

    }
    //swaps all necessary variables to the target enemy body
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
        {
            playerCollider = target.GetComponent<Collider>();
        }

        bodyCamera = target.GetComponentInChildren<CameraMovement>();
        bodyCamera.enabled = true;
        bodyTakeover.isPossessed = false;
        bodyTakeover = target.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        cineCam.Follow = bodyTakeover.followTarget;
        cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = bodyCamera.armDist;
        currentBodyModel = bodyTakeover.bodyModel;
        bodyTakeover.isPossessed = true;
        currentBody = target;
        soulEnergy.AddEnergy(-bodyTakeover.soulNeeded);

        mainBodyModel.SetActive(false);
        mainBody.GetComponent<Collider>().enabled = false;
        isPossessing = true;
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
