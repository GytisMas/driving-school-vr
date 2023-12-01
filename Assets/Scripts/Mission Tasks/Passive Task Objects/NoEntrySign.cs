using UnityEngine;

public class NoEntrySign : PassiveTaskObject
{
    Transform car; 
    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Car") {
            float angle = Vector3.Angle(transform.right, other.transform.forward);
            if (angle > 90f) {
                onFailState?.Invoke(this, "Driving through no entry sign");
            }
        }
    }
}