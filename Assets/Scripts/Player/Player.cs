using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchRate;
    [SerializeField] private float jumpForce;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float sprintFOV;
    [SerializeField] private float NormalScale;
    [SerializeField] private float CrouchScale;

    [SerializeField] private GameObject HpBar;
    [SerializeField] private GameObject StaminaBar;
    [SerializeField] private GameObject AmmoImage;
    [SerializeField] private GameObject[] bulletImages;
    [SerializeField] private GameObject ReloadText;

    [SerializeField] private GameObject crossHair;

    [SerializeField] private Gun[] guns;

    private float speed;

    private int horzin;
    private int vertin;
    private int gunIndex = 0;

    private bool isGrounded;
    private bool jump;
    private bool crouch;

    private Camera cam;
    private PlayerCam playerCam;
    private Rigidbody rb;
    private Move move;
    private StatsSystem stats;
    private Gun currentGun;
    void Start()
    {
        // grabs the player camera object
        playerCam = FindObjectOfType<PlayerCam>();
        // grabs the Movescript attacthed to the player object
        move = GetComponent<Move>();
        // grabs the stats script
        stats = GetComponent<StatsSystem>();
        // grabs the Rigidbody2D attacthed to the player object
        rb = GetComponent<Rigidbody>();
        // gets the main camera
        cam = Camera.main;

        speed = walkSpeed;

        for (int i=0; i<guns.Length; i++)
        {
            guns[i].SetDefaultFOV(defaultFOV);
        }

        currentGun = guns[gunIndex];
        currentGun.gameObject.SetActive(true);
    }
    void Update()
    {
        // gets user input based on unity axis definitions (in this case wasd, LeftArrow RightArrow UpArrow DownArrow)
        horzin = (int)Input.GetAxisRaw("Horizontal");
        vertin = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space)) jump = true;
        else jump = false;

        if (Input.GetKey(KeyCode.LeftShift) && vertin > 0 && !currentGun.GetIsAiming()) speed = sprintSpeed;
        else if (Input.GetKeyDown(KeyCode.LeftShift)) playerCam.SetSensitivity(playerCam.GetSensitivity() * currentGun.GetConcentration());
        else if (Input.GetKeyUp(KeyCode.LeftShift)) playerCam.SetSensitivity(playerCam.GetSensitivity());
        else speed = walkSpeed;
        if (crouch) speed = crouchSpeed;

        if (speed == sprintSpeed && (horzin != 0 || vertin != 0)) playerCam.ChangeFOV(sprintSpeed, sprintFOV);
        else playerCam.ChangeFOV(sprintSpeed, defaultFOV);

        if (Input.GetKey(KeyCode.LeftControl) && !jump) Crouch(true);
        else Crouch(false);

        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            currentGun.gameObject.SetActive(false);
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) gunIndex++;
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) gunIndex--;
            if (gunIndex >= guns.Length) gunIndex = 0;
            if (gunIndex < 0) gunIndex = guns.Length - 1;
            currentGun = guns[gunIndex];
            currentGun.gameObject.SetActive(true);
            Debug.Log(gunIndex);
        }

        stats.RemoveStamina(rb.velocity.magnitude > walkSpeed ? rb.velocity.magnitude*1.25f:rb.velocity.magnitude*0.25f);
        if (rb.velocity.magnitude <= 0.001f) stats.RecoverStamina();
        Debug.Log(stats.GetStamina());

        // limits the magnitude of the player objects velocity 
        move.SpeedLimit3D(speed, rb);

        // UI
        if (Input.GetKeyDown(KeyCode.L)) stats.TakeDamage(10f);
        if (HpBar != null) HpBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, stats.GetHp()-96, 0);
        if (StaminaBar != null) StaminaBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(stats.GetStamina()-96, 0, 0);
        if (AmmoImage != null) AmmoImageHandler();
        if (ReloadText != null && GetComponent<Attack>().GetIsReloading()) ReloadText.SetActive(true);
        else if (ReloadText != null) ReloadText.SetActive(false);

        if (currentGun.GetIsAiming()) crossHair.SetActive(false);
        else crossHair.SetActive(true);
    }

    private void FixedUpdate()
    {
        // calls the move fuction to move the player
        if (isGrounded) move.Move3DForce(speed, rb, transform.forward * vertin, transform.right * horzin);
        // calls the jump function to make the player jump
        if (isGrounded && jump) move.Jump3D(jumpForce, rb, transform.up);
    }

    private void Crouch(bool state)
    {
        Vector3 temp;
        crouch = state;
        if (crouch) temp = new Vector3(1f, CrouchScale, 1f);
        else temp = new Vector3(1f, NormalScale, 1f);
        transform.localScale = Vector3.Lerp(transform.localScale, temp, Time.deltaTime * crouchRate);
    }

    private void AmmoImageHandler()
    {
        if (currentGun.GetAmmo() == bulletImages.Length)
        {
            for (int i=0;i<bulletImages.Length;i++)
            {
                bulletImages[i].SetActive(true);
            }
        }
        if (bulletImages.Length - currentGun.GetAmmo() > 0)
        {
            bulletImages[bulletImages.Length - currentGun.GetAmmo() - 1].SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6) isGrounded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6) isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6) isGrounded = false;
    }

    public Gun GetCurrentGun(){
        return currentGun;
    }
}
