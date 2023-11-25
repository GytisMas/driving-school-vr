using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button[] buttons;
    private int selectedButtonIndex = 0;
    private void Awake() {
    }

    void Start()
    {
        SelectPrimary();
    }

    private void DetectMenuInput()
    {
        InputLabel currentInput = CustomInput.GetInputDown(InputLabel.ALL);
        if (currentInput == InputLabel.UP
         || currentInput == InputLabel.DOWN) {
            SelectOther(currentInput);
        } else if (currentInput == InputLabel.ENTER) {
            PressButton();
        }
    }

    public void LaunchTutorial() 
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void LaunchLevel() 
    {
        SceneManager.LoadScene("City");
    }

    private void PressButton() 
    {
        buttons[selectedButtonIndex].onClick?.Invoke();        
    }

    private void SelectPrimary()
    {
        SelectButton(0);
    }

    private void SelectButton(int index)
    {
        buttons[index].Select();
        selectedButtonIndex = index;
    }

    private void SelectOther(InputLabel input) 
    {
        switch (input) {
            case InputLabel.UP:
                SelectButton(Modulo(selectedButtonIndex - 1, 4));
                break;
            case InputLabel.DOWN:
                SelectButton(Modulo(selectedButtonIndex + 1, 4));
                break;
        }
    }

    private int Modulo(int a, int b) 
    {
        int r = a % b;
        return r < 0 ? r + b : r;
    }

    private void Update()
    {
        DetectMenuInput();
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit works.");
    }
}
