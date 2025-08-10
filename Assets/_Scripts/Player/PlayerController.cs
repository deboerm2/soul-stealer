using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private InputAction inputXZ;
    private InputAction inputY;
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

    public InputActionAsset plControls;
    //public CinemachineVirtualCamera cineCam;
    public GameObject mainBody;
    public GameObject mainBodyModel;
    public GameObject currentBody { get; private set; }
    

    

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        soulEnergy = FindObjectOfType<SoulEnergy>();
        Startup();
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement Input & rotations
        
        inputXZ = plControls.FindAction("move");
        inputY = plControls.FindAction("jump");
        rotY = bodyCamera.camRotY;
        movementDir.x = inputXZ.ReadValue<Vector2>().x;
        movementDir.z = inputXZ.ReadValue<Vector2>().y;
        movementDir = Quaternion.AngleAxis(rotY, Vector3.up) * movementDir;

        if (bodyTakeover.restrictMovement)
            movementDir = Vector3.zero;

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
            if (inputY.triggered)
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
        if (bodiesInRange.Count > 0)
        {
            foreach (GameObject body in bodiesInRange)
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
            if (closestTakeOver != null)
                Debug.DrawLine(currentBody.transform.position, closestTakeOver.transform.position, Color.red);
        }
        else
            closestTakeOver = null;

        #endregion
        #region Possesion
        if (plControls.FindAction("possession").triggered)
        {
            if (isPossessing)
            {
                BodySwap();
                if (!closestTakeOver.GetComponent<BodyTakeover>().isPossesable)
                {
                    closestTakeOver = null;
                }
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
            if (plControls.FindAction("basicAttack").triggered)
            {
                bodyTakeover.BodyAttack();
            }
            if (plControls.FindAction("specialAttack").triggered)
            {
                bodyTakeover.AltAttack();
            }
        }
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
            if (playerRB.velocity.magnitude < 2f && movementDir == Vector3.zero)
            {
                playerRB.velocity = Vector3.zero;
            }
            else
                playerRB.AddForce((movementDir.normalized - new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z).normalized),
                    ForceMode.VelocityChange);
            
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
        playerRB.velocity *= bodyTakeover.currentTimeScale;
        #endregion

    }
    //swaps all necessary variables to the main player body
    public void BodySwap()
    {
        playerRB = mainBody.GetComponent<Rigidbody>();
        playerCollider = mainBody.GetComponent<Collider>();
        bodyCamera = mainBody.GetComponentInChildren<CameraMovement>();
        bodyTakeover.cineCam.enabled = false;
        bodyCamera.enabled = true;
        bodyTakeover.isPossessed = false;
        bodyTakeover.GetComponent<Health>().Die();
        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        bodyTakeover.cineCam.enabled = true;
        
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
        //target cineCam will have higher priority over player mainBody cineCam
        bodyTakeover.cineCam.enabled = true;
        //cineCam.Follow = bodyTakeover.followTarget;
        //cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = bodyCamera.armDist;
        currentBodyModel = bodyTakeover.bodyModel;
        bodyTakeover.isPossessed = true;
        currentBody = target;
        soulEnergy.AddEnergy(-bodyTakeover.soulNeeded);

        mainBodyModel.SetActive(false);
        mainBody.GetComponent<Collider>().enabled = false;
        isPossessing = true;
    }
    
    //adds a gameobject to the hashSet used by player to know which bodies are even in range to be considered possessable candidates
    public void AddBodyInRange(GameObject body)
    {
        bodiesInRange.Add(body);
    }
    //removes a gameobject from the hashSet used by player to know which bodies are even in range to be considered possessable candidates
    public void RemoveBodyInRange(GameObject body)
    {
        bodiesInRange.Remove(body);
    }

    void Startup()
    {
        playerRB = mainBody.GetComponent<Rigidbody>();
        playerCollider = mainBody.GetComponent<Collider>();
        bodyCamera = mainBody.GetComponentInChildren<CameraMovement>();
        bodyCamera.enabled = true;

        bodyTakeover = mainBody.GetComponent<BodyTakeover>();
        maxSpeed = bodyTakeover.maxSpeed;
        acceleration = bodyTakeover.acceleration;
        jumpStrength = bodyTakeover.jumpStrength;
        bodyTakeover.cineCam.enabled = true;

        currentBodyModel = bodyTakeover.bodyModel;
        bodyTakeover.isPossessed = true;
        currentBody = mainBody;
        currentBody.transform.position += Vector3.up * 3;

        isPossessing = false;
    }

}
