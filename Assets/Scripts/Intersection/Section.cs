using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class Section : MonoBehaviour 
{
    [SerializeField] BoxCollider col;
    [HideInInspector] public UnityAction onFail;
    [HideInInspector] public UnityAction<Section, Transform> onZoneEntry;
    [HideInInspector] public int CarsInZoneCount => CarsInZone.Count;
    protected List<Transform> CarsInZone = new List<Transform>();

    public abstract void ResetPlayerInfo();

    public int CarsInZoneWithout(Transform t)
    {
        if (CarsInZone.Contains(t))
            return CarsInZone.Count - 1;
        return CarsInZone.Count;
    }
    
    protected void AddCarToZone(Transform t)
    {
        CarsInZone.Add(t);
        onZoneEntry?.Invoke(this, t);
    }

    protected int RemoveCarFromZone(Transform t)
    {
        int index = CarsInZone.IndexOf(t);
        CarsInZone.Remove(t);
        return index;
    } 
}