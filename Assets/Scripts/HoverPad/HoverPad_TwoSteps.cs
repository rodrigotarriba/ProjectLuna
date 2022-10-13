using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HoverPad_TwoSteps : HoverPadBase
{
    [SerializeField] Vector3 Pos1;
    [SerializeField] Vector3 Pos2;
    [SerializeField] float traslationSpeed;
    [SerializeField] Transform xrRig;

    public void Update()
    {
        transform.position = Vector3.Lerp(Pos1, Pos2, Mathf.Sin(Time.time * traslationSpeed));
    }


    public override void CheckHoverState()
    {
        base.CheckHoverState();

        if(previousState == HoverState.oneFootDown || previousState == HoverState.twoFeetDown)
        {
            xrRig.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }
}
