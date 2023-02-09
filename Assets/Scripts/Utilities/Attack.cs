using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private bool canAttack = true;
    private bool isReloading;

    //private Animations animations;
    public void ShootXY(float bulletForce, float shootDelay, Bullet bullet, GameObject bulletSpawn/*, Animations ani*/)
    {
        Bullet currentBullet = Instantiate(bullet, bulletSpawn.transform.position, transform.rotation);
        currentBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce * bulletSpawn.transform.up, ForceMode2D.Impulse);
        //animations = ani;
        StartCoroutine(DelayAttacking(shootDelay));
    }
    public void Shoot3D(float bulletForce, float shootDelay, Bullet bullet, GameObject bulletSpawn/*, Animations ani*/)
    {
        Bullet currentBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        currentBullet.GetComponent<Rigidbody>().AddForce(bulletForce * bulletSpawn.transform.forward, ForceMode.Impulse);
        //animations = ani;
        StartCoroutine(DelayAttacking(shootDelay));
    }
    public void Melee2D(GameObject attackPoint, float damage, float range, LayerMask enemyLayers, float attackDelay)
    {
        Debug.Log("Melee Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, range, enemyLayers);
        foreach (Collider2D enemy in hitEnemies) enemy.GetComponent<Enemy>().TakeDamage(damage);
        StartCoroutine(DelayAttacking(attackDelay));
    }
    public void AimAtMouse2D(Camera cam, GameObject WeaponPivot, GameObject mouseObject)
    {
        if (mouseObject != null) mouseObject.transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0f);
        Vector3 difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        WeaponPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z - 90f);
    }
    IEnumerator DelayAttacking(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    public void Reload(float reloadDelay)
    {
        isReloading = true;
        StartCoroutine(DelayReloading(reloadDelay));
    }
    IEnumerator DelayReloading(float time)
    {
        //animations.isShooting(true);
        yield return new WaitForSeconds(time);
        isReloading = false;
        //animations.isShooting(false);
    }

    public void SetCanAttack(bool state)
    {
        canAttack = state;
    }
    public bool GetCanAttack()
    {
        return canAttack;
    }
    public bool GetIsReloading()
    {
        return isReloading;
    }
}
