using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject point;
    private float damage;
    private float headshot;
    private float range;
    private float impactForce;

    private Vector3 lastpos;
    private Vector3 dir;

    private Transform HitParent;

    private RaycastHit ray;
    void Awake()
    {
        lastpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (damage != 0 && headshot != 0 && range != 0 && impactForce != 0)
        {
            dir = transform.position - lastpos;
            if (Physics.Raycast(lastpos, dir.normalized, out ray, dir.magnitude))
            {
                if (ray.collider.GetComponent<DamagableObject>() != null)
                {
                    ray.collider.GetComponent<DamagableObject>().DoDamage(damage, headshot);
                    if (ray.collider.GetComponentInParent<Enemy>() != null) SpawnHitmarker();
                }
                if (ray.collider.CompareTag("LevelPart")) SpawnBulletHole();
                Rigidbody rb = null;
                if (ray.collider.GetComponent<Rigidbody>() != null) rb = ray.collider.GetComponent<Rigidbody>();
                if (ray.collider.GetComponentInParent<Rigidbody>() != null) rb = ray.collider.GetComponentInParent<Rigidbody>();
                if (rb != null) rb.AddForceAtPosition(impactForce * dir.normalized, ray.point, ForceMode.Impulse);
                Destroy(gameObject);
            }
            lastpos = transform.position;
        }
    }
    private void SpawnHitmarker()
    {
        HitParent = GameObject.FindGameObjectWithTag("HitMarker").transform;
        GameObject marker = GameObject.Instantiate(hitMarker, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, HitParent);
        Destroy(marker, 0.125f);
    }

    private void SpawnBulletHole()
    {
        GameObject pos = Instantiate(point, ray.point + ray.normal * 0.025f, Quaternion.LookRotation(-ray.normal), ray.collider.gameObject.transform);
        GameObject hole = Instantiate(bulletHole, pos.transform.position, Quaternion.LookRotation(-ray.normal));
        hole.GetComponent<BulletHole>().SetPos(pos.transform);
        Destroy(hole, 5f);
        Destroy(pos, 5.1f);
    }

    public void SetDamage(float d)
    {
        damage = d;
    }
    public void SetHeadShot(float h)
    {
        headshot = h;
    }
    public void SetRange(float r)
    {
        range = r;
    }
    public void SetImpactForce(float f)
    {
        impactForce = f;
    }
}
