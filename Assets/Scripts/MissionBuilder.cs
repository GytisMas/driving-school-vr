using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class MissionBuilder 
{
    public static List<ActiveTask> GetMissionTasks(int index, Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState)
    { 
        switch (index) {
            case 0:
                return GetMission0Tasks(holder, onComplete, onFailState);
            case 1:
                return GetMission1Tasks(holder, onComplete, onFailState);
            case 2:
                return GetMission2Tasks(holder, onComplete, onFailState);
            case 3:
                return GetMission3Tasks(holder, onComplete, onFailState);
            case 4:
                return GetMission4Tasks(holder, onComplete, onFailState);
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
                new Vector3(-4.28999996f,1.5f,18.3500004f),
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
            case 0:
                t.position = new Vector3(-551f,1f,121.900002f);
                t.eulerAngles = new Vector3(0f, 180f, 0f);
                break;
            case 1:
                t.position = new Vector3(-552,1f,165f);
                t.eulerAngles = new Vector3(0f, 180f, 0f);
                break;
            case 2:
                t.position = new Vector3(-529.114624f,1.74468231f,79.018959f);
                t.eulerAngles = new Vector3(0f, 135f, 0f);
                break;
            case 3:
                t.position = new Vector3(-529.114624f,1.74468231f,79.018959f);
                t.eulerAngles = new Vector3(0f, 135f, 0f);
                break;
            case 4:
                t.position = new Vector3(-529.114624f,1.74468231f,79.018959f);
                t.eulerAngles = new Vector3(0f, 135f, 0f);
                break;
        }
    } 

    private static List<ActiveTask> GetMission1Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState) 
    {
        var tasks = new List<ActiveTask>();
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

    private static List<ActiveTask> GetMission0Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState) 
    {
        var tasks = new List<ActiveTask>();
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-550.783752f,3.03354168f,102.396164f)
            },
            new() {
                180f
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-539.53f,3.03354955f,80.8115768f)
            },
            new() {
                136.53f
            },
            new() {
                5f
            }
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-510.690002f,3.03354955f,73.0500031f)
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
                new Vector3(-448.350006f,3.03354955f,66.7099991f)
            },
            new() {
                132f
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, holder, onComplete,
            new() {
                new Vector3(-448.119995f,1.74465382f,32.2261429f)
            },
            new() {
                180f
            }
        );
        return tasks;
    }

    private static List<ActiveTask> GetMission2Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState) 
    {
        var tasks = new List<ActiveTask>();
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-490.224121f,3.03356934f,72.2473297f)
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
                new Vector3(-417.224121f,3.03356934f,68.4973221f)
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
                new Vector3(-373.314026f,3.03356934f,66.4100266f)
            },
            new() {
                135f
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, holder, onComplete,
            new() {
                new Vector3(-372.921417f,1.74465942f,31.9939117f)
            },
            new() {
                180f
            }
        );
        return tasks;
    }

    private static List<ActiveTask> GetMission3Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState) 
    {
        var tasks = new List<ActiveTask>();
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-490.224121f,3.03356934f,72.2473297f)
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
                new Vector3(-445.224121f,3.03356934f,72.2473221f)
            },
            new() {
                45f
            },
            new() {
                5f
            }
        );
        tasks = AddAICarRoutine(tasks, 3f, holder,
            new() { 
                 2, 2, 2
            },
            onFailState
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-443f,3.03356934f,105.599998f)
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
                new Vector3(-443f,3.03356934f,105.599998f)
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
                new Vector3(-441.5f,3.03356934f,141.100006f)
            },
            new() {
                45f
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, holder, onComplete,
            new() {
                new Vector3(-408.399994f,1.74466515f,142f)
            },
            new() {
                90f
            }
        );
        return tasks;
    }

    private static List<ActiveTask> GetMission4Tasks(Transform holder, UnityAction<ActiveTask> onComplete, 
        UnityAction<PassiveTaskObject, string> onFailState) 
    {
        var tasks = new List<ActiveTask>();
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-490.224121f,3.03356934f,72.2473297f)
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
                new Vector3(-445.224121f,3.03356934f,72.2473221f)
            },
            new() {
                45f
            },
            new() {
                5f
            }
        );
        tasks = AddAICarRoutine(tasks, 3f, holder,
            new() { 
                 2, 2
            },
            onFailState
        );
        tasks = AddReachLocationTask(tasks, holder, onComplete,
            new() {
                new Vector3(-443f,3.03356934f,105.599998f)
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
                new Vector3(-443f,3.03356934f,105.599998f)
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
                new Vector3(-441.5f,3.03356934f,141.100006f)
            },
            new() {
                45f
            },
            new() {
                5f
            }
        );
        tasks = AddParkingTask(tasks, holder, onComplete,
            new() {
                new Vector3(-329.649994f,1.74466515f,141.999985f)
            },
            new() {
                90f
            }
        );
        return tasks;
    }

    private static List<ActiveTask> AddAICarRoutine(List<ActiveTask> tasks, float _timeDelay, Transform holder
        , List<int> scenarioIDs, UnityAction<PassiveTaskObject, string> onFailState)
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