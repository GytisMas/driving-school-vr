using UnityEngine;

public class AISetSpeed : AIDrivingAction
{
    public override bool waitForEnd => false;
    public float speed = 0f;
    public AISetSpeed(float _speed) 
    {
        speed = _speed;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.SetSpeed(speed);
        return GetDelayOrNull();
    }
}