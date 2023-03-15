using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HealingItem : MonoBehaviour
{
    public int maxCigs;
    private int currentCigs;
    public float maxChange;
    public List<Animation> ani;
    private StatsSystem stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatsSystem>();
        currentCigs = maxCigs;
    }

    private void Update()
    {
        //If heal input taken
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentCigs != 0)
            {
                ani[currentCigs].Play();
                currentCigs--;
                stats.ChangeMaxHealth(maxChange);
            }

        }
    }
}
