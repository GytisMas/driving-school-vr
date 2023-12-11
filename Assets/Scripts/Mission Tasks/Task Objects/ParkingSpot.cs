using System;
using UnityEngine;
using UnityEngine.Events;

public class ParkingSpot : MonoBehaviour
{
    public UnityAction<ParkingSpot> onSuccess;    
    private Collider col;
    GameObject wrlObj;
    Collider leftSide;
    BoxCollider wrlCollider;
    void Start()
    {
        col = GetComponent<Collider>();
    }

    void OnTriggerStay(Collider other)
    {
        // Debug.Log(other.name);
        if (other.tag == "Car")
        {
            leftSide = transform.Find("LeftSide").GetComponent<Collider>();
            wrlObj = other.gameObject.transform.GetChild(3).gameObject;
         
            wrlCollider = wrlObj.GetComponent<BoxCollider>();
            if (col.bounds.Contains(other.bounds.max) && col.bounds.Contains(other.bounds.min)
                && leftSide.bounds.Contains(wrlCollider.bounds.max) && leftSide.bounds.Contains(wrlCollider.bounds.min))
            {
                gameObject.SetActive(false);
                onSuccess?.Invoke(this);
            }
            else
            {
                Debug.Log("Ne taip priparkavai");
            }
        }
    }
}