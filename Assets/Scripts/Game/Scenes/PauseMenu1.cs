using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu1 : MonoBehaviour
{

    private MainHub Hub;

    [SerializeField ]private GameObject PauseMenuUI;


    private void Start()
    {
        Hub = FindObjectOfType<MainHub>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuUI.SetActive(true);
            Hub.DisableMouse = true;
            Time.timeScale = 0.001f;
            
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Hub.DisableMouse = false;
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitFromPause()
    {
        Debug.Log("Quit Game From pause Menu");
        Application.Quit();
    } 

}
