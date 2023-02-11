using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    [SerializeField] private float impactForce;
    [SerializeField] private bool isPlayerBullet;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject BloodEffect;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private GameObject ParticleEffect;
    [SerializeField] private AudioClip hitSFX;

    private Vector3 dir;

    private Vector3 lastPos;

    private Rigidbody bulletRb;

    private RaycastHit ray;
    void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
        lastPos = transform.position;
    }
    void Update()
    {
        dir = transform.position - lastPos;
        if (Physics.Raycast(lastPos, dir.normalized, out ray, dir.magnitude))
        {
            /*if (ray.collider.gameObject.GetComponentInParent<Enemy>() != null && isPlayerBullet)
            {
                ray.collider.gameObject.GetComponentInParent<Enemy>().TakeDamage(bulletDamage);
                //Instantiate(BloodEffect, ray.point, Quaternion.LookRotation(ray.normal));
                SpawnHitmarker();
            }*/
            if (ray.collider.gameObject.GetComponent<DamagableObject>() != null && isPlayerBullet)
            {
                ray.collider.gameObject.GetComponent<DamagableObject>().DoDamage(bulletDamage);
                //Instantiate(BloodEffect, ray.point, Quaternion.LookRotation(ray.normal));
                //Instantiate(ParticleEffect, ray.point, Quaternion.LookRotation(ray.normal));
                SpawnHitmarker();
            }
            if (ray.collider.gameObject.GetComponentInParent<BreakableObject>() != null) ray.collider.gameObject.GetComponentInParent<BreakableObject>().AddHit();
            if (ray.collider.gameObject.GetComponentInParent<StatsSystem>() != null && !isPlayerBullet) ray.collider.gameObject.GetComponentInParent<StatsSystem>().TakeDamage(bulletDamage);
            if (ray.transform.CompareTag("LevelPart")) SpawnBulletHole();
            if (ray.collider.GetComponent<Rigidbody>() != null) ray.collider.GetComponent<Rigidbody>().AddForceAtPosition(dir.normalized * impactForce, ray.point);
            Destroy(gameObject);
        }
        lastPos = transform.position;
    }
    private void SpawnBulletHole()
    {
        GameObject hole = Instantiate(bulletHole, ray.point + ray.normal * 0.025f, Quaternion.LookRotation(ray.normal));
        //AudioSource.PlayClipAtPoint(hitSFX, transform.position);
        Destroy(hole, 5f);
    }
    private void SpawnHitmarker()
    {
        Transform temp = GameObject.FindGameObjectWithTag("HitMarker").transform;
        GameObject marker = GameObject.Instantiate(hitMarker, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, temp);
        Destroy(marker, 0.125f);
    }
}
