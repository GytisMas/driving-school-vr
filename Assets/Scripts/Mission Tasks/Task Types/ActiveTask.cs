using Unity.VisualScripting;
using UnityEngine;

public abstract class ActiveTask : Task
{
    
    public bool active { get; protected set; }
    public ActiveTask(Transform holder) : base(holder)
    {
        active = false;
    }
    public abstract void SetAsActive();    
    public abstract void SetAsInactive();
}