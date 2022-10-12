using UnityEngine;

/// <summary>
/// Manager for all mapping and transformation of the Controllers + Camera to the Inversed Kinematic elements.
/// </summary>
public class AvatarController : MonoBehaviour
{
    [SerializeField] public VRMapping head;
    [SerializeField] public VRMapping leftHand;
    [SerializeField] public VRMapping rightHand;
    [SerializeField] public VRMapping leftFoot;
    [SerializeField] public VRMapping rightFoot;

    [SerializeField] private float turnSmoothness;

    [SerializeField] Transform ikHead;
    [SerializeField] Vector3 headBodyOffset;//height of character/person

    [SerializeField] private LowerBodyGroundCheck lowerBodyGroundCheck;

    //Only if using kinect locally
    //[SerializeField] private float rightFootHeight;
    //[SerializeField] private Transform rightFootTarget;
    //[SerializeField] private Transform kinectSphereRightFootTracker;
    //[SerializeField] KinectManager kinectManager;


    private void Awake()
    {
        lowerBodyGroundCheck = GetComponent<LowerBodyGroundCheck>();
    }


    private void Update()
    {
        //Made in late update to avoid jigger

        transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);  // We only want the Y axis rotation, instead of veering sides or tilting the head weirdly. - We add a lerp from the previous forward position to allow a level of smoothness and no immediate jitter.

        head.Mapping();
        leftFoot.Mapping();
        rightFoot.Mapping();




    }
}



/// <summary>
/// Maps an IK target into the position and rotation of a VR target.
/// </summary>
[System.Serializable]
public class VRMapping : IMapper
{
    //A couple of variables to holding hands and things like that.
    //Transform for the vr headset,. 

    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;



    public void Mapping()
    {
        //Map position and rotation of the given vectors
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}


[System.Serializable]
public class KinectMapping : IMapper
{
    public Transform kinectTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;


    public void Mapping()
    {
        Vector3 currentPos = ikTarget.position;

        currentPos.y = kinectTarget.position.y;

        ikTarget.position = currentPos;
    }
}


public interface IMapper
{
    public void Mapping();
}
