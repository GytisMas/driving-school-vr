using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMissions : MonoBehaviour
{
    public Toggle mission1;
    public Toggle mission2;
    public Toggle mission3;
    public Toggle mission4;
    public Toggle mission5;
    public Toggle mission6;

    private bool startWheelDataSet = false;
    private float startWheelSteeringAxis = 0f;
    
    void Start()
    {
        mission1.GetComponentInChildren<Text>().text = "Press W or press on the gas pedal to drive forward";
        mission2.GetComponentInChildren<Text>().text = "Press A or turn steering wheel left to steer left";
        mission3.GetComponentInChildren<Text>().text = "Press D or turn steering wheel right to steer right";
        mission4.GetComponentInChildren<Text>().text = "Press S or press on the brake pedal to stop or drive backwards";
        mission5.gameObject.SetActive(false);
        mission6.gameObject.SetActive(false);
        mission5.GetComponentInChildren<Text>().text = "Press Q or L3 to show left turn signal";
        mission6.GetComponentInChildren<Text>().text = "Press E or R3 to to show right turn signal";

    }

    float SteeringDiff() 
    {
        return CustomInput.GetAxisNormalised("Steering") - startWheelSteeringAxis;
    }

    void Update()
    {
        if (!startWheelDataSet && LogitechSteeringWheel.wheelConnected) {
            startWheelSteeringAxis = CustomInput.GetAxisNormalised("Steering");
            if (CustomInput.GetAxisNormalised("Brake") != 0)
                startWheelDataSet = true;
        }
        if (Input.GetKeyDown(KeyCode.W) || (startWheelDataSet && CustomInput.GetAxisNormalised("Gas") <= .7f))
        {
            mission1.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.A) || (startWheelDataSet && SteeringDiff() <= -0.3f))
        {
            mission2.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.D) || (startWheelDataSet && SteeringDiff() >= 0.3f))
        {
            mission3.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.S) || (startWheelDataSet && CustomInput.GetAxisNormalised("Brake") <= .7f))
        {
            mission4.isOn = true;
            Invoke("ChangeMission", 3);
        }
        InputLabel currentInput = CustomInput.GetInputDown(InputLabel.ALL);

        if(currentInput == InputLabel.L3 && mission1.isOn)
        {
            mission5.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (currentInput == InputLabel.R3 && mission2.isOn)
        {
            mission6.isOn = true;
            Invoke("ChangeMission", 3);
        }
        //Tokiu paciu principu bus galima su collisionais suzaist.
    }
    void ChangeMission()
    {
        if (mission1.isOn) {
            mission1.gameObject.SetActive(false);
            mission5.gameObject.SetActive(true);
        }
        if (mission2.isOn)
        {
            mission2.gameObject.SetActive(false);
            mission6.gameObject.SetActive(true);
        }
        if (mission3.isOn)
        {
            mission3.gameObject.SetActive(false);
            
        }
        if (mission4.isOn)
        {
            mission4.gameObject.SetActive(false);
            
        }
        if (mission5.isOn)
        {
            mission5.gameObject.SetActive(false);
            
        }
        if (mission6.isOn)
        {
            mission6.gameObject.SetActive(false);
            
        }

    }


}
