using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float floatForce;
    [SerializeField] private float walkForce;
    [SerializeField] private float runForce;
    [SerializeField] private float crouchForce;
    [SerializeField] private float noStaminaForce;
    [SerializeField] private float swimForce;
    [SerializeField] private float minStamina;
    [SerializeField] private float maxSwimSpeed;
    [SerializeField] private float slowForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sprintFOV;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float CrouchScale;
    [SerializeField] private float NormalScale;
    [SerializeField] private float crouchRate;
    [SerializeField] private LayerMask groundlayers;
    [SerializeField] private Transform[] GChecks;
    [SerializeField] private Gun[] guns;
    [SerializeField] private GameObject HpBar;
    [SerializeField] private GameObject StaminaBar;
    [SerializeField] private GameObject AmmoImage;
    [SerializeField] private GameObject[] bulletImages;
    [SerializeField] private GameObject ReloadText;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject crossHair;

    private float stealth = 0f;

    private float horzin = 0f, vertin = 0f;
    private float speed;
    private float maxSpeed;

    private int gunIndex = 0;

    private bool inWater;
    private bool jump;
    private bool crouch;

    //private Vector3 r = Vector3.zero;
    //private Vector3 f = Vector3.zero;
    //private Vector3 u = Vector3.zero;

    private RaycastHit ray;

    private PlayerCam cam;

    private Rigidbody rb;
    public Collider walkSound;
    private Gun currentGun;
    private StatsSystem stats;
    private MainHub hub;
    void Start()
    {
        hub = FindObjectOfType<MainHub>();
        cam = FindObjectOfType<PlayerCam>();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<StatsSystem>();

        speed = walkForce;
        maxSpeed = speed * 1.5f;

        currentGun = guns[gunIndex];
        currentGun.gameObject.SetActive(true);
    }
    void Update()
    {
        if (!hub.DisableMouse)
        {
            // set maxspeed
            maxSpeed = speed * 1.5f;

            stealth = transform.localScale.y;

            // relative directions
            //r = Vector3.Project(rb.velocity, transform.right);
            //f = Vector3.Project(rb.velocity, transform.forward);
            //u = Vector3.Project(rb.velocity, transform.up);

            // enable/disable crosshair
            if (Input.GetKey(KeyCode.Mouse1)) HideObject(true, crossHair, 4f);
            else HideObject(false, crossHair, 4f);

            // weapon swap
            if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                currentGun.gameObject.SetActive(false);
                //currentGun.SwapWeapOff();
                if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) gunIndex++;
                if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) gunIndex--;
                if (gunIndex >= guns.Length) gunIndex = 0;
                if (gunIndex < 0) gunIndex = guns.Length - 1;
                currentGun = guns[gunIndex];
                currentGun.gameObject.SetActive(true);
                //currentGun.SwapWeapOn();
                //Debug.Log(gunIndex);
            }

            // set gravity and max speed based on inWater
            rb.useGravity = !inWater;
            if (rb.velocity.magnitude > (inWater ? maxSwimSpeed : maxSpeed)) rb.velocity = rb.velocity.normalized * (inWater ? maxSwimSpeed : maxSpeed);

            // set speed to walk, run or crouch
            if (Input.GetKey(KeyCode.LeftShift) && !crouch) speed = runForce;
            else if (crouch) speed = crouchForce;
            else speed = walkForce;

            // stamina
            stats.RemoveStamina(rb.velocity.magnitude > walkForce * 1.5f ? rb.velocity.magnitude * 1.25f : rb.velocity.magnitude * 0.25f);
            if (rb.velocity.magnitude <= 0.001f) stats.RecoverStamina();
            // set no stamina move speed
            if (stats.GetStamina() <= minStamina) speed = noStaminaForce;

            // change FOV based on move speed
            if (speed == runForce && (horzin != 0 || vertin != 0)) cam.ChangeFOV(runForce, sprintFOV);
            else cam.ChangeFOV(runForce, defaultFOV);

            // jump if not in water
            if (Input.GetKey(KeyCode.Space) && !inWater) jump = true;
            else jump = false;

            // crouch if not jumping
            if (Input.GetKey(KeyCode.LeftControl) && !jump)
            {
                Crouch(true);
                walkSound.enabled = false;
            }
            else
            {
                Crouch(false);
                walkSound.enabled = true;
            }

            // movement input
            horzin = Input.GetAxisRaw("Horizontal");
            vertin = Input.GetAxisRaw("Vertical");

            // UI
            if (HpBar != null) HpBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, stats.GetHp() - 96, 0);
            if (StaminaBar != null) StaminaBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(stats.GetStamina() - 96, 0, 0);
            //if (AmmoImage != null) AmmoImageHandler();
            //if (ReloadText != null && GetComponent<Attack>().GetIsReloading()) ReloadText.SetActive(true);
            //else if (ReloadText != null) ReloadText.SetActive(false);
        }
        else HideObject(true, crossHair, 4f);
    }
    private void FixedUpdate()
    {
        // if in water move the player in the direction of the camera using the swim force
        if (inWater) rb.AddForce((vertin * cam.transform.forward + horzin * cam.transform.right) * swimForce);
        // if not in the water and on the ground move the player 
        // speed is used for forwards movement
        // walk speed or crouch speed is used for side to side movement
        else if (CheckGround()) rb.AddForce(((Mathf.Abs(vertin) > 0.001f ? vertin : 0f) * 10f * speed * GetMoveDir() + (Mathf.Abs(horzin) > 0.001f ? horzin : 0f) * 10f * (speed != runForce ? speed : walkForce) * GetMoveDirRight()));
        else rb.AddForce(0.35f * (speed * vertin * cam.transform.forward + horzin * (speed != runForce ? speed : walkForce) * cam.transform.right));

        // jumping
        if (CheckGround() && !inWater && jump) Jump();

        // adds upward force if in water and not moving down
        //if (inWater && Vector3.Dot(Vector3.one, cam.transform.forward) < 3f) rb.AddForce(floatForce * transform.up); 

        // Slow movement in water
        //if (horzin == 0 && r.magnitude > 0.1f && inWater) rb.velocity += slowForce * Time.deltaTime * -r.normalized;
        //if (vertin == 0 && f.magnitude > 0.1f && inWater) rb.velocity += slowForce * Time.deltaTime * -f.normalized;
        //if (vertin == 0 && horzin == 0 && u.magnitude > 0.1f && inWater) rb.velocity += slowForce * Time.deltaTime * -u.normalized;

        // stop the player if moving slow enough
        if (rb.velocity.magnitude <= 0.1f) rb.velocity = Vector3.zero;
    }
    private void Jump()
    {
        // adds an upwards force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void Crouch(bool state)
    {
        Vector3 temp;
        crouch = state;
        if (crouch) temp = new Vector3(1f, CrouchScale, 1f);
        else temp = new Vector3(1f, NormalScale, 1f);
        transform.localScale = Vector3.Lerp(transform.localScale, temp, Time.deltaTime * crouchRate);
    }
    private void HideObject(bool state, GameObject obj, float rate)
    {
        Vector3 temp;
        if (state) temp = new Vector3(0f, 0f, 0f);
        else temp = new Vector3(1f, 1f, 1f);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, temp, Time.deltaTime * rate);
    }
    private Vector3 GetMoveDir()
    {
        // gets the move direction of the player based on the slope of the surface they are standing on
        if (CheckGround()) return Vector3.ProjectOnPlane(transform.forward, ray.normal);
        else return transform.forward;
    }
    private Vector3 GetMoveDirRight()
    {
        // gets the right direction vector based on the move direction
        return -Vector3.Cross(GetMoveDir(), ray.normal).normalized;
    }
    private bool CheckGround()
    {
         // checks the ground collision
        bool temp = false;
        for (int i=0;i<GChecks.Length;i++)
        {
            if (Physics.Raycast(GChecks[i].position, -transform.up, out ray, 1.25f, groundlayers))
            {
                if (ray.collider.isTrigger) temp = false;
                else if (rb.velocity.y <= 0f)
                {
                    temp = true;
                    break;
                }
            }
        }
        return temp;
    }
    public void Die()
    {
        // Kills player
        Time.timeScale = 0f;
        SceneManager.LoadScene(2);
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 4) inWater = true;
        else inWater = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4) inWater = false;
    }
    public float CurrentStealth
    {
        get { return stealth; }
        set { stealth = value; }
    }
}
