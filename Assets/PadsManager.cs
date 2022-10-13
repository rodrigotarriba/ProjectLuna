using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class is in charge of receiving and updating all pads status, assigning trayectory for moving objects and alerting other managers when prompted. Still pretty basic at the moment, will become larger with more implementation.
/// </summary>
public class PadsManager : MonoBehaviour
{

    public static PadsManager padsManager;


    private void Awake()
    {
        //Make a singleton of this class
        if (padsManager != null && padsManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            padsManager = this;
        }
    }

    public void onPadHit(Collider padCollider, string footName)
    {
        //Point of reference for pad hits, send info to specific pad for particles.
        if (footName == "Left") padCollider.GetComponent<HoverPadBase>().leftFootTouching = true;
        if (footName == "Right") padCollider.GetComponent<HoverPadBase>().rightFootTouching = true;
    }

}
