using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    float camRotX;
    public float camRotY;
    public float sensitivity = 1f;

    PlayerInput plControls;

    [Header("Virtual Camera settings")]
    public float armDist;

    private void Start()
    {
        plControls = FindObjectOfType<PlayerInput>();
    }

    private void FixedUpdate()
    {
        Vector2 mouseMovement = plControls.actions.FindAction("look").ReadValue<Vector2>() * sensitivity;
        camRotX += mouseMovement.y;
        camRotY += mouseMovement.x;
        camRotX = Mathf.Clamp(camRotX, -85, 85);
        gameObject.transform.localRotation = Quaternion.Euler(-camRotX,camRotY, 0);

        //gameObject.transform.Rotate(0, mouseMovement.y, 0);
    }
}
