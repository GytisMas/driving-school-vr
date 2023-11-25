using UnityEngine;

public abstract class ActiveTask : Task
{
    
    protected bool active = false;
    public ActiveTask(Transform holder) : base(holder)
    {
        active = false;
    }
    public abstract void SetAsActive();    
    public abstract void SetAsInactive();
}