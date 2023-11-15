using UnityEngine;
using UnityEngine.Events;

public abstract class Task 
{
    public UnityAction<ActiveTask> onComplete;
    protected bool completed = false;
    protected Transform taskObjectHolder;
    
    public Task(Transform holder) 
    {
        taskObjectHolder = holder;
        completed = false;
    }

    public virtual void FailState() {}
}