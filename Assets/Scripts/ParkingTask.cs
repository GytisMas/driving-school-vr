using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ParkingTask : Task
{
    private GameObject parkingSpotPrefab;
    private List<ParkingSpot> parkingSpots;
    private List<bool> parkingSpotsVisited;

    public ParkingTask(Transform holder) : base(holder) 
    {
        parkingSpotPrefab = (GameObject)Resources.Load("MissionObjects/ParkingSpot");
        parkingSpots = new List<ParkingSpot>();
        parkingSpotsVisited = new List<bool>();
    }

    public void AddParkingSpot(Vector3 coords, float yRot) 
    {
        ParkingSpot parkSpot = 
            GameObject.Instantiate(parkingSpotPrefab, coords, 
                Quaternion.identity, taskObjectHolder)
            .GetComponent<ParkingSpot>();
        parkSpot.transform.eulerAngles = new Vector3(
            parkSpot.transform.eulerAngles.x,
            yRot,
            parkSpot.transform.eulerAngles.z
        );
        parkingSpots.Add(parkSpot);
        parkingSpotsVisited.Add(false);
        parkSpot.gameObject.SetActive(false);
        parkSpot.onSuccess += DetectParkedSpot;
    }

    public override void SetAsActive()
    {
        active = true;
        foreach (var loc in parkingSpots)
            loc.gameObject.SetActive(true);
    }

    public override void SetAsInactive()
    {
        active = false;
        foreach (var loc in parkingSpots)
            loc.gameObject.SetActive(false);
    }

    private void DetectParkedSpot(ParkingSpot loc) 
    {
        int index = parkingSpots.IndexOf(loc);
        parkingSpotsVisited[index] = true;
        
        bool allVisited = true;
        foreach (var visited in parkingSpotsVisited) {
            if (!visited) {
                allVisited = false;
                break;
            }
        }
           
        if (allVisited)
            onComplete?.Invoke(this);
    }    
}