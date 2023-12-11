using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private bool isInGameMenu = false;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject menuPanel;
    private int selectedButtonIndex = 0;
    private bool canDetectInputThisFrame = true;
    private void Awake() {
    }

    void Start()
    {
        SelectPrimary();
        HideMenu();
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
        SceneManager.LoadScene("ParkingLot");
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

    public void ResumeGame()
    {
        HideMenu();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
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
                SelectButton(Modulo(selectedButtonIndex - 1, buttons.Length));
                break;
            case InputLabel.DOWN:
                SelectButton(Modulo(selectedButtonIndex + 1, buttons.Length));
                break;
        }
    }

    private int Modulo(int a, int b) 
    {
        int r = a % b;
        return r < 0 ? r + b : r;
    }

    public void ShowMenu()
    {

        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
        }
    }

    public void HideMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }
    private void ToggleOpenMenu()
    {
        if (!isInGameMenu)
            return;
        if (CustomInput.GetInputDown(InputLabel.ESCAPE) != 0)
        {
            ToggleMenu();
            canDetectInputThisFrame = false;
        } else {
            canDetectInputThisFrame = true;
        }
    }

    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }
    }

    private void Update()
    {
        ToggleOpenMenu();
        if (ActiveMenu())
            DetectMenuInput();
    }

    private bool ActiveMenu()
    {
        return !isInGameMenu || (canDetectInputThisFrame && menuPanel != null && menuPanel.activeSelf);
    }

    public void OnQuit()
    {
        Application.Quit();
        Debug.Log("Quit works.");
    }
}
