using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTakeover : MonoBehaviour
{
    [SerializeField]
    private bool mainBody;
    public bool isPossesable = true;

    public Transform followTarget;
    public GameObject bodyModel;

    [Header("Movement stats")]
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;

    // Start is called before the first frame update
    void Start()
    {
        if (bodyModel == null)
            Debug.LogWarning(gameObject.name + " does not have an attached model in bodyModel and will not function properly");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
