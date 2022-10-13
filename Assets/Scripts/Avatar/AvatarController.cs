using UnityEngine;

/// <summary>
/// Manager for all mapping and transformation of the Controllers + Camera to the Inversed Kinematic elements.
/// </summary>
public class AvatarController : MonoBehaviour
{
    [SerializeField] public VRHeadMapper headVR;
    [SerializeField] public VRHandMapper leftHand;
    [SerializeField] public VRHandMapper rightHand;
    [SerializeField] public VRFootMapper leftFootVR;
    [SerializeField] public VRFootMapper rightFootVR;

    [SerializeField] private float turnSmoothness;

    [SerializeField] Transform ikHead;
    [SerializeField] Vector3 headBodyOffset;//height of character/person
    [SerializeField] Vector3 anklesHeight;//height of character/person



    //Raycast to determine how far BELOW ground the foot kinematics must attempt to stick to the ground.
    [SerializeField] private Vector3 raycastThresholdBelowGround;


    //Raycast to determine how far ABOVE ground the foot kinematics must attempt to stick to the ground.
    [SerializeField] private Vector3 raycastThresholdAboveGround;



    //Layer that determines solid objects to be stand-on
    private int solidSurfaceLayer;

    //Reference to avatar controller script for this player.
    private AvatarController avatarController;


    private void Awake()
    {
        solidSurfaceLayer = 1 << LayerMask.NameToLayer("SolidSurface"); //layer for collisions with solid surfaces
        avatarController = GetComponent<AvatarController>();
    }

    public void LateUpdate()
    {
        //Made in late update to avoid jigger
        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);  // We only want the Y axis rotation, instead of veering sides or tilting the head weirdly. - We add a lerp from the previous forward position to allow a level of smoothness and no immediate jitter.

        headVR.Mapping(); 
        var leftFoot = leftFootVR.Mapping();
        var rightFoot = rightFootVR.Mapping();

        GroundHitDetection(leftFoot);
        GroundHitDetection(rightFoot);
    }



    //Detect ground, hoverpads and/or clash with death zone (falling)
    public void GroundHitDetection(Transform foot)
    {
        RaycastHit footHit;

        //Desired thresehold for sticking foot into ground/solid
        float raycastMagnitude = Vector3.Distance(raycastThresholdAboveGround, raycastThresholdBelowGround);

        bool isFootDown = Physics.Raycast(foot.position - anklesHeight + raycastThresholdBelowGround, Vector3.up, out footHit, raycastMagnitude, solidSurfaceLayer);

        if (isFootDown)
        {
            if (foot.name == "LeftFootTarget") PadsManager.padsManager.onPadHit(footHit.collider, "Left");
            if (foot.name == "RightFootTarget") PadsManager.padsManager.onPadHit(footHit.collider, "Right");
        }
    }
}



