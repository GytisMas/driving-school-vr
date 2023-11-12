using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MissionBuilder 
{
    public static List<Task> AddParkingTask(List<Task> tasks, Transform holder, UnityAction<Task> onComplete
        , List<Vector3> coordsList) 
    {
        var newTask = new ParkingTask(holder);
        foreach (var coords in coordsList) {
            newTask.AddParkingSpot(coords);
        }
        newTask.onComplete += onComplete;
        tasks.Add(newTask);
        return tasks;
    }

    public static List<Task> AddButtonPressTask(List<Task> tasks, Transform holder, UnityAction<Task> onComplete
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

    public static List<Task> GetMission1Tasks(Transform holder, UnityAction<Task> onComplete) 
    {
        var tasks = new List<Task>();
        tasks = AddParkingTask(
            tasks,
            holder, 
            onComplete,
            new() {
                new Vector3(-4.67000008f,1.5f,15.3199997f)
            }
        );

        tasks = AddButtonPressTask(
            tasks,
            holder, 
            onComplete,
            new() {
                new TargetButton(KeyCode.Space),
                new TargetButton(KeyCode.G)
            }
        );

        tasks = AddParkingTask(
            tasks,
            holder, 
            onComplete,
            new() {
                new Vector3(7.54600525f,1.5f,12.3253994f)
            }
        );
        return tasks;
    }
}