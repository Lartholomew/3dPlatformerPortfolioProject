using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Vector2 horizontalMovement;
    Vector2 verticalMovement;
    Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float jumpPower;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckPos.position, 0.1f, groundLayer);
    }


    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(horizontalMovement.x * speed, rb.velocity.y, horizontalMovement.y * speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {
        if(!IsGrounded()) //  one check simplyifying if statements keeping from inbeding them, if not grounded dont jump
        {
            return;
        }

        if(context.performed)
        {
            verticalMovement.y = jumpPower;
            rb.velocity = new Vector3(horizontalMovement.x, verticalMovement.y, horizontalMovement.y);
        }
    }

}
