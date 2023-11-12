using UnityEngine;
using UnityEngine.Events;

public class ParkingSpot : MonoBehaviour
{
    public UnityAction<ParkingSpot> onSuccess;    
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Car")
        {
            if (col.bounds.Contains(other.bounds.max) && col.bounds.Contains(other.bounds.min))
            {
                gameObject.SetActive(false);
                onSuccess?.Invoke(this);
            }
        }
    }
}