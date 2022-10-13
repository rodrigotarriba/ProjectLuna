using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detection of foot mapping towards IK - this was one of the biggest challenges of this delivery, how to detect feet easily and without lag. Ultimately, this follows the controllers attached close to the player's feet. 
/// However, the goal is to finally implemente Kinect or ZED, or other sensor usage, unforatunately the lag was too strong no matter what I did. Will have to create a wrapper for their SDK later on for implementation.
/// </summary>
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



