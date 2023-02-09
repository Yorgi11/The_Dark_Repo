using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private Player player;
    [SerializeField] private float sensitivity = 200;
    [SerializeField] private bool usingRecoil;
    private float defaultSens;
    private float mousex = 0;
    private float mousey = 0;
    private float yRot = 0;
    private float xRot = 0;
    private float t = 0;

    private Vector2 multiplier = new Vector2(1,1);

    private Camera thisCamera;
    void Start()
    {
        player = FindObjectOfType<Player>();
        thisCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultSens = sensitivity;
    }
    void Update()
    {
        mousex = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        // for use with camera recoil
        yRot += mousex + (usingRecoil ? (player.GetCurrentGun().GetComponent<WeaponAnimations>().GetCurrentRotation().y * multiplier.y) : 0f);
        xRot -= mousey + (usingRecoil ? (-player.GetCurrentGun().GetComponent<WeaponAnimations>().GetCurrentRotation().x * multiplier.x) : 0f);

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

        transform.localPosition = new Vector3(0f, 1.7f * player.transform.localScale.y, 0f);
    }
    public void ChangeFOV(float speed, float fov)
    {
        float dur = Mathf.Abs(thisCamera.fieldOfView - fov) / (speed * 0.4f);
        t += Time.deltaTime;
        float percent = Mathf.SmoothStep(0, 1, t / dur);
        thisCamera.fieldOfView = Mathf.Lerp(thisCamera.fieldOfView, fov, percent);
        if (t>=1) t = 0;
    }
    public void SetMultiplier(Vector2 multi)
    {
        multiplier = multi;
    }
    public void SetSensitivity(float val)
    {
        sensitivity = val;
    }
    public float GetSensitivity()
    {
        return sensitivity;
    }
    public float GetDefaultSens()
    {
        return defaultSens;
    }
}
