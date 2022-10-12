using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class is in charge of receiving and updating pads status, assigning trayectory for moving objects and alerting other managers when prompted.
/// </summary>
public class PadsManager : MonoBehaviour
{

    public static PadsManager padsManager;


    private void Awake()
    {
        if (padsManager != null && padsManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            padsManager = this;
        }
    }


    private void Update()
    {

    }

    public void onPadHit(Collider padCollider, string footName)
    {
        if (footName == "Left") padCollider.GetComponent<HoverPadBase>().leftFootTouching = true;
        if (footName == "Right") padCollider.GetComponent<HoverPadBase>().leftFootTouching = true;
    }

}
