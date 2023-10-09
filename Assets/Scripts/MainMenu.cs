using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button primaryButton;
    void Start()
    {
        primaryButton.Select();
    }
    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit works.");
    }
}
