using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingInput : MonoBehaviour
{
    public static float GetAxis(string axisName) 
    {
        var rec = LogitechGSDK.LogiGetStateUnity(0);
        switch (axisName) {
            case "Steering H": return rec.lX;
            case "Gas V": return rec.lY;
        }
        return 0f;
    }
}