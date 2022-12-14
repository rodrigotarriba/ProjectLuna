using UnityEngine;



/// <summary>
/// Maps an IK target into the position and rotation of a VR target.
/// </summary>
[System.Serializable]
public class MapTransforms
{
    //A couple of variables to holding hands and things like that.
    //Transform for the vr headset,. 

    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void VRMapping()
    {
        //Map position and rotation of the given vectors
        ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }


}


/// <summary>
/// Manager for all mapping and transformation of the Controllers + Camera to the Inversed Kinematic elements.
/// </summary>
public class AvatarController : MonoBehaviour
{
    [SerializeField] private MapTransforms head;
    [SerializeField] private MapTransforms leftHand;
    [SerializeField] private MapTransforms rightHand;
    [SerializeField] private float rightFootHeight;
    //[SerializeField] private Transform rightFootTarget;
    //[SerializeField] private Transform kinectSphereRightFootTracker;

    [SerializeField] private float turnSmoothness;

    [SerializeField] Transform ikHead;
    [SerializeField] Vector3 headBodyOffset;//height of character/person

    //[SerializeField] KinectManager kinectManager;

    private void LateUpdate()
    {
        //Made in late update to avoid jigger

        transform.position = ikHead.position + headBodyOffset;
        //transform.forward = Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);  // We only want the Y axis rotation, instead of veering sides or tilting the head weirdly. - We add a lerp from the previous forward position to allow a level of smoothness and no immediate jitter.


        //if(KinectManager.kinectManager.isSensorReady) rightFootHeight = kinectManager.kinectBodies[0].Joints[LightBuzz.Kinect4Azure.JointType.AnkleRight].Position.Y;
        //kinectSphereRightFootTracker.position = kinectManager.kinectBodies[0].Joints[LightBuzz.Kinect4Azure.JointType.AnkleRight].Position;


        //Debug.Log($"{rightFootHeight}");
        //string script1 = $"Right foot target is at {rightFootTarget.position}";
        //rightFootTarget.position = new Vector3(rightFootTarget.position.x, rightFootHeight, rightFootTarget.position.z);
        //Debug.Log($"{script1} and now its at {rightFootTarget.position}");


        head.VRMapping();
        leftHand.VRMapping();
        rightHand.VRMapping();

        


    }



}
