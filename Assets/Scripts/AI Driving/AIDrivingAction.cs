using UnityEngine;
using UnityEngine.AI;

public abstract class AIDrivingAction
{
    public abstract bool waitForEnd { get; }
    public abstract WaitForSeconds Execute(AIDriver carDriver);
    public float delayAfterAction = 0f;

    public WaitForSeconds GetDelayOrNull()
    {
        return delayAfterAction > 0f
            ? new WaitForSeconds(delayAfterAction)
            : null;
    }
}