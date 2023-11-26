using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AIDrivingScenario : MonoBehaviour
{
    [SerializeField] GameObject aiCarPrefab;
    public Vector3 startCoords;
    public Vector3 startRotation;
    public bool destroyAfter;
    [HideInInspector] public UnityAction<PassiveTaskObject> onFailState;
    [HideInInspector] public List<AIDrivingAction> DrivingActions;
    [HideInInspector] public UnityAction<AIDrivingScenario> onEnd;
    private AIDriver carDriver;
    

    public void StartScenario() 
    {
        StartCoroutine(DrivingScenario());
    }

    IEnumerator DrivingScenario()
    {
        SetNewAICar();
        foreach (var action in DrivingActions) {
            yield return action.Execute(carDriver);

            if (action.waitForEnd)
                while (carDriver.NextActionIsReady)
                    yield return null;
        }
        if (destroyAfter)
            carDriver.Despawn();
        onEnd?.Invoke(this);
    }

    void SetNewAICar()
    {
        carDriver = Instantiate(aiCarPrefab, 
         startCoords, Quaternion.identity)
            .GetComponent<AIDriver>();
        carDriver.transform.Rotate(startRotation);
        carDriver.onFailState += onFailState;
    }

    IEnumerator DrivingScenarioOld()
    {
        carDriver = Instantiate(aiCarPrefab, 
         startCoords, Quaternion.identity)
            .GetComponent<AIDriver>();
        carDriver.transform.Rotate(startRotation);

        carDriver.SetSpeed(15f);
        carDriver.SetAcceleration(1f);
        yield return new WaitForSeconds(3f);

        carDriver.SetTurnRate(0.5f);
        yield return new WaitForSeconds(3f);

        carDriver.StopForSeconds(7f);
        while (!carDriver.NextActionIsReady)
            yield return null;

    }
}