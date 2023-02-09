using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    private Player player;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        transform.forward = (transform.position - player.transform.position).normalized;
    }
}
