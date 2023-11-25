using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
public class MissionManager : MonoBehaviour
{

    //Button press task'ai

    // public KeyCode[] requiredKeys;
    // UI checklist
    // public Toggle[] toggles;
    // public UnityAction<int> onButtonPressed;

    [SerializeField] private Transform taskObjectHolder;
    private List<ActiveTask> allTasks;

    private int currTask = -1;
    private bool missionStarted = false;

    void Awake() 
    {
        ActivatePassiveTasks();
        allTasks = 
            MissionBuilder
            .GetMission1Tasks(taskObjectHolder, TaskComplete);
    }

    private void Start() 
    {
        ActivateNextTask();
    }

    private void ActivatePassiveTasks() 
    {
        var passiveTaskObjects = FindObjectsOfType<PassiveTaskObject>().ToList();
        foreach (var pTask in passiveTaskObjects)
            pTask.onFailState += FailMission;
    }

    private void FailMission(PassiveTaskObject pObj) 
    {
        SceneManager.LoadScene("Test Drive");
    }

    private void TaskComplete(ActiveTask task) 
    {
        DeactivateTask(task);
        ActivateNextTask();
    }

    private void ActivateNextTask()
    {
        currTask++;
        if (currTask < allTasks.Count)
            allTasks[currTask].SetAsActive();
        else
            SceneManager.LoadScene("MainMenu");
    }

    private void DeactivateTask(ActiveTask task) 
    {
        task.SetAsInactive();
    }

    // private void Update()
    // {
    //     for (int i = 0; i < requiredKeys.Length; i++)
    //     {
    //         if (Input.GetKeyDown(requiredKeys[i]) && !toggles[i].isOn)
    //         {
    //             onButtonPressed(i);
    //         }
    //     }

    //     // Check if first 4 tutorial tasks are completed.
    //     if (taskCount == 4 && currTask + 4 < allTasks.Count)
    //     {
    //         StartCoroutine(CompleteAndActivateMissions(currTask, currTask + 4));
    //         currTask += 4;
    //         taskCount = 0;
    //     }
    // }

    // private void ParkingMissionCompleted()
    // {
    //     // Find the index of the completed parking mission
    //     int parkingMissionIndex = currTask;
    //     for (int i = 0; i < allTasks.Count; i++)
    //     {
    //         if (allTasks[i].activeSelf && toggles[i].isOn)
    //         {
    //             parkingMissionIndex = i;
    //             break;
    //         }
    //     }

    //     // Ensure the index is valid
    //     // if (parkingMissionIndex >= 0 && parkingMissionIndex < allMissions.Count)
    //     // {
    //     //     toggles[parkingMissionIndex].isOn = true;
    //     //     if (currentParkingIndex < parkingSpots.Length)
    //     //     {
    //     //         parkingSpots[currentParkingIndex].gameObject.SetActive(true);
    //     //         currentParkingIndex++;
    //     //     }

    //     //     // Start coroutine to deactivate the completed mission with 2sec delay.
    //     //     StartCoroutine(CompleteAndActivateParkingMission(parkingMissionIndex));

    //     //     // Check if all parking tasks are completed
    //     //     if (currentParkingIndex >= parkingSpots.Length)
    //     //     {
    //     //         currentMissionIndex++;
    //     //     }
    //     // }

    //     // Debug.Log(currTask);
    // }

    // IEnumerator CompleteAndActivateMissions(int startIndex, int endIndex)
    // {
    //     // Delay after button press completion
    //     yield return new WaitForSeconds(2f);

    //     // Deactivate the completed button press tasks
    //     DeactivateMissions(startIndex, endIndex);

    //     // Activate the next set of tasks
    //     ActivateNextTask(currTask);
    // }

    // IEnumerator CompleteAndActivateParkingMission(int completedMissionIndex)
    // {
    //     // Wait for 2 seconds before deactivating the completed mission
    //     yield return new WaitForSeconds(2f);

    //     // Deactivate the completed parking mission
    //     DeactivateMissions(completedMissionIndex, completedMissionIndex + 1);

    //     // Find the index of the next mission
    //     int nextMissionIndex = completedMissionIndex + 1;

    //     // Ensure the next mission index is valid
    //     if (nextMissionIndex < allTasks.Count)
    //     {
    //         // Activate the next mission
    //         allTasks[nextMissionIndex].SetActive(true);
    //     }
    // }

    // public void OnClick(int i)
    // {
    //     if (taskCount < 4)
    //     {
    //         toggles[i].isOn = true;
    //         taskCount++;
    //     }
    // }


    // private void DeactivateMissions(int startIndex, int endIndex)
    // {

    //     // Deactivate the completed tasks
    //     for (int i = startIndex; i < endIndex; i++)
    //     {
    //         if (i < allTasks.Count)
    //         {
    //             allTasks[i].SetActive(false);
    //         }
    //     }
    // }
}


