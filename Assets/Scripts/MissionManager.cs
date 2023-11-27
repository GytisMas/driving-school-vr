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
    [SerializeField] private Transform taskObjectHolder;
    private List<ActiveTask> allTasks;

    private int currTask = -1;
    private bool missionStarted = false;

    void Awake()
    {
        ActivatePassiveTasks();
        SetTasks();
    }

    private void SetTasks()
    {
        if (SceneManager.GetActiveScene().name == "ParkingLot")
            allTasks =
                MissionBuilder
                .GetTutorialTasks(taskObjectHolder, TaskComplete);
        else
            allTasks =
                MissionBuilder
                .GetMission1Tasks(taskObjectHolder, TaskComplete, FailMission);
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
        Debug.Log("Failed mission");
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
        if (currTask < allTasks.Count) {
            allTasks[currTask].SetAsActive();
            if (!allTasks[currTask].active)
                ActivateNextTask();
        }
        else
            FinishMission();
    }

    private static void FinishMission()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void DeactivateTask(ActiveTask task) 
    {
        task.SetAsInactive();
    }
}


