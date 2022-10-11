using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SyncRotations : MonoBehaviour
{
    [SerializeField] Vector3 positionTransform;
    [SerializeField] Vector3 rotationTransform;

    [SerializeField] Transform riggedBone;
    [SerializeField] Transform adjustedBone;
 
    // Update is called once per frame
    void Update()
    {
        positionTransform = adjustedBone.position - riggedBone.position;
        rotationTransform = adjustedBone.rotation.eulerAngles - riggedBone.rotation.eulerAngles;
    }
}
