using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightBuzz.Kinect4Azure;


public class TempAvatarKinect : MonoBehaviour
{
    [SerializeField] private Transform[] avatarJoints;
    private Dictionary<string, Transform> namedAvatarJoints;

    private LightBuzz.Kinect4Azure.Joint[] kinectJoints;
    //[SerializeField] KinectManager kinectManager;

    /// <summary>
    /// A dictionary representing the name of the avatar joint, and the best corresponding join in the kinect API.
    /// </summary>
    public Dictionary<string, JointType> kinectToAvatarMapping;

    public void Awake()
    {
        //Reference KinectManager
        //kinectManager = GetComponent<KinectManager>();

        //Map avatar joints to a dictionary by name
        //foreach (Transform avatarJoint in avatarJoints)
        //{
        //    namedAvatarJoints[avatarJoint.name] = avatarJoint;
        //}
    }

    public void LateUpdate()
    {
        //Guard clause, only update if sensor is able to export frames
        //Using singleton
        if (!KinectManager.kinectManager.isSensorReady) return;

        {
            //Collect the joints from the first body tracked by the Kinect API
            Dictionary<JointType, LightBuzz.Kinect4Azure.Joint> kinectJoints = KinectManager.kinectManager.kinectBodies[0].Joints;
            
            if (kinectJoints == null) return;
            mapKinectToAvatar();


        }
    }


    private void mapKinectToAvatar()
    {
        mapKinect("Hips", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);
        mapKinect("Spine", JointType.Pelvis);

    }



    private void mapKinect(string avatarJoint, JointType kinectJoint)
    {
        if (!KinectManager.kinectManager.isSensorReady) return;
        kinectToAvatarMapping[avatarJoint] = kinectJoint;
    }

}
