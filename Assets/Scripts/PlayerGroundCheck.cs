using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckOld : MonoBehaviour
{

    private MeshRenderer groundRenderer;



    public void Awake()
    {
        groundRenderer = GetComponent<MeshRenderer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Object is {other.gameObject.name} and its in layer {other.gameObject.layer}");

        if (other.gameObject.layer == 10) 
        {
            groundRenderer.material.color = Color.red;
        }

    }


    public void OnTriggerExit(Collider other)
    {
        
        
        if(other.gameObject.layer == 10)
        {
            groundRenderer.material.color = Color.white;
        }
    }



}
