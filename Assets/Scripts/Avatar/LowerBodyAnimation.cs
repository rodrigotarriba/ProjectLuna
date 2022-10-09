using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Added to each avatar - controls passive vs. active character movement, allows to touch the ground.
/// </summary>
public class LowerBodyAnimation : MonoBehaviour
{
    //Tell how much of the animation we want, or from the inverse kinematics we really want. 
    //If there is a mazimum thereshold

    //We need to access the animator to have access to all our positions and IK functions.
    [SerializeField] private Animator animator;

    //joint positions
    [SerializeField][Range(0, 1)] private float leftFootPositionWeight;
    [SerializeField][Range(0, 1)] private float rightFootPositionWeight;
    
    //joint weights
    [SerializeField][Range(0, 1)] private float leftFootRotationWeight;
    [SerializeField][Range(0, 1)] private float rightFootRotationWeight;

    /// <summary>
    /// General foot offset to add more defintion manually through editor.
    /// </summary>
    [SerializeField] private Vector3 footOffset;

    //Using a raycast to determine how far are we from the floor, to determine how much of the inverse kinematics we intend to apply.
    [SerializeField] private Vector3 raycastThresholdBelowGround;
    [SerializeField] private Vector3 raycastThresholdAboveGround;
    

    [SerializeField] private Transform sphereTransform;
    [SerializeField] private Transform sphereTransform2;
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private Transform cubeTransform2;




    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 leftFootPosition = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPosition = animator.GetIKPosition(AvatarIKGoal.RightFoot);

        //sphereTransform.position = rightFootPosition + footOffset;
        int layerMasks = 1 << 7;

        //Determnine if the raycast is touching the floor
        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;

        //Magnitude of the raycast before computing
        float raycastMagnitude =  Vector3.Distance(raycastThresholdAboveGround, raycastThresholdBelowGround);
        //Debug.Log($"{raycastMagnitude}");

        //Hook feet to the ground surface if close to the surface, this is a ray going upwards to avoid the feet from colliding with the surface.
        bool isLeftFootDown = Physics.Raycast(leftFootPosition + raycastThresholdBelowGround, Vector3.up, out hitLeftFoot, raycastMagnitude, layerMasks);
        bool isRightFootDown = Physics.Raycast(rightFootPosition + raycastThresholdBelowGround, Vector3.up, out hitRightFoot, raycastMagnitude, layerMasks);

        CalculateLeftFoot(hitLeftFoot, isLeftFootDown);
        CalculateRightFoot(hitRightFoot, isRightFootDown);

    }



    void CalculateLeftFoot(RaycastHit hitLeftFoot, bool isLeftFootDown)
    {
        //Determine position and rotation of the foot
        if (isLeftFootDown)
        {
            //Assign the weight to the foot IK goal, then move the IK goal to the raycast hit point, plus the offset for the ankle
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPositionWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitLeftFoot.point + footOffset);

            //Project the forward direction from our player to the surface of where he is hitting, 
            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitLeftFoot.normal));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotationWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }

        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }


    void CalculateRightFoot(RaycastHit hitRightFoot, bool isRightFootDown)
    {
        //Determine position and rotation of the foot
        if (isRightFootDown)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPositionWeight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, hitRightFoot.point + footOffset);

            //Project the forward direction from our player to the surface of where he is hitting, 
            //and 
            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitRightFoot.normal));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotationWeight);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }

        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }


}

