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

    private Transform leftFootIK;
    private Transform rightFootIK;

    [SerializeField] Transform sphere;


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

    [SerializeField] 

    private void Awake()
    {
        solidSurfaceLayer = 1 << LayerMask.NameToLayer("SolidSurface"); //layer for collisions with solid surfaces
        avatarController = GetComponent<AvatarController>();


    }

    private void Update()
    {
        //Made in late update to avoid jigger

        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);  // We only want the Y axis rotation, instead of veering sides or tilting the head weirdly. - We add a lerp from the previous forward position to allow a level of smoothness and no immediate jitter.

        headVR.Mapping(); 
        leftFootVR.Mapping();
        rightFootVR.Mapping();

        //Pending implementation of hand tracing through Kinect
        //leftHand.Mapping();
        //rightHand.Mapping();

        RaycastHit leftFootHit;
        RaycastHit rightFootHit;

        if (GroundHitDetection(leftFootVR, out leftFootHit))
        { 
            leftFootHit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        if (GroundHitDetection(rightFootVR, out rightFootHit))
        {
            sphere.position = rightFootHit.point;
            rightFootHit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
        }

    }



    public bool GroundHitDetection(VRFootMapper footMapper, out RaycastHit footHit)
    {
        //Desired thresehold for sticking foot into ground/solid
        float raycastMagnitude = Vector3.Distance(raycastThresholdAboveGround, raycastThresholdBelowGround);

        bool isFootDown = Physics.Raycast(footMapper.ikTarget.position - anklesHeight + raycastThresholdBelowGround, Vector3.up, out footHit, raycastMagnitude, solidSurfaceLayer);

        if (isFootDown)
        {
            footMapper.GroundRemapping(footHit.point);
        }

        return isFootDown;
    }

}


/// <summary>
/// Maps an IK target into the position and rotation of a VR target.
/// </summary>
[System.Serializable]
public class VRHeadMapper : IMapper
{
    //A couple of variables to holding hands and things like that.
    //Transform for the vr headset,. 

    [SerializeField] public Transform vrTarget;
    [SerializeField] public Transform ikTarget;
    [SerializeField] public Vector3 trackingPositionOffset;
    [SerializeField] public Vector3 trackingRotationOffset;


    public Transform Mapping()
    {
        
        //Map position and rotation of the given vectors
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        return ikTarget;

    }

}

[System.Serializable]
public class VRFootMapper : IMapper
{
    //A couple of variables to holding hands and things like that.
    //Transform for the vr headset,. 

    [SerializeField] public Transform vrTarget;
    [SerializeField] public Transform ikTarget;
    [SerializeField] public Vector3 trackingPositionOffset;
    [SerializeField] public Vector3 trackingRotationOffset;
    [SerializeField] public Transform footVRRotationReference;

    public Transform Mapping()
    {
        //Map position and rotation of the given vectors
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        return ikTarget;
    }

    public void GroundRemapping(Vector3 groundHit)
    {
        float yRotation = ikTarget.localRotation.eulerAngles.y;
        ikTarget.rotation = footVRRotationReference.rotation * Quaternion.Euler(trackingRotationOffset);
        ikTarget.localRotation = Quaternion.Euler(ikTarget.localRotation.eulerAngles.x, yRotation + 10f, ikTarget.localRotation.eulerAngles.z);

    }




}



//Pending implementation to kinect synchronizatoin
[System.Serializable]
public class VRHandMapper : IMapper
{
    public Transform kinectTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;


    public Transform Mapping()
    {
        Vector3 currentPos = ikTarget.position;
        currentPos.y = kinectTarget.position.y;
        ikTarget.position = currentPos;
        return ikTarget;
    }
}



public interface IMapper
{
    public Transform Mapping();
}
