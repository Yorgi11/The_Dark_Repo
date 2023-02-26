using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}
