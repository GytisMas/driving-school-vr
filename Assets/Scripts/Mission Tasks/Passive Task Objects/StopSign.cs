using UnityEngine;

public class StopSign : PassiveTaskObject
{
    bool carStopped;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            carStopped = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Car" && other.GetComponent<CarControllerV2>().Carspeed < 0.01f)
        {
            Debug.Log("Mldc sustojai");
            carStopped = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!carStopped)
        {
            onFailState?.Invoke(this, "Driving through stop sign without stopping");
        }

    }
}