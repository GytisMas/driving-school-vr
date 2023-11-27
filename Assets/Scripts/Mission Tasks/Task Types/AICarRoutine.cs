using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AICarRoutine : ActiveTask
{
    public float timeDelay = 0f;
    private GameObject drivingScenarioObjPrefab;
    private GameObject spawnerPrefab;
    private List<AIDrivingScenario> DrivingScenarios;
    private AICarSpawner spawner;
    private List<bool> endedScenarios;

    public AICarRoutine(Transform holder) : base(holder) 
    {
        drivingScenarioObjPrefab = (GameObject)Resources.Load("MissionObjects/AICarScenario");
        spawnerPrefab = (GameObject)Resources.Load("MissionObjects/AICarSpawner");
        DrivingScenarios = new List<AIDrivingScenario>();
        endedScenarios = new List<bool>();
        spawner = GameObject.Instantiate(spawnerPrefab, Vector3.zero, 
                Quaternion.identity, taskObjectHolder)
            .GetComponent<AICarSpawner>();        
        spawner.gameObject.SetActive(false);
    }

    public void AddDrivingScenario(List<AIDrivingAction> DrivingActions, Vector3 start, Vector3 rotation, bool destroyAfter,
        UnityAction<PassiveTaskObject> onFailState) 
    {
        AIDrivingScenario newScenario = 
            GameObject.Instantiate(drivingScenarioObjPrefab, Vector3.zero, 
                Quaternion.identity, taskObjectHolder)
            .GetComponent<AIDrivingScenario>();
        
        newScenario.startCoords = start;
        newScenario.startRotation = rotation;
        newScenario.destroyAfter = destroyAfter;
        newScenario.onEnd += CheckTaskEnd;
        newScenario.DrivingActions = DrivingActions;
        newScenario.onFailState += onFailState;
        DrivingScenarios.Add(newScenario);
        endedScenarios.Add(false);
        newScenario.gameObject.SetActive(false);
    }

    public override void SetAsActive()
    {
        spawner.gameObject.SetActive(true);
        spawner.StartSpawning(DrivingScenarios, timeDelay);
    }

    public override void SetAsInactive()
    {
        foreach (var scenario in DrivingScenarios)
            scenario.gameObject.SetActive(false);
    }

    private void CheckTaskEnd(AIDrivingScenario scenario)
    {
        int i = DrivingScenarios.IndexOf(scenario);
        endedScenarios[i] = true;

        foreach (var visited in endedScenarios) {
            if (!visited)
                return;
        }

        SetAsInactive();
    }

    IEnumerator CarSpawning()
    {
        foreach (var scenario in DrivingScenarios)
        {
            scenario.gameObject.SetActive(true);
            scenario.StartScenario();
            yield return new WaitForSeconds(timeDelay);
        }
    }  
}