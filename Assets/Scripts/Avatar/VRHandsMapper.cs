using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pending final implementation to kinect synchronization - solving lag issues.
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
