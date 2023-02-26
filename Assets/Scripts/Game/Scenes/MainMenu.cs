using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MMenu;
    [SerializeField] private GameObject OptionsMenu;
    private bool options = false;
    public void ToggleOptionsMenu()
    {
        // flips options then sets the the options menu to be the state of options
        OptionsMenu.SetActive(options=!options);
        MMenu.SetActive(!options);
    }
    public void PlayButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
