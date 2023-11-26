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
            onEntry?.Invoke(this);
        }
    }

    public override void ResetPlayerInfo()
    {
        playerHasEntered = false;
        playerInSection = false;
    }
}
