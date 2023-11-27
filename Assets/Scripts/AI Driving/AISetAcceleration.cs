using UnityEngine;

public class AISetAcceleration : AIDrivingAction
{
    public override bool waitForEnd => false;
    public float acceleration = 0f;
    public bool waitForStop = false;
    public AISetAcceleration(float _acceleration) 
    {
        acceleration = _acceleration;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.SetAcceleration(acceleration);
        return GetDelayOrNull();
    }
}