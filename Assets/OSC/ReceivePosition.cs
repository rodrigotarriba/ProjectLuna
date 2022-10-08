using UnityEngine;
using System.Collections;

public class ReceivePosition : MonoBehaviour {
    
   	public OSC osc;
    [SerializeField] private Transform joint;


    public void Awake()
    {
        
    }




    // Use this for initialization
    void Start () {
	   osc.SetAddressHandler( "/Joints" , OnReceiveJoints );
       //osc.SetAddressHandler("/x", OnReceiveX);
       //osc.SetAddressHandler("/y", OnReceiveY);
       //osc.SetAddressHandler("/z", OnReceiveZ);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnReceiveJoints(OscMessage message)
    {
        float x = message.GetInt(0);
        float y = message.GetInt(1);
        float z = message.GetInt(2);

        transform.position = new Vector3(x, y, z);
    }
















    //void OnReceiveX(OscMessage message) {
    //    float x = message.GetFloat(0);

    //    Vector3 position = joint.position;

    //    position.x = x;

    //    joint.position = position;
    //}

    //void OnReceiveY(OscMessage message) {
    //    float y = message.GetFloat(0);

    //    Vector3 position = joint.position;

    //    position.y = y;

    //    joint.position = position;
    //}

    //void OnReceiveZ(OscMessage message) {
    //    float z = message.GetFloat(0);

    //    Vector3 position = joint.position;

    //    position.z = z;

    //    joint.position = position;
    //}


}
