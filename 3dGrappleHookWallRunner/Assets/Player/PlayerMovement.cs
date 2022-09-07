using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: https://www.youtube.com/watch?v=LqnPeqoJRFY video series has wall running in it
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Vector2 horizontalInput;
    Vector3 moveDirection;

    [SerializeField] float groundDrag = 6f; // drag for ground movement prevents slippery sliding
    [SerializeField] float airDrag = 2f; // drag when in the air to prevent slow falling

    
    Rigidbody rb;

    [SerializeField] float speed; // grounded movement speed
    [SerializeField] float movementMultiplier; // overcome drag
    [SerializeField] float airMultiplier; // account for less drag in the air
    [SerializeField] float jumpPower;

    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetDrag(groundDrag);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckPos.position, 0.1f, groundLayer);
    }

    void SetDrag(float drag)
    {
        rb.drag = drag;
    }


    // Update is called once per frame
    void Update()
    {
        if(IsGrounded())
        {
            SetDrag(groundDrag);
        }
        else
        {
            SetDrag(airDrag);
        }
        moveDirection = transform.forward * horizontalInput.y + transform.right * horizontalInput.x;
        Debug.Log(rb.drag);
    }

    private void FixedUpdate()
    {
        if(IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Acceleration);
            Debug.Log("grounded move speed");
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            Debug.Log("air move speed");
        }
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();       
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if(!IsGrounded()) //  one check simplyifying if statements keeping from inbeding them, if not grounded dont jump
        {
            return;
        }

        if(context.performed)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }
    }

}
