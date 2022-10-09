using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectReceiver : MonoBehaviour
{
    [SerializeField] OSC oscManager;
    [SerializeField] Transform[] cubes;
    [SerializeField] Vector3 posOffset;

    Vector3[] jointsPos;
    Vector3[] jointsRot;

    [SerializeField] Transform cubesParent;
    [SerializeField] Transform hipReference;
    private Vector3 referenceJoint;

    // Start is called before the first frame update
    public void Start()
    {
        //Initializing OSC handler

        oscManager.SetAllMessageHandler(OnReceiveJoints);
        //oscManager.SetAddressHandler("/Joints", OnReceiveJoints);
        //oscManager.SetAddressHandler("/Joints", OnReceiveJoints);

        //Establish two lists for rotation and position of each joint
        jointsPos = new Vector3[6];
        jointsRot = new Vector3[6];

}



    void Update()
    {
        //Get all the reference joints instanced in world coordinates
        if (jointsPos[0] == null) return;

        for (var i = 0; i < jointsPos.Length; i++)
        {
            cubes[i].SetPositionAndRotation(jointsPos[i], Quaternion.Euler(jointsRot[i]));

            cubes[i].position += posOffset;

        }

        //align reference joints with user
        var toMove = hipReference.position - jointsPos[0];
        cubesParent.position += toMove;
        cubesParent.forward = hipReference.forward;




    }

    public void OnReceiveJoints(OscMessage message)
    {
        if (message.address == "/tx")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsPos[i].x = message.GetFloat(i);
            }
        }

        if (message.address == "/ty")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsPos[i].y = message.GetFloat(i);
            }
        }

        if (message.address == "/tz")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsPos[i].z = message.GetFloat(i);
            }
        }

        if (message.address == "/rx")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsRot[i].x = message.GetFloat(i);
            }
        }

        if (message.address == "/ry")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsRot[i].y = message.GetFloat(i);
            }
        }

        if (message.address == "/rz")
        {
            for (var i = 0; i < jointsPos.Length; i++)
            {
                jointsRot[i].z = message.GetFloat(i);
            }
        }


    }
}





