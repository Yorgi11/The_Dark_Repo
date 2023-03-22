using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HealingItem : MonoBehaviour
{
    //How many cigs per a pack
    public int cigsPerPack;
    //How many boxes you have
    private int cigBoxs;
    //What cig your on in the pack
    private int currentCigs;
    public float maxHealthChange;
    public List<Animation> ani;
    private StatsSystem stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatsSystem>();
    }

    private void Update()
    {
        //Uses a new pack
        if (currentCigs == 0 && cigBoxs != 0)
        {
            cigBoxs--;
            currentCigs = cigsPerPack;
        }
        //If heal input taken
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentCigs != 0)
            {
                //ani[currentCigs].Play();
                currentCigs--;
                stats.ChangeMaxHealth(maxHealthChange);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "HealthPickup")
        {
            Destroy(collision.gameObject);
            cigBoxs++;
        }
    }
}
