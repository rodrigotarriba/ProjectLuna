using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Added to each avatar - controls whether feet are solid against the ground to implement triggers and avoid impact giggling from controllers.
/// </summary>
public class LowerBodyGroundCheck : MonoBehaviour
{


    /// <summary>
    /// General foot offset to add more defintion manually through editor.
    /// </summary>
    [SerializeField] private Vector3 footOffset;

    /// <summary>
    /// Raycast to determine how far BELOW ground the foot kinematics must attempt to stick to the ground.
    /// </summary>
    [SerializeField] private Vector3 raycastThresholdBelowGround;

    /// <summary>
    /// Raycast to determine how far ABOVE ground the foot kinematics must attempt to stick to the ground.
    /// </summary>
    [SerializeField] private Vector3 raycastThresholdAboveGround;


    /// <summary>
    /// Layer that determines solid objects to be stand-on
    /// </summary>
    private int solidSurfaceLayer;

    /// <summary>
    /// Reference to avatar controller script for this player.
    /// </summary>
    private AvatarController avatarController;


    /// <summary>
    /// Reference to the player IKTargets
    /// </summary>
    [SerializeField] Transform leftFootIKTarget;
    [SerializeField] Transform rightFootIKTarget;

    //rtcClean - Spheres for testing debugging in 3D
    [SerializeField] private Transform sphereTransform2;
    [SerializeField] private Transform sphereTransform3;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private LineRenderer lineRenderer;
    //private Vector3[] pointsPositions = {};
    [SerializeField] Transform leftFootRotationReference;
    [SerializeField] Transform rightFootRotationReference;


    private void Awake()
    {
        solidSurfaceLayer = 1 << LayerMask.NameToLayer("SolidSurface");

    }

    /// <summary>
    /// Called evry time right before IKs are applied to animation update, 
    /// </summary>
    /// <param name="layerIndex"></param>
    //private void OnAnimatorIK(int layerIndex)
    //{
    //    FeetSolidCheck();
    //}

    public void Start()
    {
        avatarController = GetComponent<AvatarController>();
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        FeetPosRaycast();
    }

    public void FeetPosRaycast()
    {
        Vector3 leftFootPosition = leftFootIKTarget.position;
        Vector3 rightFootPosition = rightFootIKTarget.position;

        //Determnine if the raycast is touching the floor
        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;


        Vector3 leftFootRaycastStartPoint = leftFootPosition + raycastThresholdBelowGround + footOffset;
        Vector3 rightFootRaycastStartPoint = rightFootPosition + raycastThresholdBelowGround + footOffset;

        Vector3 leftFootRaycastEndPoint = leftFootPosition + raycastThresholdAboveGround + footOffset;
        Vector3 rightFootRaycastEndPoint = rightFootPosition + raycastThresholdAboveGround + footOffset;



        var pointsList = new List<Vector3> { leftFootRaycastStartPoint, leftFootRaycastEndPoint, rightFootRaycastEndPoint, rightFootRaycastStartPoint };

        lineRenderer.SetPositions(pointsList.ToArray());

        //Desired of the raycast
        float raycastMagnitude = Vector3.Distance(raycastThresholdAboveGround, raycastThresholdBelowGround);

        bool isLeftFootDown = Physics.Raycast(leftFootRaycastStartPoint, Vector3.up, out hitLeftFoot, raycastMagnitude, solidSurfaceLayer);
        bool isRightFootDown = Physics.Raycast(rightFootRaycastStartPoint + raycastThresholdBelowGround + footOffset, Vector3.up, out hitRightFoot, raycastMagnitude, solidSurfaceLayer);

        if (isRightFootDown)
        {
            Debug.Log("yes right foot its down");
            sphereTransform3.position = hitRightFoot.collider.transform.position;
            hitRightFoot.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        if (isLeftFootDown)
        {
            Debug.Log("yes left its down");
            sphereTransform2.position = hitLeftFoot.collider.transform.position;
            hitLeftFoot.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }


        GroundCheck(leftFootIKTarget, hitLeftFoot, isLeftFootDown, leftFootRotationReference);
        GroundCheck(rightFootIKTarget, hitRightFoot, isRightFootDown, rightFootRotationReference);
        

    }



    void GroundCheck(Transform ikTarget, RaycastHit hitFoot, bool isFootDown, Transform footRotationReference)
    {
        //Determine position and rotation of the foot
        if (isFootDown)
        {
            ikTarget.position = hitFoot.point + footOffset;
            ikTarget.rotation = footRotationReference.rotation;
        }

    }

}

