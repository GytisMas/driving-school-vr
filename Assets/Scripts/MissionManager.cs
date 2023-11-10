using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
public class MissionManager : MonoBehaviour
{
    //Button press task'ai
    public KeyCode[] requiredKeys;
    // UI checklist
    public Toggle[] toggles;
    public UnityAction<int> onButtonPressed;
    public List<GameObject> allMissions;
    public Collider[] parkingSpots;

    private int currentParkingIndex = 0;
    private int taskCount = 0;
    private int currentMissionIndex = 0;

    void Start()
    {
        ActivateMissions(currentMissionIndex);
        onButtonPressed += OnClick;

        ReachLocationTask.OnParkingMissionCompleted += ParkingMissionCompleted;

    }

    private void Update()
    {
        for (int i = 0; i < requiredKeys.Length; i++)
        {
            if (Input.GetKeyDown(requiredKeys[i]) && !toggles[i].isOn)
            {
                onButtonPressed(i);
            }
        }

        // Check if first 4 tutorial missions are completed.
        if (taskCount == 4 && currentMissionIndex + 4 < allMissions.Count)
        {
            StartCoroutine(CompleteAndActivateMissions(currentMissionIndex, currentMissionIndex + 4));
            currentMissionIndex += 4;
            taskCount = 0;
        }
    }

    private void ParkingMissionCompleted()
    {
        // Find the index of the completed parking mission
        int parkingMissionIndex = currentMissionIndex;
        for (int i = 0; i < allMissions.Count; i++)
        {
            if (allMissions[i].activeSelf && toggles[i].isOn)
            {
                parkingMissionIndex = i;
                break;
            }
        }

        // Ensure the index is valid
        if (parkingMissionIndex >= 0 && parkingMissionIndex < allMissions.Count)
        {
            toggles[parkingMissionIndex].isOn = true;
            if (currentParkingIndex < parkingSpots.Length)
            {
                parkingSpots[currentParkingIndex].gameObject.SetActive(true);
                currentParkingIndex++;
            }

            // Start coroutine to deactivate the completed mission with 2sec delay.
            StartCoroutine(CompleteAndActivateParkingMission(parkingMissionIndex));

            // Check if all parking missions are completed
            if (currentParkingIndex >= parkingSpots.Length)
            {
                currentMissionIndex++;
            }
        }

        Debug.Log(currentMissionIndex);
    }
    IEnumerator CompleteAndActivateMissions(int startIndex, int endIndex)
    {
        // Delay after button press completion
        yield return new WaitForSeconds(2f);

        // Deactivate the completed button press missions
        DeactivateMissions(startIndex, endIndex);

        // Activate the next set of missions
        ActivateMissions(currentMissionIndex);
    }

    IEnumerator CompleteAndActivateParkingMission(int completedMissionIndex)
    {
        // Wait for 2 seconds before deactivating the completed mission
        yield return new WaitForSeconds(2f);

        // Deactivate the completed parking mission
        DeactivateMissions(completedMissionIndex, completedMissionIndex + 1);

        // Find the index of the next mission
        int nextMissionIndex = completedMissionIndex + 1;

        // Ensure the next mission index is valid
        if (nextMissionIndex < allMissions.Count)
        {
            // Activate the next mission
            allMissions[nextMissionIndex].SetActive(true);


        }
    }
    public void OnClick(int i)
    {
        if (taskCount < 4)
        {
            toggles[i].isOn = true;
            taskCount++;
        }
    }

    private void ActivateMissions(int startIndex)
    {
        for (int i = startIndex; i < startIndex + 4; i++)
        {
            if (i < allMissions.Count)
            {
                // Check if the current mission is a parking mission
                if (allMissions[i].GetComponent<ReachLocationTask>() != null)
                {
                    // Show the corresponding parking spot
                    if (currentParkingIndex < parkingSpots.Length)
                    {

                        parkingSpots[currentParkingIndex].gameObject.SetActive(true);
                        currentParkingIndex++;
                    }
                }

                allMissions[i].SetActive(true);
            }
        }
    }

    private void DeactivateMissions(int startIndex, int endIndex)
    {

        // Deactivate the completed missions
        for (int i = startIndex; i < endIndex; i++)
        {
            if (i < allMissions.Count)
            {
                allMissions[i].SetActive(false);
            }
        }
    }
}


