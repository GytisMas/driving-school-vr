using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class MissionBuilder 
{
    public static List<ActiveTask> GetMissionTasks(int index, Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject> onFailState)
    {
        switch (index) {
            case 1:
                return GetMission1Tasks(holder, onComplete, onFailState);
        }
        Debug.LogWarning("Loading tutorial missions");
        return GetTutorialTasks(holder, onComplete);
    }

    public static List<ActiveTask> GetTutorialTasks(Transform taskObjectHolder, UnityAction<ActiveTask> onComplete)
    {
        var tasks = new List<ActiveTask>();
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(0f,0.600000024f,6.67000008f),
            },
            new() {
                90f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(16.7199993f,0.600000024f,7.90999985f),
            },
            new() {
                30f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(18.0900002f,0.600000024f,22.9799995f),
            },
            new() {
                0f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(18.0900002f,0.600000024f,47.1399994f),
            },
            new() {
                -45f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(7.84000015f,0.600000024f,53.0200005f),
            },
            new() {
                -120.43f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(1.37f,0.600000024f,47.4099998f),
            },
            new() {
                180f,
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, taskObjectHolder, onComplete, 
            new() {
                new Vector3(1.37f,0.600000024f,29.6599998f),
            },
            new() {
                193.7f,
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, taskObjectHolder, onComplete,
            new() {
                new Vector3(-4.28999996f,1f,18.3500004f),
            },
            new() {
                -90f
            }
        );
        return tasks;
    }

    public static void SetPlayerCarStartPos(int index, Transform t) 
    {
        switch (index) {
            case 1:
                t.position = new Vector3(-552,1f,165f);
                t.eulerAngles = new Vector3(0f, 180f, 0f);
                t.GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;
        }
    } 

    private static List<ActiveTask> GetMission1Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject> onFailState) 
    {
        var tasks = new List<ActiveTask>();
        // Vector3(-551.273132,3.03354359,132.85553)
        // Vector3(-537.632874,3.03354931,79.137825) -50
        // Vector3(-494.791809,3.03356099,67.6948166) 90
        // Vector3(-427.350616,3.03352642,66.8543625) 90
        // park Vector3(-395.799652,1.74467468,62.9536514) 0
        tasks = AddAICarRoutine(tasks, 0f, holder,
            new() { 
                 2, 3
            },
            onFailState
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-551.273132f,3.03354359f,132.85553f)
            },
            new() {
                0f
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-537.632874f, 3.03354931f, 79.137825f)
            },
            new() {
                -50f
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-489.589996f,0.529999971f,71.8799973f)
            },
            new() {
                90f
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-427.350616f,3.03352642f,66.8543625f)
            },
            new() {
                90f
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, holder, onComplete,
            new() {
                new Vector3(-395.799652f,1.74467468f,62.9536514f)
            },
            new() {
                90f
            }
        );
        return tasks;
    }

    private static List<ActiveTask> AddAICarRoutine(List<ActiveTask> tasks, float _timeDelay, Transform holder
        , List<int> scenarioIDs, UnityAction<PassiveTaskObject> onFailState)
    {
        if (scenarioIDs == null || scenarioIDs.Count < 1) {
            Debug.LogWarning("Driving routine not created successfully");
            return tasks;
        }

        var newTask = new AICarRoutine(holder) {
            timeDelay = _timeDelay
        };

        foreach (int scenarioID in scenarioIDs)
            newTask.AddDrivingScenario(
                DrivingScenarioBuilder.GetActionList(scenarioID), 
                DrivingScenarioBuilder.GetStartVector(scenarioID), 
                DrivingScenarioBuilder.GetStartRotation(scenarioID), 
                DrivingScenarioBuilder.GetDestroyAfter(scenarioID), 
                onFailState);

        tasks.Add(newTask);
        return tasks;
    }

    private static List<ActiveTask> AddReachLocationTask(List<ActiveTask> tasks, Transform holder, UnityAction<ActiveTask> onComplete
        , List<Vector3> coordsList, List<float> yRotList, List<float> widthList) 
    {
        var newTask = new ReachLocationTask(holder);
        for(int i = 0; i < coordsList.Count; i++) {
            newTask.AddLocationSpot(coordsList[i], yRotList[i], widthList[i]);
        }
        newTask.onComplete += onComplete;
        tasks.Add(newTask);
        return tasks;
    }

    private static List<ActiveTask> AddParkingTask(List<ActiveTask> tasks, Transform holder, UnityAction<ActiveTask> onComplete
        , List<Vector3> coordsList, List<float> yRotList) 
    {
        var newTask = new ParkingTask(holder);
        for(int i = 0; i < coordsList.Count; i++) {
            newTask.AddParkingSpot(coordsList[i], yRotList[i]);
        }
        newTask.onComplete += onComplete;
        tasks.Add(newTask);
        return tasks;
    }

    private static List<ActiveTask> AddButtonPressTask(List<ActiveTask> tasks, Transform holder, UnityAction<ActiveTask> onComplete
        , List<TargetButton> targetButtons) 
    {
        var newTask = new ButtonPressTask(holder);
        foreach (var b in targetButtons) {
            newTask.AddButtonDetector(b.keyCode, b.inputLabel, b.wheelAxis, b.axisTarget);
        }
        newTask.onComplete += onComplete;
        tasks.Add(newTask);
        return tasks;
    }
}