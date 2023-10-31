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
    
    void Start()
    {
        mission1.GetComponentInChildren<Text>().text = "Press W to drive forward";
        mission2.GetComponentInChildren<Text>().text = "Press A to turn steering wheel left";
        mission3.GetComponentInChildren<Text>().text = "Press D to turn steering wheel right";
        mission4.GetComponentInChildren<Text>().text = "Press S to drive backwards";
        mission5.gameObject.SetActive(false);
        mission6.gameObject.SetActive(false);
        mission5.GetComponentInChildren<Text>().text = "Press Q to show left turn signal";
        mission6.GetComponentInChildren<Text>().text = "Press E to to show right turn signal";

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            mission1.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            mission2.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            mission3.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            mission4.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if(Input.GetKeyDown(KeyCode.Q) && mission1.isOn)
        {
            mission5.isOn = true;
            Invoke("ChangeMission", 3);
        }
        if (Input.GetKeyDown(KeyCode.E) && mission2.isOn)
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
