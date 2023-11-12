using UnityEngine;

public class TargetButton 
{
    public KeyCode keyCode;
    public InputLabel inputLabel;
    public string wheelAxis;
    public float axisTarget;
    public TargetButton(KeyCode kc = KeyCode.None, InputLabel wheel = InputLabel.NONE, string axis = "", float target = -1f) 
    {
        if (kc == KeyCode.None && wheel == InputLabel.NONE && axis == "" && target == -1f) {
            Debug.LogWarning("Creating target button with no params set");
            return;
        }
        
        keyCode = kc;
        inputLabel = wheel;
        wheelAxis = axis;
        axisTarget = target;
    }
}