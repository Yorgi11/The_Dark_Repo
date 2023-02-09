using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float bulletForce; // actual number / 43000 bc of bullet mass
    [SerializeField] private float shootDelay; // 1 / actual number / 60
    [SerializeField] private float reloadDelay;
    [SerializeField] private float snapiness;
    [SerializeField] private float adsSpeed;
    [SerializeField] private float idleSwaySpeed;
    [SerializeField] private float lookSwaySpeed;
    [SerializeField] private float idlex;
    [SerializeField] private float idley;

    [SerializeField] private float smooth;

    [SerializeField] private float concentraion;

    [SerializeField] private int maxAmmo;

    [SerializeField] private float recoilx;
    [SerializeField] private float recoily;
    [SerializeField] private float recoilz;
    [SerializeField] private float recoilSlide;
    [SerializeField] private float aimRecoilx;
    [SerializeField] private float aimRecoily;
    [SerializeField] private float aimRecoilz;
    [SerializeField] private float aimRecoilSlide;

    [SerializeField] private bool hasScope;
    [SerializeField] private float scopeFOV;

    [SerializeField] private Vector2 camRecoilMultiplier = new(1,1);

    [SerializeField] private Vector3 aimPos;
    [SerializeField] private Vector3 aimRot;
    [SerializeField] private Vector3 hipPos;
    [SerializeField] private Vector3 hipRot;

    [SerializeField] private GameObject bulletSpawn;

    [SerializeField] private Bullet bullet;

    [Header("1=Semi, 2=Auto, 3=Burst")]
    [SerializeField][Range(1, 3)] private int fireMode = 1;
    [SerializeField] private int burstNum = 0;

    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private ParticleSystem ShootParticles;

    private float defaultFOV;
    private float tempForce;
    private float tempDelay;
    private float currentCon;

    private int currentAmmo;
    private int currentShots;

    private bool isAiming;
    private bool shoot;
    private bool isShoot;
    private bool canReload = true;

    private AudioSource source;

    private Attack weapon;
    private WeaponAnimations ani;
    private PlayerCam cam;
    private Player player;
    private Rigidbody rb;
    void Start()
    {
        player = FindObjectOfType<Player>();
        source = GetComponentInParent<AudioSource>();
        float width = Screen.width;
        currentAmmo = maxAmmo;
        weapon = player.GetComponent<Attack>();
        rb = player.GetComponent<Rigidbody>();
        ani = GetComponent<WeaponAnimations>();
        cam = FindObjectOfType<PlayerCam>();
        tempForce = bulletForce;
        tempDelay = shootDelay;
        if (width != 1920f)
        {
            recoilx *= (1920 / width);
            recoily *= (1920 / width);
            recoilz *= (1920 / width);
            aimRecoilx *= (1920 / width);
            aimRecoily *= (1920 / width);
            aimRecoilz *= (1920 / width);
        }
    }

    private void Update()
    {
        cam.SetMultiplier(camRecoilMultiplier);
        bulletForce = System.MathF.Round(tempForce / 10750f, 2);
        shootDelay = System.MathF.Round(1 / (tempDelay / 60), 2);
        if (fireMode == 1 || fireMode == 3) if (Input.GetKeyDown(KeyCode.Mouse0)) shoot = true;
        if (fireMode == 2) if (Input.GetKey(KeyCode.Mouse0)) shoot = true;
        if (Input.GetKeyUp(KeyCode.Mouse0)) shoot = false;

        if (Input.GetKeyDown(KeyCode.Mouse1)) isAiming = true;
        if (Input.GetKeyUp(KeyCode.Mouse1)) isAiming = false;
        if (Input.GetKeyUp(KeyCode.Mouse1) && rb.velocity.magnitude < 0.01f) currentCon = concentraion;
        else currentCon = 1f;

        if (currentAmmo < maxAmmo && canReload && Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload(reloadDelay);
            canReload = false;
        }
        if (currentAmmo <= 0 && canReload)
        {
            weapon.Reload(reloadDelay);
            canReload = false;
        }
        if (!canReload && !weapon.GetIsReloading())
        {
            currentAmmo = maxAmmo;
            canReload = true;
        }

        // animate ads
        if (isAiming) ani.ADS(aimPos, aimRot);
        else ani.ADS(hipPos, hipRot);

        if (isAiming && hasScope) cam.ChangeFOV(adsSpeed, scopeFOV);
        else if (hasScope) cam.ChangeFOV(adsSpeed, defaultFOV);

        /*if (!shoot) */
        ani.IdleSway(idlex, idley, idleSwaySpeed);
        //else ani.IdleSway(0f, 0f, 0f);
        if (shoot) ani.LookSway(smooth*0.25f, lookSwaySpeed);
        else ani.LookSway(smooth, lookSwaySpeed);
    }

    private void FixedUpdate()
    {
        if (shoot && weapon.GetCanAttack() && currentAmmo > 0)
        {
            isShoot = true;
            GameObject temp = new GameObject();
            temp.transform.position = cam.transform.position;
            temp.transform.forward = bulletSpawn.transform.forward;
            if (fireMode == 3)
            {
                for (int i=0; i<burstNum; i++)
                {
                    weapon.Shoot3D(bulletForce, shootDelay, bullet, temp);

                    currentShots++;
                    currentAmmo--;
                    Recoil();
                }
            }else
            {
                weapon.Shoot3D(bulletForce, shootDelay, bullet, temp);
                currentShots++;
                currentAmmo--;
                Recoil();
            }
            float vol = currentShots;
            Destroy(temp);
            //AudioSource.PlayClipAtPoint(shootSFX, cam.transform.position, vol < 4f ? vol:4f);
            ParticleSystem p = Instantiate(ShootParticles.gameObject, bulletSpawn.transform.position, Quaternion.LookRotation(bulletSpawn.transform.forward)).GetComponent<ParticleSystem>();
            p.Play();
            Destroy(p, 0.06f);
            Destroy(p.gameObject, 0.06f);
            source.PlayOneShot(shootSFX);
            shoot = false;
        }else
        {
            isShoot = false;
            ani.Recoil(0, 0, 0, 0);
            currentShots = 0;
        }
    }
    private void Recoil()
    {
        // applies recoil to the weapon model
        if (isAiming)
        {
            ApplyRecoil(aimRecoilx, aimRecoily, aimRecoilz, aimRecoilSlide);
        }
        else
        {
            ApplyRecoil(recoilx, recoily, recoilz, recoilSlide);
        }
    }
    private void ApplyRecoil(float recx, float recy, float recz, float slide)
    {
        float x;
        float y;
        float z;

        if (currentShots <= 3)
        {
            x = Random.Range((recx * 0.1f) * 4.5f, (recx * 0.1f) * 5f);
            y = Random.Range((recy * 0.1f) * 4.5f, (recy * 0.1f) * 5f);
            z = Random.Range((recz * 0.1f) * 4.5f, (recz * 0.1f) * 5f);
        }
        else if (currentShots <= 6) 
        {
            x = Random.Range((recx * 0.1f) * 5.1f, (recx * 0.1f) * 6.5f);
            y = Random.Range((recy * 0.1f) * 5.1f, (recy * 0.1f) * 6.5f);
            z = Random.Range((recz * 0.1f) * 5.1f, (recz * 0.1f) * 6.5f);
        }
        else if (currentShots <= 12)
        {
            x = Random.Range((recx * 0.1f) * 6.6f, (recx * 0.1f) * 7.5f);
            y = Random.Range((recy * 0.1f) * 6.6f, (recy * 0.1f) * 7.5f);
            z = Random.Range((recz * 0.1f) * 6.6f, (recz * 0.1f) * 7.5f);
        }
        else
        {
            x = Random.Range((recx * 0.1f) * 7.6f, recx);
            y = Random.Range((recy * 0.1f) * 7.6f, recy);
            z = Random.Range((recz * 0.1f) * 7.6f, recz);
        }
        float temp = (int)Random.Range(-1, 1);
        float temp2 = (int)Random.Range(-1, 1);
        ani.Recoil(x, temp != 0 ? y * temp: y, temp != 0 ? z * temp2: z, slide);
    }
    public int GetAmmo()
    {
        return currentAmmo;
    }
    public float GetSnappiness()
    {
        return snapiness;
    }
    public float GetAdsSpeed()
    {
        return adsSpeed;
    }
    public float GetReloadDelay()
    {
        return reloadDelay;
    }

    public void SetDefaultFOV(float val)
    {
        defaultFOV = val;
    }

    public bool GetIsShooting()
    {
        return isShoot;
    }
    public bool GetIsAiming()
    {
        return isAiming;
    }

    public float GetConcentration()
    {
        return currentCon;
    }
}
