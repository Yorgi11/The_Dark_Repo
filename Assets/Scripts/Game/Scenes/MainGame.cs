using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject Game;
    private bool pause = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePauseMenu();
    }
    public void TogglePauseMenu()
    {
        // flips options then sets the the options menu to be the state of options
        PauseMenu.SetActive(pause = !pause);
        Game.SetActive(!pause);
        MainUI.SetActive(!pause);
    }
    public void ResumeButton()
    {
        TogglePauseMenu();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void LoadDeathScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }
}
