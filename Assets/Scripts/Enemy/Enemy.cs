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

    private RaycastHit ray;

    private Rigidbody rb;

    private Vector3 Target;
    private Vector3 dir;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        speed = walkForce;
        maxSpeed = speed * 1.5f;
        if (RunForce < walkForce * 1.5f) RunForce = walkForce * 2f;
    }
    private void Update()
    {
        // sets the max speed based on the current allowed move speed
        maxSpeed = speed * 1.5f;
        // limmits the speed to be the max speed
        if (rb.velocity.magnitude > maxSpeed) rb.velocity = 1.5f * speed * rb.velocity.normalized;

        // casts a ray from the enemy to the player
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out ray, DetectLayers))
        {
            Debug.Log("Sees: " + ray.collider.name);
            // if the ray hits the player set the target to the players position
            // if not set the target to a random position around the enemy's current position
            if (ray.collider.GetComponentInParent<Player>() != null) Target = player.transform.position;
            //else Target = new Vector3(transform.position.x + Random.Range(minRandMove, maxRandMove), transform.position.y, transform.position.z + Random.Range(minRandMove, maxRandMove));
        }
        dir = (Target - transform.position).normalized;
        distToTarget = Vector3.Distance(transform.position, Target);
        transform.forward = dir;
        SeekHandler();
    }
    private void FixedUpdate()
    {
        // move the enemy to the targets position
        rb.AddForce(10f * speed * dir);
    }
    private void SeekHandler()
    {
        // set the speed of the enemy based on how far they are from the player
        if (distToTarget <= S1Range)
        {
            speed = walkForce;
        }
        else if (distToTarget <= S2Range)
        {
            speed = walkForce;
        }
        else if (distToTarget <= S3Range)
        {
            speed = walkForce * 1.5f;
        }
        else if (distToTarget <= S4Range)
        {
            speed = RunForce;
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
        Debug.Log("died");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<StatsSystem>() != null) collision.gameObject.GetComponent<StatsSystem>().TakeDamage(25f * Time.deltaTime);
    }
}
