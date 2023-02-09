using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float maxHunger;
    [SerializeField] private float maxStamina;
    [SerializeField] private float StaminaAmount;
    [SerializeField] private float HungerAmount;
    [SerializeField] private float healAmount;
    [SerializeField] private float staminaLossAmount;

    [SerializeField] private float healDelay;
    [SerializeField] private float recoverDelay;

    private float currentHp;
    private float currentHunger;
    private float currentStamina;

    private bool canHeal = true;
    private bool canRecover = true;

    private Scenes scene;
    void Start()
    {
        scene = FindObjectOfType<Scenes>();
        currentHp = maxHp;
        currentHunger = maxHunger;
        currentStamina = maxStamina;
    }
    private void Update()
    {
        if (currentHp < maxHp && canHeal) Heal();
        //if (currentStamina < maxStamina && canRecover) RecoverStamina();
    }

    public void RemoveStamina(float speedFactor)
    {
        if (currentStamina - staminaLossAmount * speedFactor * Time.deltaTime * 0.1f < 0) currentStamina = 0;
        else currentStamina -= staminaLossAmount * speedFactor * Time.deltaTime * 0.1f;
    }
    public void RecoverStamina()
    {
        if (currentStamina + StaminaAmount * Time.deltaTime > maxStamina) currentStamina = maxStamina;
        else currentStamina += StaminaAmount * Time.deltaTime;
    }

    public void RemoveHunger(float amount)
    {
        if (currentHunger - amount < 0) currentHunger = 0;
        else currentHunger -= amount;
    }
    public void AddHunger()
    {
        if (currentHunger + HungerAmount > maxHunger) currentHunger = maxHunger;
        else currentHunger += HungerAmount;
    }

    public void TakeDamage(float dmg)
    {
        if (currentHp - dmg <= 0)
        {
            currentHp = 0;
            Die();
        }
        else currentHp -= dmg;
        canHeal = false;
        StopCoroutine(DelayHealing(healDelay));
        StartCoroutine(DelayHealing(healDelay));
    }
    public void Heal()
    {
        if (currentHp + healAmount > maxHp) currentHp = maxHp;
        else currentHp += healAmount * Time.deltaTime;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + "Died");
        //scene.LoadDeathScene();
        //Destroy(gameObject);
    }

    IEnumerator DelayHealing(float time)
    {
        yield return new WaitForSeconds(time);
        canHeal = true;
    }

    // Getters

    public float GetHp()
    {
        return currentHp;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }
    public float GetHunger()
    {
        return currentHunger;
    }
    public float GetStamina()
    {
        return currentStamina;
    }
}
