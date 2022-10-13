using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectionForPlayerHeight : MonoBehaviour
{
    public void Update()
    {
        //PlayerManager.playerManager.useGravity = true;
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 1 << 7)
        {
            //PlayerManager.playerManager.useGravity = false;
            return;
        }
    }
}
