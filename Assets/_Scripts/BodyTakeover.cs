using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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
    public GameObject bodyModel;
    [SerializeField]
    private Animator bodyAnimator;
    //[HideInInspector]
    public bool acceptAttackInputs = true;

    private float currentTimeScale = 1f;

    [Header("Movement stats")]
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;



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
    }

    //used to allow for player to attack with possessed enemy
    public void BodyAttack()
    {
        if (mainBody)
        {
            SetAnimatorParam("Light", true);
            SetAnimatorParam("inCombo", true);
            acceptAttackInputs = false;
        }
        else
            enemyScript.Attack();
    }

    public void AltAttack()
    {
        if (mainBody)
        {
            SetAnimatorParam("Heavy", true);
            SetAnimatorParam("inCombo", true);
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
}
