using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float camRotX;
    public float camRotY;
    public float sensitivity = 1f;

    [Header("Virtual Camera settings")]
    public float armDist;

    private void FixedUpdate()
    {
        Vector2 mouseMovement = new Vector2(-Input.GetAxis("Mouse Y") * sensitivity, Input.GetAxis("Mouse X") * sensitivity);
        camRotX -= mouseMovement.x;
        camRotY += mouseMovement.y;
        camRotX = Mathf.Clamp(camRotX, -85, 85);
        gameObject.transform.localRotation = Quaternion.Euler(-camRotX,camRotY, 0);

        //gameObject.transform.Rotate(0, mouseMovement.y, 0);
    }
}
