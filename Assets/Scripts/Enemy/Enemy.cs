using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float speed;
    [SerializeField] private float meleeDmg;
    [SerializeField] private float targetRange;

    [SerializeField] private int pointsPerDeath;

    [SerializeField] private int playerLayer;

    private float currentHp;
    private float range;

    private Vector3 dir;

    private bool followPlayer;

    private Move move;
    private Player player;
    private Manager manager;

    private Rigidbody rb;
    void Start()
    {
        player = FindObjectOfType<Player>();
        manager = FindObjectOfType<Manager>();
        move = GetComponent<Move>();
        rb = GetComponent<Rigidbody>();
        currentHp = maxHp;
    }
    void Update()
    {
        // direction from the enemy to the player
        dir = (player.transform.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0f, dir.z);
        // distance between the enemy and the player
        range = Vector2.Distance(player.transform.position, transform.position);
        // if in target range follow the player
        if (range <= targetRange) followPlayer = true;
        else followPlayer = false;

        transform.forward = dir;

        move.SpeedLimit3D(speed, rb);
    }
    private void FixedUpdate()
    {
        // if in range follow the player
        if (followPlayer) move.Move3DVelocityDir(speed, rb, dir);
        else move.Stop3D(rb);
    }
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
        manager.SetPoints((int)dmg);
    }

    private void Die()
    {
        manager.RemoveEnemy();
        manager.SetPoints(pointsPerDeath);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == playerLayer) player.GetComponent<StatsSystem>().TakeDamage(meleeDmg);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == playerLayer) player.GetComponent<StatsSystem>().TakeDamage(meleeDmg * Time.deltaTime);
    }
}
