using LightBuzz.Kinect4Azure;
using System.Collections.Generic;
using UnityEngine;

public class KinectManager : MonoBehaviour
{
    //Singleton kinectManager
    public static KinectManager kinectManager;
    
    [SerializeField] private Configuration _configuration;
    
    //rtc we avoid using the stickmanManager
    [SerializeField] private StickmanManager _stickmanManager;

    [Tooltip("The rotation and zoom speed when using the left/right/top/down arrow keys or the mouse wheel.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _speed = 0.5f;

    private KinectSensor _sensor;

    //rtc
    public bool isSensorReady => _sensor != null && _sensor.IsOpen;
    public List<Body> kinectBodies;

    public void Awake()
    {
        //Make KinectManager class a Singleton && check it is unique
        if (kinectManager != null && kinectManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            kinectManager = this;
        }

    }


    private void Start()
    {
        _sensor = KinectSensor.Create(_configuration);

        if (_sensor == null)
        {
            Debug.LogError("Sensor not connected!");
            return;
        }

        _sensor.Open();
    }

    private void OnDestroy()
    {
        _sensor?.Close();
    }

    private void Update()
    {
        if (_sensor == null || !_sensor.IsOpen) return;

        Frame frame = _sensor.Update();

        if (frame != null)
        {
            kinectBodies = frame.BodyFrameSource?.Bodies;


            //rtc we interrupt loading bodies onto stickmanager
            _stickmanManager.Load(kinectBodies);
        }
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = Camera.main.transform.localPosition;
        Vector3 originPosition = Vector3.zero;
        float angle = _speed * 100.0f * Time.deltaTime;

        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    Camera.main.transform.RotateAround(originPosition, Vector3.up, angle);
        //}

        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    Camera.main.transform.RotateAround(originPosition, Vector3.down, angle);
        //}

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    Camera.main.transform.RotateAround(originPosition, Vector3.right, angle);
        //}

        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    Camera.main.transform.RotateAround(originPosition, Vector3.left, angle);
        //}

        //if (Input.mouseScrollDelta != Vector2.zero)
        //{
        //    Camera.main.transform.localPosition = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z + Input.mouseScrollDelta.y * _speed);
        //}
    }
}
