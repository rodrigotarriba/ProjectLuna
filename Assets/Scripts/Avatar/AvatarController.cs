using System.Collections;
using System.Collections.Generic;
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


    [SerializeField] Transform sphere;

    Transform ball1;
    Transform ball2;
    Transform ball3;
    Transform ball4;
    Transform ball5;
    Transform ball6;
    Transform ball7;

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

    [SerializeField] Rigidbody xrOriginRigidBody;
    [SerializeField] Transform xrOriginTransform;
    [SerializeField] bool useGravity;


    private void Awake()
    {
        //Assign layer for pads and ground
        solidSurfaceLayer = 1 << LayerMask.NameToLayer("SolidSurface"); //layer for collisions with solid surfaces

    }


    private void Start()
    {
        ball1 = Instantiate(sphere);
        ball2 = Instantiate(sphere);
        ball3 = Instantiate(sphere);
        ball4 = Instantiate(sphere);
        ball5 = Instantiate(sphere);
        ball6 = Instantiate(sphere);
        ball7 = Instantiate(sphere);
    }

    public void LateUpdate()
    {
        //Made in late update to avoid jiggering from fighting IK transformations
        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);  // We only want the Y axis rotation, instead of veering sides or tilting the head weirdly. - We add a lerp from the previous forward position to allow a level of smoothness and no immediate jitter.

        //Map HMD and feet
        headVR.Mapping(); 
        var leftFoot = leftFootVR.Mapping();
        var rightFoot = rightFootVR.Mapping();


        if(!GroundHitDetection(leftFoot) && !GroundHitDetection(rightFoot) && useGravity)
        {
            xrOriginRigidBody.isKinematic = false;
            xrOriginRigidBody.useGravity = true;
        }

    }



    /// <summary>
    /// Detect ground, hoverpads and/or clash with death zone (falling)
    /// </summary>
    /// <param name="foot"></param>
    public bool GroundHitDetection(Transform foot)
    {
        RaycastHit footHit;

        //Desired detection thresehold for sticking foot into ground/solid
        float raycastMagnitude = Vector3.Distance(raycastThresholdAboveGround, raycastThresholdBelowGround);

        bool isFootDown = Physics.Raycast(foot.position - anklesHeight + raycastThresholdBelowGround, Vector3.up, out footHit, raycastMagnitude, solidSurfaceLayer);

        if (isFootDown)
        {
            xrOriginRigidBody.isKinematic = true;
            xrOriginRigidBody.useGravity = false;

            if (foot.name == "LeftFootTarget") PadsManager.padsManager.onPadHit(footHit.collider, "Left");
            if (foot.name == "RightFootTarget") PadsManager.padsManager.onPadHit(footHit.collider, "Right");


            //Adjust player height as standing in the platform they just collided with
            //xrOriginTransform.Translate(0f, footHit.point.y - xrOriginTransform.position.y + anklesHeight.y, 0f);

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
