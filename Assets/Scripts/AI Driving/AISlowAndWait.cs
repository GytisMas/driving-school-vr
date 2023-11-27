using UnityEngine;

public class AISlowAndWait : AIDrivingAction
{
    public override bool waitForEnd => true;
    public float time = 0f;
    public float targetSpeed = 0f;
    public AISlowAndWait(float _time, float _targetSpeed = 0f) 
    {
        time = _time;
        targetSpeed = _targetSpeed;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.SlowDownAndWait(time, targetSpeed);
        return GetDelayOrNull();
    }
}