using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float walkForce;
    [SerializeField] private float RunForce;
    [SerializeField] private float minRandMove;
    [SerializeField] private float maxRandMove;
    [SerializeField] private float obstDist;
    [SerializeField] private Vector3[] obstacleCheckDirs;
    [Header("Seek Level Ranges")]
    [SerializeField] private float S1Range;
    [SerializeField] private float S2Range;
    [SerializeField] private float S3Range;
    [SerializeField] private float S4Range;
    [SerializeField] private LayerMask DetectLayers;

    private float distToTarget;
    private float currentHp;
    private float speed;
    private float maxSpeed;

    private bool noPlayer = true;
    private bool targeted = false;

    private RaycastHit ray;
    private RaycastHit obstRay;

    private Rigidbody rb;

    private Vector3 Target;
    private Vector3 dir;
    private Vector3 avoidForce;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        speed = walkForce;
        maxSpeed = speed * 1.5f;
        if (RunForce < walkForce * 1.5f) RunForce = walkForce * 2f;
        currentHp = maxHp;
    }
    private void Update()
    {
        // sets the max speed based on the current allowed move speed
        maxSpeed = speed * 1.5f;
        // limmits the speed to be the max speed
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = 1.5f * speed * rb.velocity.normalized;

        // casts a ray from the enemy to the player
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out ray))
        {
            // if the ray hits the player set the target to the players position
            // if not set the target to a random position around the enemy's current position
            if (ray.collider.GetComponentInParent<Player>() != null)
            {
                if (player.CurrentStealth > 0.55f || player.GetComponent<Rigidbody>().velocity.magnitude > 2.5f)
                {
                    noPlayer = false;
                    targeted = false;
                    Target = player.transform.position;
                }
            }
            else noPlayer = true;
        }

        if (noPlayer && !targeted)
        {
            Target = new Vector3(transform.position.x + Random.Range(minRandMove, maxRandMove), transform.position.y, transform.position.z + Random.Range(minRandMove, maxRandMove));
            targeted = true;
        }

        dir = (Target - transform.position).normalized;
        distToTarget = Vector3.Distance(transform.position, Target);
        transform.forward = dir;

        if (distToTarget < 0.25f || rb.velocity.magnitude < 0.25f) targeted = false;

        SeekHandler();
    }
    private void FixedUpdate()
    {
        // move the enemy to the targets position
        avoidForce = Vector3.zero;
        ObstaclesCheck();
        rb.AddForce(7f * speed * avoidForce);
        if (distToTarget <= S1Range) rb.AddForce(10f * speed * dir);
    }
    private void SeekHandler()
    {
        // set the speed of the enemy based on how far they are from the player
        if (distToTarget <= S1Range)
        {
            speed = walkForce * player.CurrentStealth;
        }
        else if (distToTarget <= S2Range)
        {
            speed = walkForce * player.CurrentStealth;
        }
        else if (distToTarget <= S3Range)
        {
            speed = walkForce * 1.5f * player.CurrentStealth;
        }
        else if (distToTarget <= S4Range)
        {
            speed = RunForce;
        }
    }
    private void ObstaclesCheck()
    {
        for (int i=0; i<obstacleCheckDirs.Length;i++)
        {
            if (Physics.Raycast(transform.position, obstacleCheckDirs[i], out obstRay, obstDist))
            {
                if (obstRay.collider.GetComponent<Player>() == null) avoidForce += -obstacleCheckDirs[i] * (1/Vector3.Distance(transform.position, obstRay.collider.transform.position));
            }
        }
    }
    public void TakeDamage(float d)
    {
        if (currentHp - d <= 0)
        {
            currentHp = 0;
            Die();
        }
        else currentHp -= d;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<StatsSystem>() != null) collision.gameObject.GetComponent<StatsSystem>().TakeDamage(25f * Time.deltaTime);
    }
}
