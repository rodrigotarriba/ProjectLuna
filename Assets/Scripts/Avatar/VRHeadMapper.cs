using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


