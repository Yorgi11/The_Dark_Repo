using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject DeathMenu;
    [SerializeField] private Slider SensitivitySlider;
    [SerializeField] private Text SensitivityText;

    private PlayerCam cam;
    private void Start()
    {
        /*cam = FindObjectOfType<PlayerCam>();
        if (SensitivitySlider != null && SensitivityText != null)
        {
            SensitivitySlider.onValueChanged.AddListener((v) =>
            {
                cam.SetSensitivity(v);
                SensitivityText.text = v.ToString();
            });
        }*/
    }

    private void Update()
    {
        /*if (SceneManager.GetActiveScene().buildIndex == 0 && Input.GetKeyDown(KeyCode.Escape) && !OptionsMenu.activeInHierarchy && PauseMenu != null)
        {
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/
        if (SceneManager.GetActiveScene().buildIndex == 2) DeathMenu.SetActive(true);
    }
    public void PlayButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }
    public void LoadDeathScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void OptionsButton()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void ExitOptions()
    {
        OptionsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }
}
