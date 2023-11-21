using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;


public enum InputLabel 
{
    NONE = 0,
    UP = 1,
    DOWN = 2,
    ENTER = 4,
    L3 = 8,
    R3 = 16,
    GEARUP = 32,
    GEARDOWN = 64,
    ALL = 1+2+4+8+16+32+64,
}
enum WheelKeyState 
{
    INACTIVE = 0,
    PRESSED = 1,
    HELD = 2,
    RELEASED = 4
}
public class CustomInput : MonoBehaviour
{
    private static WheelKeyState[] wheelKeys = new WheelKeyState[128]; // 128 buttons in wheel
    private static int lastWheelKeyPressed = -1;
    public static InputLabel GetInputDown(InputLabel input) 
    {
        var inputRes = InputLabel.NONE;
        if (GetKeyDown(input, InputLabel.UP, KeyCode.UpArrow, 19))
            inputRes = InputLabel.UP;
        if (GetKeyDown(input, InputLabel.DOWN, KeyCode.DownArrow, 20))
            inputRes = InputLabel.DOWN;
        if (GetKeyDown(input, InputLabel.ENTER, KeyCode.KeypadEnter, 23))
            inputRes = InputLabel.ENTER;
        if (GetKeyDown(input, InputLabel.L3, KeyCode.Q, 11))
            inputRes = InputLabel.L3;
        if (GetKeyDown(input, InputLabel.R3, KeyCode.E, 10))
            inputRes = InputLabel.R3;
        if (GetKeyDown(input, InputLabel.GEARUP, KeyCode.M, 4))
            inputRes = InputLabel.GEARUP;
        if (GetKeyDown(input, InputLabel.GEARDOWN, KeyCode.N, 5))
            inputRes = InputLabel.GEARDOWN;
        return inputRes;
    }
    
    public static float GetAxis(string axisName) 
    {
        if (!LogitechSteeringWheel.wheelConnected)
            return 2f;
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

    private static bool GetKeyDown(InputLabel inputLabel, InputLabel targetLabel, KeyCode keyCode, int wheelKeyIndex) {
        if ((inputLabel & targetLabel) != 0)
            if (Input.GetKeyDown(keyCode) || GetWheelKeyDown(wheelKeyIndex))
                return true;
        return false;
    }

    private static bool GetKeyDown(InputLabel inputLabel, InputLabel targetLabel, int wheelKeyIndex) {
        if ((inputLabel & targetLabel) != 0)
            if (GetWheelKeyDown(wheelKeyIndex))
                return true;
        return false;
    }

    private static bool GetKeyDown(InputLabel inputLabel, InputLabel targetLabel, KeyCode keyCode) {
        if ((inputLabel & targetLabel) != 0)
            if (Input.GetKeyDown(keyCode))
                return true;
        return false;
    }

    private static bool GetWheelKeyDown(int index) 
    {
        return wheelKeys[index] == WheelKeyState.PRESSED;
    }

    private void SetWheelKeyValues()
    {
        if (!LogitechSteeringWheel.wheelConnected)
            return;
        var rec = LogitechGSDK.LogiGetStateUnity(0);

        for (int i = 0; i < wheelKeys.Length; i++) {
            // Key is pressed when value is 128
            SetKeyState(i, rec.rgbButtons[i] == 128);
        }
    }

    private void SetKeyState(int index, bool currentlyPressed) 
    {
        if (!currentlyPressed) {
            switch (wheelKeys[index]) {
                case WheelKeyState.INACTIVE:
                    break;
                case WheelKeyState.PRESSED:
                    wheelKeys[index] = WheelKeyState.RELEASED;
                    break;
                case WheelKeyState.HELD:
                    wheelKeys[index] = WheelKeyState.RELEASED;
                    break;
                case WheelKeyState.RELEASED:
                    wheelKeys[index] = WheelKeyState.INACTIVE;
                    break;
            }
        } else {
            switch (wheelKeys[index]) {
                case WheelKeyState.HELD:
                    break;
                case WheelKeyState.INACTIVE:
                    wheelKeys[index] = WheelKeyState.PRESSED;
                    break;
                case WheelKeyState.PRESSED:
                    wheelKeys[index] = WheelKeyState.HELD;
                    break;
                case WheelKeyState.RELEASED:
                    wheelKeys[index] = WheelKeyState.PRESSED;
                    break;
            }
        }
    }

    private void Update() 
    {
        SetWheelKeyValues();
    }

    private static bool GetWheelKeyDownOld(LogitechGSDK.DIJOYSTATE2ENGINES rec, int index) 
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
