using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BodyTakeover : MonoBehaviour
{
    [SerializeField]
    private bool mainBody;
    public bool isPossesable = true;
    [HideInInspector]
    public bool isPossessed;
    //^^used to turn off AI control

    [Tooltip("A transform used by the cinemachine third-person camera to follow this object")]
    public Transform followTarget;
    public GameObject bodyModel;
    [SerializeField]
    private Animator bodyAnimator;
    //[HideInInspector]
    public bool acceptAttackInputs = true;


    [Header("Movement stats")]
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;



    // Start is called before the first frame update
    void Start()
    {
        if (bodyModel == null)
        {
            Debug.LogWarning(gameObject.name + " does not have an attached model in bodyModel and will not function properly");
        }
        bodyAnimator = gameObject.GetComponentInChildren<Animator>();
        acceptAttackInputs = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //will be used to turn on/off ai control of the animator
    public void PlayerControlsAnimator(bool isPlayerControlled)
    {
        foreach(AnimatorControllerParameter parameter in bodyAnimator.parameters)
        {
            bodyAnimator.SetBool(parameter.name, false);
        }
    }

    //called to allow other scripts to change animator params without needing a reference to the animator in the other script
    public void SetAnimatorParam(string paramName, bool isTrue)
    {
        bodyAnimator.SetBool(paramName, isTrue);
    }

}
