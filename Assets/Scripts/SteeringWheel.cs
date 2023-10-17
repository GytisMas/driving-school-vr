using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour
{
    private const float MIN_VAL = -32768;
    private const float MAX_VAL = 32768;
    private const float MIN_STEER_ROT = -450;
    private const float MAX_STEER_ROT = 450;
    private const float MIN_PEDAL_ROT = 60;
    private const float MAX_PEDAL_ROT = 15;

    [SerializeField] private Transform gasPedal;
    [SerializeField] private Transform brakePedal;
    [SerializeField] private Transform clutchPedal;

    private float[] lastVals = new float[4];
    private float[] lastRots = new float[4];

    private void Start() {
        // LogitechGSDK.LogiPlayFrontalCollisionForce(0, 20);        
    }

    private void RotateParts()
    {
        transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    -GetPartRotation("Steering", MIN_STEER_ROT, MAX_STEER_ROT, 0)
                );
        gasPedal.eulerAngles = new Vector3(
                    GetPartRotation("Gas", MIN_PEDAL_ROT, MAX_PEDAL_ROT, 1),
                    gasPedal.eulerAngles.y,
                    gasPedal.eulerAngles.z
                );
        brakePedal.eulerAngles = new Vector3(
                    GetPartRotation("Brake", MIN_PEDAL_ROT, MAX_PEDAL_ROT, 2),
                    brakePedal.eulerAngles.y,
                    brakePedal.eulerAngles.z
                );
        clutchPedal.eulerAngles = new Vector3(
                    GetPartRotation("Clutch", MIN_PEDAL_ROT, MAX_PEDAL_ROT, 3),
                    clutchPedal.eulerAngles.y,
                    clutchPedal.eulerAngles.z
                );
    }

    private float GetPartRotation(string part, float minRot, float maxRot, int index) 
    {
        // Y = Y1 + (Y2 – Y1)/(X2 – X1) * (X * X1)
        float value = CustomInput.GetAxis(part);

        if (value == lastVals[index])
            return lastRots[index];

        float rotation = 
            minRot 
            + (maxRot - minRot) 
                / (MAX_VAL - MIN_VAL) 
                * (value - MIN_VAL);

        lastRots[index] = rotation;
        lastVals[index] = value;
        return rotation;
    }

    private void Update()
    {
        RotateParts();
    }
}
