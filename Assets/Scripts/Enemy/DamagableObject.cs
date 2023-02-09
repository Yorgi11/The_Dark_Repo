using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObject : MonoBehaviour
{
    [SerializeField] private float bulletDamageMultiplier;
    [SerializeField] bool enemy;

    public void DoDamage(float damage)
    {
        if (enemy) GetComponentInParent<Enemy>().TakeDamage(damage * bulletDamageMultiplier);
        else GetComponent<StatsSystem>().TakeDamage(damage * bulletDamageMultiplier);
        Debug.Log(this.gameObject.name + " shot");
    }
}
