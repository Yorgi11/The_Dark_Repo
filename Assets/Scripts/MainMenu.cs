using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options;
    // Start is called before the first frame update
    void Start()
    {
        options.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        if(options.activeInHierarchy) options.SetActive(false);
        else options.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
