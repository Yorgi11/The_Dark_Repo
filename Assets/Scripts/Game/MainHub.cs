using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHub : MonoBehaviour
{
    private int enemiesKilled = 0;

    private bool disableMouse = false;

    private void Update()
    {
        if (disableMouse && Input.GetKeyDown(KeyCode.Escape)) disableMouse = false;
        if (disableMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public bool DisableMouse
    {
        get { return disableMouse; }
        set { disableMouse = value; }
    }
    public int EnemiesKilled
    {
        get { return enemiesKilled; }
        set { enemiesKilled = value; }
    }
}
