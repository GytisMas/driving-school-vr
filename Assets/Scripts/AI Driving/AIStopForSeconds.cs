using UnityEngine;

public class AIStopForSeconds : AIDrivingAction
{
    public override bool waitForEnd => true;
    public float time = 0f;
    public AIStopForSeconds(float _time) 
    {
        time = _time;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.StopForSeconds(time);
        return GetDelayOrNull();
    }
}