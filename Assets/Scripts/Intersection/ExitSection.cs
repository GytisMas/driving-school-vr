using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitSection : Section
{
    [SerializeField] EntrySection[] ValidEntrySections;
    [HideInInspector] public UnityAction<ExitSection> onSuccessfulPass;

    public override void ResetPlayerInfo()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Car") {
            onZoneEntry?.Invoke(this, other.transform);
            bool hasValidEntry = false;
            foreach (var e in ValidEntrySections) {
                if (e.playerHasEntered) {
                    hasValidEntry = true;
                    break;
                }
            }
            
            if (!hasValidEntry) {
                onFail?.Invoke("Entering or exiting intersection through the wrong lane");
            }
            onSuccessfulPass?.Invoke(this);
        }
    }
}
