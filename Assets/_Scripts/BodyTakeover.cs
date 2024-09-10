using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTakeover : MonoBehaviour
{
    [SerializeField]
    private bool mainBody;
    public bool isPossesable = true;

    public Transform followTarget;

    [Header("Movement stats")]
    public float maxSpeed;
    public float acceleration;
    public float jumpStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
