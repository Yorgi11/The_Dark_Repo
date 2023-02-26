using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private Player player;
    [SerializeField] private float sensitivity = 200;
    private float mousex = 0;
    private float mousey = 0;
    private float yRot = 0;
    private float xRot = 0;
    private float resFactor;
    private float t;
    private Vector3 camrec =Vector3.zero;
    private Camera thisCamera;
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        //if (Screen.currentResolution > )
        char[] screen = Screen.currentResolution.ToString().ToCharArray();
        string width = "";
        for (int i=0;i<4;i++)
        {
            width += screen[i];
        }
        if (int.Parse(width) != 1920) resFactor = int.Parse(width) / 1920;
        else resFactor = 1f;
        player = FindObjectOfType<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        mousex = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensitivity * resFactor;
        mousey = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensitivity * resFactor;

        // for use with camera recoil
        yRot += mousex + camrec.y;
        xRot -= mousey + camrec.x;

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

        // for crouching
        transform.localPosition = new Vector3(0f, 1.7f * player.transform.localScale.y, 0f);
    }
    public void CamRecoil(Vector3 rot)
    {
        camrec = rot * 0.1f;
    }
    public void ChangeFOV(float speed, float fov)
    {
        float dur = Mathf.Abs(thisCamera.fieldOfView - fov) / (speed * 0.4f);
        t += Time.deltaTime;
        float percent = Mathf.SmoothStep(0, 1, t / dur);
        thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, fov, percent);
        if (t >= 1) t = 0;
    }
}
