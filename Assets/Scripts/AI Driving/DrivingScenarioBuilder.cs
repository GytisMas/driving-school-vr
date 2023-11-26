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
                    new AIStopForSeconds(7f)
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

        }
        Debug.LogWarning("Returning vector.zero");
        return Vector3.zero;
    }

    public static Vector3 GetStartRotation(int index) 
    {
        switch (index) {
            case 1:
                return new Vector3(0, 180f, 0);
        }
        Debug.LogWarning("Returning vector.zero");
        return Vector3.zero;
    }

    public static bool GetDestroyAfter(int index) 
    {
        switch (index) {
            case 1:
                return false;
        }
        Debug.LogWarning("Couldn't match index, returning false");
        return false;
    }
}