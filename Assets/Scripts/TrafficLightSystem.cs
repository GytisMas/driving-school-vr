using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrafficLightSystem : MonoBehaviour
{
    [SerializeField]private List<TrafficLight> trafficLights = new List<TrafficLight>();
    [SerializeField]private float lightsSwitchInterval = 20f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < trafficLights.Count; i++)
        {
            if ((i % 2) == 0)
            {
                trafficLights[i].SetState(TrafficLightState.Red);
            }
            else
            {
                trafficLights[i].SetState(TrafficLightState.Green);
            }
        }
        FunctionTimer.Create(SwitchStates, lightsSwitchInterval);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchStates()
    {
        foreach(TrafficLight light in trafficLights)
        {
            light.SwitchNextState();
        }
        FunctionTimer.Create(SwitchStates, lightsSwitchInterval);
    }
}
