using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(LineRenderer))]
public class GrappleGun : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] LayerMask grappleable;
    [SerializeField] Transform barrel;
    [SerializeField] Transform Camera;
    [SerializeField] Transform Player;
    [SerializeField] float maxDistance = 100f;
    RaycastHit hit;
    Ray ray;


    public bool grappling;
    Vector3 grapplePoint;
    SpringJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawRope();
    }

    public void Grapple(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance, grappleable))
            {
                grappling = true;
                grapplePoint = hit.point;
                joint = Player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;
                float distancefromPoint = Vector3.Distance(Player.position, grapplePoint);

                //Distance grapple will try to keep from grapple point
                joint.maxDistance = distancefromPoint * 0.4f;
                joint.minDistance = distancefromPoint * 0.25f;

                // change values to adjust gameplay
                joint.spring = 4.5f;
                joint.damper = 7f;
                joint.massScale = 4.5f;
            }
        }

        if(context.canceled)
        {
           // line.positionCount = 0;
            Destroy(joint);
        }
    }

    /// <summary>
    /// Call in other classes to cleanup the joint of the grapple gun
    /// </summary>
    public void DestroyJoint()
    {
        grappling = false;
        Destroy(joint);
    }

    void DrawRope()
    {
        if(!joint)
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            return;
        }
        line.SetPosition(0, barrel.position);
        line.SetPosition(1, grapplePoint);
    }
}
