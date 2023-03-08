using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObject : MonoBehaviour
{
    [SerializeField] private float bulletDamageMultiplier = 1f;
    [SerializeField] bool enemy;
    [SerializeField] bool head;

    public void DoDamage(float damage, float h)
    {
        if (enemy) GetComponentInParent<Enemy>().TakeDamage((head ? h : damage * bulletDamageMultiplier));
        else GetComponent<StatsSystem>().TakeDamage(damage * bulletDamageMultiplier);
        Debug.Log(this.gameObject.name + " shot");
    }
}
