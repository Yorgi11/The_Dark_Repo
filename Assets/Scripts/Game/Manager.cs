using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private float scoreToSpawnNewEnemy;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Enemy enemy;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text numEnemiesText;

    private int currentPoints;

    private float currentNumEnemies = 0;
    private float maxNumEnemies = 4;

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentPoints;
        if (numEnemiesText != null) numEnemiesText.text = "Enemies: " + currentNumEnemies;
        if (currentNumEnemies < maxNumEnemies)
        {
            Instantiate(enemy, spawnPoints[(int)Random.Range(0, spawnPoints.Length - 1)].position, Quaternion.identity);
            currentNumEnemies++;
        }
        maxNumEnemies = (int)(4 + currentPoints / scoreToSpawnNewEnemy);
    }

    public void RemoveEnemy()
    {
        currentNumEnemies--;
    }
    public void SetPoints(int val)
    {
        currentPoints += val;
    }
}