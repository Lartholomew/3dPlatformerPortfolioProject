using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent(typeof(LineRenderer))]
public class GrappleGun : MonoBehaviour
{
    LineRenderer line;
    [SerializeField] LayerMask grappleable;
    [SerializeField] Image grappleTimer;
    [Header("Transforms")]
    [SerializeField] Transform barrel;
    [SerializeField] Transform Camera;
    [SerializeField] Transform Player;
    [Header("GrappleHookStats")]
    [SerializeField] float maxDistance = 100f;
    [Range(1f, 10f)]
    [SerializeField] float springValue = 4.5f;
    [Range(1f, 10f)]
    [SerializeField] float damperValue = 7f;
    [Range(1f, 10f)]
    [SerializeField] float massScaleValue = 4.5f;

    [SerializeField] float grappleGunDurration; // how long can the grapple gun grapple to an object

    RaycastHit hit;
    Ray ray;

    [HideInInspector]
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
                StartCoroutine(GrappleGunTimer());
                grapplePoint = hit.point;
                joint = Player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;
                float distancefromPoint = Vector3.Distance(Player.position, grapplePoint);

                //Distance grapple will try to keep from grapple point
                joint.maxDistance = distancefromPoint * 0.4f;
                joint.minDistance = distancefromPoint * 0.25f;

                // change values to adjust gameplay
                joint.spring = springValue; // strength of the spring
                joint.damper = damperValue; // how much force is absorbed by the spring higher value = more
                joint.massScale = massScaleValue; 
            }
        }

        if(context.canceled)
        {
           // line.positionCount = 0;
           StopCoroutine(GrappleGunTimer());
           Destroy(joint);
        }
    }

    /// <summary>
    /// Call in other classes to cleanup the joint of the grapple gun
    /// </summary>
    public void DestroyJoint()
    {
        grappling = false;
        grappleTimer.fillAmount = 1f;
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

    IEnumerator GrappleGunTimer()
    {
        float startTime = Time.time;
        while((Time.time - startTime) < grappleGunDurration)
        {
            grappleTimer.fillAmount = 1 - (Time.time - startTime) / grappleGunDurration;
            yield return null;
        }
        DestroyJoint();
    }
}
