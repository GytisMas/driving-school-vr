using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InputLabel 
{
    NONE = 0,
    UP = 1,
    DOWN = 2,
    ENTER = 3,
    ALL = 15,
}
public static class CustomInput
{
    private static int lastWheelKeyPressed = -1;
    public static InputLabel GetInputDown(InputLabel input) 
    {
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        if (LogitechSteeringWheel.wheelConnected)
            rec = LogitechGSDK.LogiGetStateUnity(0);
        else
            rec = default;
        if (GetKeyDown(input, InputLabel.UP, KeyCode.UpArrow, 19, rec))
            return InputLabel.UP;
        if (GetKeyDown(input, InputLabel.DOWN, KeyCode.DownArrow, 20, rec))
            return InputLabel.DOWN;
        if (GetKeyDown(input, InputLabel.ENTER, 23, rec)) {
            return InputLabel.ENTER;
        }
        return InputLabel.NONE;
    }
    
    public static float GetAxis(string axisName) 
    {
        if (!LogitechSteeringWheel.wheelConnected)
            return 0f;
        var rec = LogitechGSDK.LogiGetStateUnity(0);
        switch (axisName) {
            case "Steering": return rec.lX;
            case "Gas": return rec.lY;
            case "Brake": return rec.lRz;
            case "Clutch": return rec.rglSlider[0];
        }
        Debug.Log("GetAxis value not found");
        return 0f;
    }

    public static float GetAxisNormalised(string axisName) 
    {
        return GetAxis(axisName) / SteeringWheel.MAX_VAL;
    }

    public static float GetAxisNormalised01(string axisName) 
    {
        return (GetAxis(axisName) + SteeringWheel.MAX_VAL) / (2 * SteeringWheel.MAX_VAL);
    }

    private static bool GetKeyDown(InputLabel inputLabel, InputLabel targetLabel, KeyCode keyCode, int wheelKeyIndex, LogitechGSDK.DIJOYSTATE2ENGINES rec) {
        if ((inputLabel & targetLabel) != 0)
            if (Input.GetKeyDown(keyCode) || GetWheelKeyDown(rec, wheelKeyIndex))
                return true;
        return false;
    }

    private static bool GetKeyDown(InputLabel inputLabel, InputLabel targetLabel, int wheelKeyIndex, LogitechGSDK.DIJOYSTATE2ENGINES rec) {
        if ((inputLabel & targetLabel) != 0)
            if (GetWheelKeyDown(rec, wheelKeyIndex))
                return true;
        return false;
    }

    private static bool GetWheelKeyDown(LogitechGSDK.DIJOYSTATE2ENGINES rec, int index) 
    {
        if (!LogitechSteeringWheel.wheelConnected)
            return false;

        bool sameLastKey = index == lastWheelKeyPressed;
        // Key is not pressed
        if (rec.rgbButtons[index] != 128) {
            if (sameLastKey)
                lastWheelKeyPressed = -1;
            return false;
        }

        // Key is held
        if (sameLastKey)
            return false;

        lastWheelKeyPressed = index;
        return true;
    }
}
