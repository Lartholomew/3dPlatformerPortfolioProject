using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Vector2 horizontalMovement;
    Vector3 horizontalVelocity;
    Rigidbody rb;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(horizontalMovement.x * speed, rb.velocity.y, horizontalMovement.y * speed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>();
        Debug.Log(horizontalMovement);
    }

}
