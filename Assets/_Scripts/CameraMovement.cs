using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float camRotX;
    public float camRotY;

    public float sensitivity = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 mouseMovement = new Vector2(-Input.GetAxis("Mouse Y") * sensitivity, Input.GetAxis("Mouse X") * sensitivity);
        camRotX -= mouseMovement.x;
        camRotY += mouseMovement.y;
        camRotX = Mathf.Clamp(camRotX, -85, 85);
        gameObject.transform.localRotation = Quaternion.Euler(-camRotX,0, 0);

        //gameObject.transform.Rotate(0, mouseMovement.y, 0);
    }
}
