using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLambda : MonoBehaviour
{
    private Transform myTransform;
    private Material myMaterial;
    public bool facingWrongWay => myTransform.forward.z >= 0f;
    public string CurrentColor { get; set; }

    public bool IsBlack { get; set ; }

    public void Awake()
    {
        myTransform = GetComponent<Transform>();
        myMaterial = GetComponent<MeshRenderer>().material;


    }

    public void Update()
    {

        if (facingWrongWay) myMaterial.color = Color.yellow;
        else myMaterial.color = Color.black;



    }


}
