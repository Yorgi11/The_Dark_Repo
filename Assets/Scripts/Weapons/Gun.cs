using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private bool auto;
    [SerializeField] private bool usingAdvanceRecoilSystem;
    [SerializeField] private float Damage = 0f;
    [SerializeField] private float HeadShot = 0f;
    [SerializeField] private float FireRate = 0f;
    [SerializeField] private float muzzleVel = 0f;
    [SerializeField] private float impactForce = 0f;
    [SerializeField] private float reloadDelay = 0.5f;
    [Header("Recoil")]
    [SerializeField] private float Recoil = 0f;
    [SerializeField] private float camFactor = 0f;
    [SerializeField] private float recx = 0f;
    [SerializeField] private float recy = 0f;
    [SerializeField] private float recz = 0f;
    [SerializeField] private float recs = 0f;
    [Space]
    [Header("Advanced Recoil")]
    [SerializeField] private int[] shotsPerGroup;
    [SerializeField] private float[] recMultiPerGroup;
    [Space]
    [SerializeField] private float Mobility = 0f;
    [Header("Handling")]
    [SerializeField] private float Handling = 0f;
    [SerializeField] private float adsSpeed = 0f;
    [SerializeField] private float snappiness = 0f;
    [SerializeField] private float swapSpeed = 0f;
    [SerializeField] private float swapAngle = 55f;
    [Header("Animation Values")]
    [SerializeField] private float idlex = 0f;
    [SerializeField] private float idley = 0f;
    [SerializeField] private float idleSpeed = 0f;
    [SerializeField] private float concentartion = 0f;
    [SerializeField] private float swaySmooth = 0f;
    [SerializeField] private float swayMulti = 0f;
    [Header("Drag-Ins")]
    [SerializeField] private int maxAmmo = 1;
    [Header("Pos/Rot")]
    [SerializeField] private Vector3 aimPos;
    [SerializeField] private Vector3 aimRot;
    [SerializeField] private Vector3 hipPos;
    [SerializeField] private Vector3 hipRot;
    [Header("Drag-Ins")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject BarrelTip;
    [SerializeField] private GameObject particleSpawn;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shootSFX;
    [Header("UI")]
    [SerializeField] private GameObject AmmoField;
    [SerializeField] private GameObject bulletImage;
    [SerializeField] private Text AmmoText;

    private GameObject[] bulletImages;

    private string ammo = "";

    private int currentAmmo;
    private int shotGroupIndex = 0;

    private int currentShots = 0;

    private bool canShoot = true;
    private bool isReloading = false;

    private GunAnimation ani;
    private MuzzleGlow mg;

    // MainHub
    private MainHub hub;
    void Awake()
    {
        hub = FindObjectOfType<MainHub>();

        ani = GetComponent<GunAnimation>();
        mg = GetComponent<MuzzleGlow>();
        FireRate = 1 / FireRate;
        currentAmmo = maxAmmo;
        SetAmmo();
        bulletImages = new GameObject[maxAmmo];
    }
    void Update()
    {
        if (!hub.DisableMouse)
        {
            if (Input.GetKey(KeyCode.Mouse0) && canShoot && auto && currentAmmo > 0) Shoot();
            else if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot && !auto && currentAmmo > 0) Shoot();
            else if (!Input.GetKey(KeyCode.Mouse0))
            {
                currentShots = 0;
                mg.SetNumShots(currentShots);
            }
            ani.PointAtSway(snappiness);
            ani.IdleSway(idlex, idley, idleSpeed, concentartion);
            ani.LookSway(swaySmooth, swayMulti);
            ani.Recoil(0f, 0f, 0f, 0f, snappiness, camFactor);
            if (Input.GetKey(KeyCode.Mouse1)) ani.ADS(aimPos, aimRot, adsSpeed);
            else ani.ADS(hipPos, hipRot, adsSpeed);

            if (Input.GetKeyDown(KeyCode.R) && !isReloading) StartCoroutine(Reload());
            if (currentAmmo <= 0 && Input.GetKey(KeyCode.Mouse0) && !isReloading) StartCoroutine(Reload());

            if (AmmoText != null) AmmoText.text = ammo;
        }
    }

    private void Shoot()
    {
        Bullet b = Instantiate(bullet, cam.transform.position, cam.transform.localRotation).GetComponent<Bullet>();
        b.SetDamage(Damage);
        b.SetHeadShot(HeadShot);
        b.SetRange(muzzleVel);
        b.SetImpactForce(impactForce);
        //b.GetComponent<Rigidbody>().AddForce(cam.transform.forward * Range, ForceMode.Impulse);
        b.GetComponent<Rigidbody>().velocity = BarrelTip.transform.forward * muzzleVel;
        currentShots++;
        currentAmmo--;
        SetAmmo();
        mg.SetNumShots(currentShots);
        ParticleSystem p = Instantiate(particles.gameObject, particleSpawn.transform.position, Quaternion.LookRotation(particleSpawn.transform.forward)).GetComponent<ParticleSystem>();
        ParticleSystem p2 = Instantiate(smoke.gameObject, particleSpawn.transform.position, Quaternion.LookRotation(particleSpawn.transform.forward)).GetComponent<ParticleSystem>();
        source.PlayOneShot(shootSFX);
        p.Play();
        p2.Play();
        Destroy(p.gameObject, 0.06f);
        Destroy(p2.gameObject, 0.06f);

        // Advanced Recoil System
        if (auto && usingAdvanceRecoilSystem)
        {
            float x = recx;
            float y = recy;
            float z = recz;
            if (currentShots <= shotsPerGroup[shotGroupIndex])
            {
                x = recx * recMultiPerGroup[shotGroupIndex];
                y = recy * recMultiPerGroup[shotGroupIndex];
                z = recz * recMultiPerGroup[shotGroupIndex];
            }
            if (shotGroupIndex < shotsPerGroup.Length - 1)
            {
                shotGroupIndex++;
            }
            else
            {
                shotGroupIndex = shotsPerGroup.Length - 1;
            }
            ani.Recoil(x, y, z, recs, snappiness, camFactor);
        }
        else
        {
            ani.Recoil(recx, recy, recz, recs, snappiness, camFactor);
        }
        Destroy(b.gameObject, 5f);
        StartCoroutine(ResetShot());
    }

    public void SwapWeapOff()
    {
        ani.SwapWeap(swapSpeed, Vector3.zero, Vector3.right * swapAngle);
    }
    public void SwapWeapOn()
    {
        ani.SwapWeap(swapSpeed, Vector3.right * swapAngle, Vector3.zero);
    }

    private IEnumerator ResetShot()
    {
        canShoot = false;
        yield return new WaitForSeconds(FireRate);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        currentAmmo = maxAmmo;
        isReloading = false;
        SetAmmo();
    }

    private void SetAmmo()
    {
        ammo = "";
        for (int i = 0; i < currentAmmo; i++)
        {
            ammo += "|";
        }
    }
}
