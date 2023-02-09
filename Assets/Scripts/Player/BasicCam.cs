using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCam : MonoBehaviour
{
    private BasicPlayer player;
    [SerializeField] private float sensitivity = 200;
    private float mousex = 0;
    private float mousey = 0;
    private float yRot = 0;
    private float xRot = 0;
    void Start()
    {
        player = FindObjectOfType<BasicPlayer>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        mousex = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRot += mousex;
        xRot -= mousey;

        if (xRot < -89.5f)
        {
            xRot = -89.5f;
        }
        else if (xRot > 89.5f)
        {
            xRot = 89.5f;
        }

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        player.transform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
