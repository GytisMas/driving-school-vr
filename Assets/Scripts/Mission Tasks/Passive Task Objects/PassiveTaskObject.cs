using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PassiveTaskObject : MonoBehaviour 
{
    public UnityAction<PassiveTaskObject> onFailState;
}