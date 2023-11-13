using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ReachLocationTask : Task
{
    private GameObject locationSpotPrefab;
    private LocationSpot locationSpot;

    public ReachLocationTask(Transform holder) : base(holder) 
    {
        locationSpotPrefab = (GameObject)Resources.Load("MissionObjects/LocationToEnter");
    }

    public void AddLocationSpot(Vector3 coords, float yRot) 
    {
        locationSpot = 
            GameObject.Instantiate(locationSpotPrefab, coords, 
                Quaternion.identity, taskObjectHolder)
            .GetComponent<LocationSpot>();
        locationSpot.transform.eulerAngles = new Vector3(
            locationSpot.transform.eulerAngles.x,
            yRot,
            locationSpot.transform.eulerAngles.z
        );
        locationSpot.gameObject.SetActive(false);
        locationSpot.onSuccess += DetectReachedSpot;
    }

    public override void SetAsActive()
    {
        active = true;
        locationSpot.gameObject.SetActive(true);
    }

    public override void SetAsInactive()
    {
        active = false;
        locationSpot.gameObject.SetActive(false);
    }

    private void DetectReachedSpot(LocationSpot loc) 
    {
        onComplete?.Invoke(this);
    }    
}