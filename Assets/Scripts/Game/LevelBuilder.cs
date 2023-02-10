using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour
{
    // | vertical wall
    // - horizontal wall
    [SerializeField] private float distBetweenWalls;
    private char[][] lastChararry;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (lastChararry != wallSpots)
        {
            for (int i=0;i<wallSpots.Length;i++)
            {
                for (int j = 0; j < wallSpots.Length; j++)
                {
                    //wallSpots[i][j];
                }
            }
        }*/
    }
}
