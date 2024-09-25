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

    public Transform followTarget;
    public GameObject bodyModel;
    [SerializeField]
    private Animator bodyAnimator;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerControlsAnimator(bool isPlayerControlled)
    {
        foreach(AnimatorControllerParameter parameter in bodyAnimator.parameters)
        {
            bodyAnimator.SetBool(parameter.name, false);
        }
    }

    public void SetAnimatorParam(string paramName, bool isTrue)
    {
        bodyAnimator.SetBool(paramName, isTrue);
    }

}
