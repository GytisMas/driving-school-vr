using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonPressTask : ActiveTask
{
    private GameObject buttonDetectorPrefab;
    private List<ButtonDetector> detectors;
    private List<bool> visitedDetectors;

    public ButtonPressTask(Transform holder) : base(holder) 
    {
        buttonDetectorPrefab = (GameObject)Resources.Load("MissionObjects/ButtonDetector");
        detectors = new List<ButtonDetector>();
        visitedDetectors = new List<bool>();
    }

    public void AddButtonDetector(KeyCode kc = KeyCode.None, InputLabel wheel = InputLabel.NONE, string axis = "", float target = -1f) 
    {
        ButtonDetector detector = 
            GameObject.Instantiate(buttonDetectorPrefab, taskObjectHolder)
            .GetComponent<ButtonDetector>();
        detector.SetTargetButtons(kc, wheel, axis, target);
        detector.gameObject.SetActive(false);
        detector.onSuccess += DetectButtonPress;
        detectors.Add(detector);
        visitedDetectors.Add(false);
    }

    public override void SetAsActive()
    {
        active = true;
        foreach (var loc in detectors)
            loc.gameObject.SetActive(true);
    }

    public override void SetAsInactive()
    {
        active = false;
        foreach (var loc in detectors)
            loc.gameObject.SetActive(false);
    }

    private void DetectButtonPress(ButtonDetector detector) 
    {
        int index = detectors.IndexOf(detector);
        visitedDetectors[index] = true;
        
        bool allVisited = true;
        foreach (var visited in visitedDetectors) {
            if (!visited) {
                allVisited = false;
                break;
            }
        }
           
        if (allVisited)
            onComplete?.Invoke(this);
    }    
}