using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingInput : MonoBehaviour
{
    public static float GetAxis(string axisName) 
    {
        var rec = LogitechGSDK.LogiGetStateUnity(0);
        switch (axisName) {
            case "Steering H": return rec.lX / 32760f;
            case "Gas V": return rec.lY / -32760f;
        }
        return 0f;
    }
}