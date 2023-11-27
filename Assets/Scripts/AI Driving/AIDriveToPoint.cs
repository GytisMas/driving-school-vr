using UnityEngine;
public class AIDriveToPoint : AIDrivingAction
{
    public override bool waitForEnd => true;
    public Vector3 targetPos;
    public float targetYRot;
    public AIDriveToPoint(Vector3 _targetPos, float _targetYRot = 0f) {
        targetPos = _targetPos;
        targetYRot = _targetYRot;
    }

    public override WaitForSeconds Execute(AIDriver carDriver)
    {
        carDriver.DriveToPoint(targetPos, targetYRot);
        return GetDelayOrNull();
    }
}