using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public static class DrivingScenarioBuilder 
{
    public static List<AIDrivingAction> GetActionList(int index) 
    {
        switch (index) {
            case 1:
                return new() {
                    new AISetSpeed(15f),
                    new AISetAcceleration(1f) {
                        delayAfterAction = 3f
                    },
                    new AISetTurnRate(0.5f) {
                        delayAfterAction = 3f
                    },
                    new AISlowAndWait(7f)
                };
            case 2:
                return new() {
                    new AISetSpeed(15f),
                    new AISetAcceleration(1f),
                    new AIDriveToPoint(new Vector3(-416.299988f, 1.25999999f, 142.679977f))
                };
            case 3:
                return new() {
                    new AISetSpeed(15f),
                    new AISetAcceleration(1f),
                    new AIDriveToPoint(new Vector3(-447.100006f,1.25999999f,164.800003f)),
                    new AISlowAndWait(0f, 5f),
                    new AIDriveToPoint(new Vector3(-447.200012f,1.25999999f,151.5f)),
                    new AISetTurnRate(.8f),
                    new AIDriveToPoint(new Vector3(-472f,1.21000004f,147.399994f), 270f),
                    new AISetSpeed(15f),
                    new AISetAcceleration(1f) {
                        delayAfterAction = 5f
                    },
                    // new AIDriveToPoint(new Vector3(-447.399994f, 1.25999999f, 110.199997f))
                };
        }
        Debug.LogWarning("Returning null");
        return null;
    }

    public static Vector3 GetStartVector(int index) 
    {
        switch (index) {
            case 1:
                return new Vector3(-546.570007f, 1.21000004f, 158.149994f);
            case 2:
                return new Vector3(-481.799988f, 1.25999999f, 142.679993f);
            case 3:
                return new Vector3(-447.050446f, 0.958f, 175.699066f);
        }
        Debug.LogWarning("Returning vector.zero");
        return Vector3.zero;
    }

    public static Vector3 GetStartRotation(int index) 
    {
        switch (index) {
            case 1:
                return new Vector3(0, 180f, 0);
            case 2:
                return new Vector3(0, 90f, 0);
            case 3:
                return new Vector3(0, 180.306f, 0);
        }
        Debug.LogWarning("Returning vector.zero");
        return Vector3.zero;
    }

    public static bool GetDestroyAfter(int index) 
    {
        switch (index) {
            case 1:
                return false;
            case 2:
                return true;
            case 3:
                return true;
        }
        Debug.LogWarning("Couldn't match index, returning true");
        return true;
    }
}