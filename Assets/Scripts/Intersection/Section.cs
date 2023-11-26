using UnityEngine;
using UnityEngine.Events;
public abstract class Section : MonoBehaviour 
{
    [SerializeField] BoxCollider col;
    [HideInInspector] public UnityAction onFail;

    public abstract void ResetPlayerInfo();
}