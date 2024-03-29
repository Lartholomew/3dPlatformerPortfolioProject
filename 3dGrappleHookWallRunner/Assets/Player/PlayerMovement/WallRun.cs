using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class WallRun : MonoBehaviour
{
    Transform orientation; // player transform


    [Header("ScriptDependencies")]
    [SerializeField] PlayerStats playerStats;

    bool wallRunning;

    [HideInInspector]
    public bool wallLeft = false;
    [HideInInspector]
    public bool wallRight = false;

    Vector3 previousWallVelocity;

    Rigidbody rb;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    [SerializeField] LayerMask wallLayer;
    [Header("Wall Run Camera Settings")]
    [SerializeField] Camera cam;
    float defaultFov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        orientation = GetComponent<Transform>();
        defaultFov = cam.fieldOfView;
    }
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, playerStats.minimumJumpHeight); // returns opposite of what it recieves 
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallHit , playerStats.wallDistance, wallLayer);
        if(wallLeft == true) // if wallleft is true no need to check wall right
        {
            return;
        }
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, playerStats.wallDistance, wallLayer);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            if(wallRunning)
            {
                Vector3 prejVel = Vector3.Cross(previousWallVelocity, collision.gameObject.transform.up);
                Debug.Log(prejVel);
                /*
                if(prejVel != Vector3.zero)
                {
                    rb.velocity = prejVel;
                }
                */
            }
            // TODO: transfer velocity when jumping to a perpendicular wall
        }    
    }

    // Update is called once per frame
    void Update()
    {
        CheckWall();

        if(CanWallRun())
        {
            if(wallLeft)
            {
                StartWallRun();
            }
            else if(wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }

        if(playerStats.onGround)
            previousWallVelocity = Vector3.zero;

    }

    void StartWallRun()
    {
        wallRunning = true;
        rb.useGravity = false;
        rb.AddForce(Vector3.down * playerStats.wallRunGrav, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
    }

    public void WallJump() // called in the playermovement script to handle wall jumping in the Jump method
    { 
        // checking if there is a wall jumping off of is on the right or left
        if (wallLeft)
        {
            Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal; // calculating what direction to add force adding up
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // resetting the players y velocity
            rb.AddForce(wallRunJumpDirection * playerStats.wallJumpForce, ForceMode.Force); // adding force to the player
        }
        // same process as on the left
        else if (wallRight)
        {
            Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * playerStats.wallJumpForce, ForceMode.Force);          
        }
        previousWallVelocity = rb.velocity;
    }

    void StopWallRun()
    {
        wallRunning = false;
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
