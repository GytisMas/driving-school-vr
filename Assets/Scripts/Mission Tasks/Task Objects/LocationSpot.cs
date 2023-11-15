using UnityEngine;
using UnityEngine.Events;

public class LocationSpot : MonoBehaviour
{
    public UnityAction<LocationSpot> onSuccess;    
    private Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            gameObject.SetActive(false);
            onSuccess?.Invoke(this);
        }
    }
}