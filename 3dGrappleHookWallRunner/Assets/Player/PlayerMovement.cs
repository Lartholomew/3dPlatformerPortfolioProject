using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// TODO: create more organization for the variables in the inspector
// TODO: mess with air drag/movment values get it how I want it, mess with wall jump force get it right where its needed, mess with grapple hook joint values get those just right, build level
[RequireComponent(typeof(Rigidbody),typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    

    [Header("Script Dependencies")]
    [SerializeField] WallRun wallRun; // for accessing wall run methods 
    [SerializeField] GrappleGun grappleGun; // reference to grapple gun for destroying the grapple point on respawn
    [SerializeField] PlayerStats playerStats;


    Vector2 horizontalInput;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    
    Rigidbody rb;
    
    [Header("Transforms/Layer settings")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform respawnPoint; // empty object in the scene("RespawnPoint") is the respawn point can adjust to create checkpoint system


    PlayerInput controls; // reference of the player input for disabling when pausing and when a level is complete
    
    RaycastHit hit;

    private void OnEnable()
    {
        FinishLine.OnFinishLinePassed += DisableControls;
    }

    private void OnDisable()
    {
        FinishLine.OnFinishLinePassed -= DisableControls;   
    }

    // Start is called before the first frame update
    void Start()
    {
        controls = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        controls.enabled = true; // dumb proofing, making sure the inputs are always enabled at start
        SetRbDrag(playerStats.groundDrag);
    }

    bool IsOnSlope()

    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2 / 2 + 0.5f)) // raycast distance is player height / 2 + 0.5f can adjust for better detection
        {
            if (hit.normal != Vector3.up) // the object hit is not facing up sloped
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

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheckPos.position, 0.1f, groundLayer);
    }

    void SetRbDrag(float drag)
    {
        rb.drag = drag;
    }


    // Update is called once per frame
    void Update()
    {
        if(IsGrounded())
        {
            SetRbDrag(playerStats.groundDrag);
        }
        else
        {
            SetRbDrag(playerStats.airDrag);
            if(transform.position.y < -10)
            {
                Respawn();
            }
        }
        moveDirection = transform.forward * horizontalInput.y + transform.right * horizontalInput.x; // calculating the direction the player should move in adds the transform forward and right, inputs will either be 0 or 1
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal); // if the player is on a slope make the movement vector perpendicular to slope for cleaner movement
    }

    private void FixedUpdate()
    {
        
        if(IsGrounded() && !IsOnSlope())
        {
           // rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Force);
           rb.AddForce(Acceleration(moveDirection.normalized) * playerStats.runTimeSpeed * playerStats.movementMultiplier, ForceMode.Force);
        }
        else if(IsGrounded() && IsOnSlope())
        {
           // rb.AddForce(slopeMoveDirection.normalized * speed * movementMultiplier, ForceMode.Force);
           rb.AddForce(Acceleration(slopeMoveDirection.normalized) * playerStats.runTimeSpeed * playerStats.movementMultiplier, ForceMode.Force);
        }
        else if(!IsGrounded())
        {
           // rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Force);
           rb.AddForce(Acceleration(moveDirection.normalized) * playerStats.runTimeSpeed * playerStats.movementMultiplier * playerStats.airMultiplier, ForceMode.Force);
        }
        
        
    }

    Vector3 Acceleration(Vector3 prevVelocity)
    {
        float projVel = Vector3.Dot(prevVelocity, transform.forward); // the projected velocity of the player
        if(projVel == 0) // checking if there is no motion
            return prevVelocity;

        float accelVel = playerStats.speed * Time.fixedDeltaTime; // the acceleration of the player

        if (projVel + accelVel > playerStats.maxAcceleration) // if the projected velocity plus the acceleration velocity is greater than max acceleration
            accelVel = playerStats.maxAcceleration - projVel; // capping the acceleration velocity by the max acceleration minus the projected velocity (capping max player speed)

        return prevVelocity + transform.forward * accelVel;
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();
    }


    public void Jump(InputAction.CallbackContext context)
    {
        // TODO: think about cleaning these nested if statements up create guard clauses
        if(context.performed)
        {
            if (!IsGrounded()) //  one check simplyifying if statements keeping from inbeding them, if not grounded dont jump
            {
                if (wallRun.wallRight == true || wallRun.wallLeft == true) // check if can walljump before exiting method
                { 
                    wallRun.WallJump();
                }
                return;
            }

            if (wallRun.wallRight == false || wallRun.wallLeft == false)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reseting y velocity to fix minor bug for jumping, tiny jumps because it is fighting the downward velocity
                rb.AddForce(transform.up * playerStats.jumpPower, ForceMode.Impulse);
            }   
        }
      
    }

    void Respawn()
    {
        grappleGun.DestroyJoint(); // remove the grapple gun joint 
        rb.velocity = Vector3.zero; // set players velocity to 0
        transform.position = respawnPoint.position; // set players position to the respawn point position
    }

    public void DisableControls()
    {
        controls.enabled = false;
    }

}
