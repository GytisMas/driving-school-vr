using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightTask : PassiveTaskObject
{
    TrafficLightState state;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            state = transform.parent.gameObject.GetComponent<TrafficLight>().GetState();
            if (state != TrafficLightState.Green)
            {
                Debug.Log("Ran through non-green light");
                onFailState?.Invoke(this);
            }
        }

        

    }
}
