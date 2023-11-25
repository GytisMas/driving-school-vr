using UnityEngine;

public class StopSign : PassiveTaskObject
{
    bool carStopped;
    CarControllerV2 carController;

    void Start()
    {
        carController = FindObjectOfType<CarControllerV2>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Car" && carController.Carspeed <= 0f && carController.AccelerationInput == 0f)
        {
            Debug.Log("Mldc sustojai");
            carStopped = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (!carStopped)
        {
            onFailState?.Invoke(this);
        }
       
    }
}