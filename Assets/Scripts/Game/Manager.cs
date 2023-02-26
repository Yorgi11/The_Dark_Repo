using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private float MaxEnemies;
    [SerializeField] private float scoreToSpawnNewEnemy;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject game;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text numEnemiesText;

    private int currentPoints;

    private int currentNumEnemies = 0;

    // Update is called once per frame
    void Update()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentPoints;
        if (numEnemiesText != null) numEnemiesText.text = "Enemies: " + currentNumEnemies;
        if (currentNumEnemies < MaxEnemies)
        {
            Instantiate(enemy, spawnPoints[currentNumEnemies].position, Quaternion.identity, game.transform);
            currentNumEnemies++;
        }
        //maxNumEnemies = (int)(4 + currentPoints / scoreToSpawnNewEnemy);
    }

    public void RemoveEnemy()
    {
        //currentNumEnemies--;
    }
    public void SetPoints(int val)
    {
        currentPoints += val;
    }
}