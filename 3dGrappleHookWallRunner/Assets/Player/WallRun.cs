using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [SerializeField] float wallRunGrav;
    [SerializeField] float wallJumpForce;

    public bool wallLeft = false;
    public bool wallRight = false;



    Rigidbody rb;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [SerializeField] Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight); // returns opposite of what it recieves 
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right,out leftWallHit ,wallDistance);
        if(wallLeft == true) // if wallleft is true no need to check wall right
        {
            return;
        }
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
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


    }

    void StartWallRun()
    {
        rb.useGravity = false;
      //  rb.AddForce(Vector3.down * wallRunGrav, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
    }

    public void WallJump() // called in the playermovemtn script to handle wall jumping
    {
        Debug.Log("wall jump");
        if (wallLeft)
        {
            Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * wallJumpForce, ForceMode.Force);
        }
        else if (wallRight)
        {
            Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * wallJumpForce, ForceMode.Force);
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
