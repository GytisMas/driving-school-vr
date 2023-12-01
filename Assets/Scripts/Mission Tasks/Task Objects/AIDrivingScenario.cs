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
    [HideInInspector] public UnityAction<PassiveTaskObject, string> onFailState;
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

            // coroutine-based actions 
            // may require to wait for end
            if (action.waitForEnd)
                while (!carDriver.NextActionIsReady)
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
}