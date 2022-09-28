using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: https://www.youtube.com/watch?v=LqnPeqoJRFY video series has wall running in it
// TODO: mess with air drag/movment values get it how I want it, mess with wall jump force get it right where its needed, mess with grapple hook joint values get those just right, build level
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] WallRun wallRun;
    Vector2 horizontalInput;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    [SerializeField] float groundDrag = 6f; // drag for ground movement prevents slippery sliding
    [SerializeField] float airDrag = 2f; // drag when in the air to prevent slow falling

    
    Rigidbody rb;

    [SerializeField] float speed; // grounded movement speed
    [SerializeField] float movementMultiplier; // overcome drag
    [SerializeField] float airMultiplier; // account for less drag in the air
    [SerializeField] float jumpPower;

    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;

    RaycastHit hit;

    bool IsOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2 / 2 + 0.5f)) // raycast distance is player height / 2 + 0.5f can adjust for better detection
        {
            if(hit.normal != Vector3.up) // the object hit is not facing up sloped
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

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
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal); // if the player is on a slope make the movement vector perpendicular to slope for cleaner movement
    }

    private void FixedUpdate()
    {
        if(IsGrounded() && !IsOnSlope())
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Force);
        }
        else if(IsGrounded() && IsOnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * speed * movementMultiplier, ForceMode.Force);
        }
        else if(!IsGrounded())
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Force);
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
            if (context.performed && wallRun.wallRight == true || wallRun.wallLeft == true) // check if can walljump before exiting method
            {
                wallRun.WallJump();
            }
            return;
        }

        if(context.performed && wallRun.wallRight == false || wallRun.wallLeft == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reseting y velocity to fix minor bug for jumping, tiny jumps because it is fighting the downward velocity
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        }
    }

}
