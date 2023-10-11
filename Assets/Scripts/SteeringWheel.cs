using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    private const float MAX_STEER_VAL = 32767;
    private const float MAX_STEER_ROT = 450;
    private const float MIN_STEER_VAL = -32768;
    private const float MIN_STEER_ROT = -450;
    private float gasInput;
    private float steeringInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float GetWheelRotation() 
    {
        // Y = Y1 + (Y2 – Y1)/(X2 – X1) * (X * X1)
        float steeringVal = DrivingInput.GetAxis("Steering H");
        float steeringRot = 
            MIN_STEER_ROT 
            + (MAX_STEER_ROT - MIN_STEER_ROT) / (MAX_STEER_VAL - MIN_STEER_VAL) 
            * (steeringVal - MIN_STEER_VAL);
        return steeringRot;
    }

    private void Update() {
        transform.eulerAngles = new Vector3 (
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            -GetWheelRotation()
        );
    }
}
