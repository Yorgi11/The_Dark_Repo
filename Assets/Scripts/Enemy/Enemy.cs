using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float meleeDmg;
    [SerializeField] private float stage1Range;
    [SerializeField] private float stage2Range;
    [SerializeField] private float stage3Range;
    [SerializeField] private float stage4Range;

    [SerializeField] private int pointsPerDeath;

    [SerializeField] private int playerLayer;

    private float currentHp;
    private float range;

    private Vector3 dir;
    private Vector3 lastdir;
    private Vector3 movedir;

    private bool followPlayer;
    private bool appliedCrouch = false;
    private bool detectedPlayer = false;

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

        transform.forward = dir;

        move.SpeedLimit3D(speed, rb);
        //AI();
    }
    private void FixedUpdate()
    {
        // if in range follow the player
        if (followPlayer) move.Move3DVelocityDir(speed, rb, dir);
        else move.Stop3D(rb);
    }

    private void AI()
    {
        float h = player.GetHidden();
        if (player.GetCrouch() && !appliedCrouch)
        {
            h -= 0.5f;
            appliedCrouch = true;
        }
        else if (!player.GetCrouch()) appliedCrouch = false;
        if (h == 1) Wander();
        else if (h == 0.75f) Chase(1);
        else if (h == 0.5f) Chase(2);
        else if (h == 0.25f) Chase(3);
        else if (h == 0.0f) Chase(4);
    }

    private void Wander()
    {

    }

    private void Chase(int stage)
    {
        // goes towards a random position in the general area of the player
        if (stage == 1) dir = new Vector3(player.transform.position.x * Random.Range(0f, stage1Range),player.transform.position.y, player.transform.position.z * Random.Range(0f, stage1Range)) - transform.position;
        // goes towards the direction of the player
        if (stage == 2 && detectedPlayer)
        {
            dir = player.transform.position;
            detectedPlayer = false;
        }
        if (stage == 3) 
        if (stage == 4) 
        movedir = Vector3.Lerp(lastdir, dir, turnSpeed * Time.deltaTime);
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
