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
    [SerializeField] private Transform playerCar;
    private List<ActiveTask> allTasks;

    private int currTask = -1;
    private bool missionStarted = false;
    private bool failed = false;
    public delegate void MissionFailedEvent(string drivingError);
    public static event MissionFailedEvent OnMissionFailed;
    void Awake()
    {
        ActivatePassiveTasks();
        SetTasks();
    }

    private void SetTasks()
    {
        int missionIndex = UnityEngine.Random.Range(0, 5);
        if (SceneManager.GetActiveScene().name == "ParkingLot")
            allTasks =
                MissionBuilder
                .GetTutorialTasks(taskObjectHolder, TaskComplete);
        else {
            allTasks = MissionBuilder.GetMissionTasks(
                missionIndex, taskObjectHolder, TaskComplete, FailMission);
            MissionBuilder.SetPlayerCarStartPos(missionIndex, playerCar);
        }
            
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

    public void FailMission(PassiveTaskObject pObj, string drivingError) 
    {
        if (failed) 
            return;
        
        Debug.Log($"Mission failed. Detected driving error: {drivingError}");
        failed = true;
        StartCoroutine(BulletTime());
        OnMissionFailed?.Invoke(drivingError);
        // SceneManager.LoadScene("MainMenu");
    }

    IEnumerator BulletTime() 
    {
        while (Time.timeScale > 0.26f) {
            Time.timeScale /= 2f;
            yield return new WaitForSeconds(Time.timeScale * .5f);
        }
        Time.timeScale = 0f;
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


