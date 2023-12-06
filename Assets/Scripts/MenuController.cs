using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;

    private void Start()
    {
        HideMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = menuPanel.activeSelf ? 0f : 1f; 
        }
    }

    public void ShowMenu()
    {

        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HideMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f; 
        }
    }
    public void ResumeGame()
    {
        HideMenu();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        Time.timeScale = 1f; 
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");

    }
}
