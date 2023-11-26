using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntrySection : Section
{
    [SerializeField] bool isMainRoad;
    
    [HideInInspector] public UnityAction<EntrySection> onEntry;
    [HideInInspector] public bool playerHasEntered = false;
    [HideInInspector] public bool playerInSection = false;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Car") {    
            AddCarToZone(other.transform);        
            onEntry?.Invoke(this);
        } else if (other.gameObject.tag == "AICar") {            
            AddCarToZone(other.transform);        
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "AICar") {
            RemoveCarFromZone(other.transform);
        }
    }

    public override void ResetPlayerInfo()
    {
        playerHasEntered = false;
        playerInSection = false;
    }
}
