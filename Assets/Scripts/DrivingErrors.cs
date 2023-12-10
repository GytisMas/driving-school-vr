using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrivingErrors : MonoBehaviour
{

    [SerializeField] TMP_Text _textMeshPro;
    private void Start()
    {
        MissionManager.OnMissionFailed += HandleMissionFailed;
    }

    private void HandleMissionFailed(string drivingError)
    {
        _textMeshPro.text = "Critical fault: " + drivingError;
        // You have access to the driving error here
        Debug.Log($"Mission failed in other script with error: {drivingError}");
        // Other actions based on the driving error...
    }

    private void OnDestroy()
    {
        MissionManager.OnMissionFailed -= HandleMissionFailed;
    }
}
