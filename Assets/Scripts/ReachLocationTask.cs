using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ReachLocationTask : MonoBehaviour
{
    Collider collider;
    public delegate void ParkingMissionCompleted();
    public static event ParkingMissionCompleted OnParkingMissionCompleted;
    private int currentParkingIndex = 0;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Car")
        {
            if (collider.bounds.Contains(other.bounds.max) && collider.bounds.Contains(other.bounds.min))
            {
                Debug.Log("Priparkuota");
                // Fire event when the car is parked
                collider.gameObject.SetActive(false);
                OnParkingMissionCompleted?.Invoke();
            }
            else
            {

            }
        }
    }

}
