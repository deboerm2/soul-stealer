using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Cinemachine;

public class BodyTakeover : MonoBehaviour
{
    [SerializeField]
    private bool mainBody;
    public bool isPossesable;
    [Tooltip("the amount of soul energy needed to possess this body")]
    public float soulNeeded;
    private SoulEnergy soulEnergy;
    [HideInInspector]
    public bool isPossessed;
    //^^used to turn off AI control
    private Enemy enemyScript;

    [Tooltip("A transform used by the cinemachine third-person camera to follow this object")]
    public Transform followTarget;
    public CinemachineVirtualCamera cineCam;
    public GameObject bodyModel;
    [SerializeField]
    private Animator bodyAnimator;
    //[HideInInspector]
    public bool acceptAttackInputs = true;
    public bool restrictMovement = false;

    public float currentTimeScale { get; private set; } = 1;

    [Header("Body Movement stats")]
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;

    [Header("Body Attacks")]
    public GameObject attackGO;
    public GameObject altAttackGO;



    // Start is called before the first frame update
    void Start()
    {
        soulEnergy = FindObjectOfType<SoulEnergy>();
        if (bodyModel == null)
        {
            Debug.LogWarning(gameObject.name + " does not have an attached model in bodyModel and will not function properly");
        }
        bodyAnimator = gameObject.GetComponentInChildren<Animator>();
        acceptAttackInputs = true;
        if (!mainBody)
        {
            if (gameObject.GetComponent<Enemy>() == null)
                Debug.LogWarning(gameObject.name + "is marked as mainBody");
            else
                enemyScript = gameObject.GetComponent<Enemy>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isPossesable = soulEnergy.currentEnergy >= soulNeeded ? true : false;

        this.tag = isPossessed ? "Player" : "Enemy";

        bodyAnimator.SetFloat("SpeedMultiplier", currentTimeScale);
    }

    //used to allow for player to attack with possessed enemy
    public void BodyAttack()
    {
        if (mainBody)
        {
            Instantiate(attackGO, bodyModel.transform);
            acceptAttackInputs = false;
        }
        else
            enemyScript.Attack();
    }

    public void AltAttack()
    {
        if (mainBody)
        {
            Instantiate(altAttackGO, bodyModel.transform);
            acceptAttackInputs = false;
        }
        else
            enemyScript.Attack();
    }

    //called to allow other scripts to change animator params without needing a reference to the animator in the other script
    public void SetAnimatorParam(string paramName, bool isTrue)
    {
        foreach (AnimatorControllerParameter param in bodyAnimator.parameters)
        {
            if (param.name == paramName)
                bodyAnimator.SetBool(paramName, isTrue);
        }

    }

    public void SlowDown(float slowAmount)
    {
        currentTimeScale = slowAmount;
    }
    public void ReturnNormalSpeed()
    {
        currentTimeScale = 1;
    }
}
