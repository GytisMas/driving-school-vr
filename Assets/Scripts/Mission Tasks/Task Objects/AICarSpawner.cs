using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class AICarSpawner : MonoBehaviour
{
    public void StartSpawning(List<AIDrivingScenario> scenarios, float timeDelay)
    {
        StartCoroutine(SpawnCars(scenarios, timeDelay));
    }

    IEnumerator SpawnCars(List<AIDrivingScenario> scenarios, float timeDelay)
    {
        foreach (var scenario in scenarios)
        {
            scenario.gameObject.SetActive(true);
            scenario.StartScenario();
            yield return new WaitForSeconds(timeDelay);
        }
    }
}