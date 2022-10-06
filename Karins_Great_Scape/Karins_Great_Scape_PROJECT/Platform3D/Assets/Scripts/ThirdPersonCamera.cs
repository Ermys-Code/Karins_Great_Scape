using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform lookAt, camTransform;
    public LayerMask notDoorMask;
    public bool camPaused;
    public float sensitivityX;
    public float sensitivityY;
    
    const float Y_ANGLE_MIN = -12.0f;
    const float Y_ANGLE_MAX = 50.0f;

    Camera cam;
    RaycastHit hitWalls;

    float distance = 3.0f;
    float currentX = 0.0f;
    float currentY = 0.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        sensitivityX = SettingsValues.sensitivityX;
        sensitivityY = SettingsValues.sensitivityY;

        if(camPaused) Cursor.lockState = CursorLockMode.None;
        else
        {   
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            if (Input.GetJoystickNames().Length > 0)
            {
                currentX += Input.GetAxis("Vertical Axis") * sensitivityX;
                currentY += Input.GetAxis("Horizontal Axis") * sensitivityY;
            }

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            if (Input.GetKey(KeyCode.LeftAlt)) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);

        if (Physics.Linecast(lookAt.position, camTransform.position, out hitWalls, notDoorMask))
        {
            camTransform.position = hitWalls.point;
        }
        else
        {
            camTransform.position = camTransform.position;
        }
    }
}
