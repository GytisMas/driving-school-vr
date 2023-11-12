using UnityEngine;
using UnityEngine.Events;
public enum ButtonType 
{
    NONE = 0,
    KEYBOARD = 1,
    WHEELB = 2,
    WHEELAXIS = 4
}
public class ButtonDetector : MonoBehaviour
{
    public UnityAction<ButtonDetector> onSuccess;    
    private ButtonType buttonType;
    private KeyCode keyCode;
    private InputLabel wheelButton;
    private string wheelAxis;
    private float wheelAxisTarget;

    public void SetTargetButtons(KeyCode kc = KeyCode.None, InputLabel wheel = InputLabel.NONE, string axis = "", float target = -1f) 
    {
        buttonType = ButtonType.NONE;
        if (kc != KeyCode.None) {
            keyCode = kc;
            buttonType = buttonType | ButtonType.KEYBOARD;
        }
        if (wheel != InputLabel.NONE) {
            wheelButton = wheel;
            buttonType = buttonType | ButtonType.WHEELB;
        }
        if (axis != "") {
            wheelAxis = axis;
            wheelAxisTarget = target;
            buttonType = buttonType | ButtonType.WHEELAXIS;
        }
    }

    void Update() 
    {
        bool pressed = false;
        if (LogitechSteeringWheel.wheelConnected) {
            if ((buttonType & ButtonType.WHEELB) != 0
             && CustomInput.GetInputDown(wheelButton) != 0)
                pressed = true;
            else if ((buttonType & ButtonType.WHEELAXIS) != 0
             && CustomInput.GetAxisNormalised(wheelAxis) <= wheelAxisTarget)
                pressed = true;
        } else if ((buttonType & ButtonType.KEYBOARD) != 0 && Input.GetKeyDown(keyCode)) {
            pressed = true;
        }
        if (pressed) {
            gameObject.SetActive(false);
            onSuccess?.Invoke(this);
        }
    }
}