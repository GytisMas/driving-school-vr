using UnityEngine;
using UnityEngine.Events;

public abstract class Task 
{
    public UnityAction<Task> onComplete;

    protected bool completed;
    protected bool active;
    protected Transform taskObjectHolder;
    
    public Task(Transform holder) 
    {
        taskObjectHolder = holder;
        completed = false;
        active = false;
    }

    public abstract void SetAsActive();    
    public abstract void SetAsInactive();
}