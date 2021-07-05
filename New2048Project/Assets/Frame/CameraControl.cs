using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float devHeight = 9.6f;
    public float devWidth = 6.4f;

    // Use this for initialization
    void Start()
    {
        float screenHeight = Screen.height;
        float orthographicSize = this.GetComponent<Camera>().orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraWidth = orthographicSize * 2 * aspectRatio;
        if (cameraWidth < devWidth)
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }
    }
    // Update is called once per frame
    void Update(){}
}
