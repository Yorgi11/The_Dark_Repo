using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCamMove : MonoBehaviour
{
    private BasicPlayer player;
    void Start()
    {
        player = FindObjectOfType<BasicPlayer>();
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}
