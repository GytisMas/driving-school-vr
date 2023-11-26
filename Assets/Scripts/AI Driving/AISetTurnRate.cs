using UnityEngine;

public class AISetTurnRate : AIDrivingAction
{
    public override bool waitForEnd => false;
    public float turnRate = 0f;
    public AISetTurnRate(float _turnRate) 
    {
        turnRate = _turnRate;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.SetTurnRate(turnRate);
        return GetDelayOrNull();
    }
}