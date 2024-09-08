using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDetection : MonoBehaviour
{

    private PlayerController plControl;

    private void Start()
    {
        plControl = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            plControl.AddBody(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            plControl.RemoveBody(other.gameObject);
        }
    }
}
