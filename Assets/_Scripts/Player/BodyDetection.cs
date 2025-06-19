using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            PlayerController.Instance.AddBodyInRange(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BodyTakeover>() != null)
        {
            PlayerController.Instance.RemoveBodyInRange(other.gameObject);
        }
    }
}
