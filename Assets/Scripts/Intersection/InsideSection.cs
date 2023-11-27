using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelativeToFirst 
{
    Straight,
    LeftTurn,
    Equal,
    Other,
}
public class InsideSection : Section
{
    [SerializeField] private InsideSection leftTurnSection;
    [SerializeField] private InsideSection straightSection;
    [SerializeField] private List<Section> YieldFirst = new List<Section>(); 
    [SerializeField] private List<Section> YieldStraight = new List<Section>(); 
    [SerializeField] private List<Section> YieldLeftTurn = new List<Section>();
    private List<RelativeToFirst> CarsInZoneDoingLeftTurn = new List<RelativeToFirst>();
    private List<bool> CanCheckCar = new List<bool>();

    public override void ResetPlayerInfo() { }

    public InsideSection leftTurn {
        get {
            return leftTurnSection;
        }
    }

    public InsideSection straightSec {
        get {
            return straightSection;
        }
    }

    public void SetCarRelativeToFirst(Transform car, RelativeToFirst relative) 
    {
        int carIndex = CarsInZone.IndexOf(car);
        CarsInZoneDoingLeftTurn[carIndex] = relative;
        CanCheckCar[carIndex] = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "AICar") {
            CarsInZoneDoingLeftTurn.Add(RelativeToFirst.Equal);
            CanCheckCar.Add(false);
            AddCarToZone(other.transform);
            
            if (other.gameObject.tag == "Car" && CarsInZoneWithout(other.transform) > 0)
                onFail?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Car" || other.gameObject.tag == "AICar") {
            int carIndex = RemoveCarFromZone(other.transform);    
            CarsInZoneDoingLeftTurn.RemoveAt(carIndex);    
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Car") {
            int carIndex = CarsInZone.IndexOf(other.transform);
            if (!CanCheckCar[carIndex])
                return;

            List<Section> YieldTo = new List<Section>();
            switch (CarsInZoneDoingLeftTurn[carIndex]) {
                case RelativeToFirst.Straight:
                    YieldTo = YieldStraight;
                    break;
                case RelativeToFirst.LeftTurn:
                    YieldTo = YieldLeftTurn;
                    break;
                case RelativeToFirst.Equal:
                    YieldTo = YieldFirst;
                    break;
            }

            foreach(var section in YieldTo) {
                int carCount = section.CarsInZoneWithout(other.transform);                    
                if (carCount > 0) {
                    onFail?.Invoke();
                }
            }
        }
    }
}
