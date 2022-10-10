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

    [SerializeField] Transform hipReference;


    [SerializeField] Transform kinectLeftAnkle;
    [SerializeField] Transform kinectRightAnkle;
    [SerializeField] Transform kinectPelvis;



    private Vector3 referenceJoint;

    // Start is called before the first frame update
    public void Start()
    {
        //Initializing OSC handler

        oscManager.SetAllMessageHandler(OnReceiveJoints);
        //oscManager.SetAddressHandler("/Joints", OnReceiveJoints);

        //Establish two lists for rotation and position of each joint
        jointsPos = new Vector3[6];
        jointsRot = new Vector3[6];

}



    void Update()
    {
        //Get all the reference joints instanced in world coordinates
        if (jointsPos[0] == null) return;



        AlignTransforms(kinectPelvis, jointsPos[0], jointsRot[0]);
        AlignTransforms(kinectLeftAnkle, jointsPos[1], jointsRot[1]);
        AlignTransforms(kinectRightAnkle, jointsPos[2], jointsRot[2]);

        //align reference joints with user
        //var toMove = hipReference.position - jointsPos[0];
        //cubesParent.position += toMove;
        //cubesParent.forward = hipReference.forward;

    }


    public void AlignTransforms(Transform jointGameObject, Vector3 position, Vector3 rotation)
    {
        jointGameObject.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));

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





