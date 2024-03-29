using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseLook : MonoBehaviour
{
    [SerializeField] WallRun wallRun;
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX;
    float mouseY;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        targetRotation.z = wallRun.tilt;
        playerCamera.eulerAngles = targetRotation;
    }

    public void RecieveMouseX(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mouseX = context.ReadValue<float>() * sensitivityX;
        }
        else if (context.canceled)
            mouseX = 0;
    }

    public void RecieveMouseY(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            mouseY = context.ReadValue<float>() * sensitivityY;
        }
        else if(context.canceled)
            mouseY = 0;
    }
}
