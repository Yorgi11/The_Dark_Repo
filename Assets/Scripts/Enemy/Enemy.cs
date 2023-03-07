using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float speed;
    [SerializeField] private float maxrange;
    [SerializeField] private float mindist;

    private float disttotarg;

    private Vector3 targ;

    private float currentHp;

    private Rigidbody rb;

    private bool seenPlayer = false;

    private RaycastHit rayHit;
    private Vector3 ray;

    public float sightLine;

    public Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
        currentHp = maxHp;
        targ = new Vector3(Random.Range(0f, maxrange), 1f, Random.Range(0f, maxrange));
    }
    private void Update()
    {
        if (seenPlayer == true)
        {
            if (rb.velocity.magnitude > speed * 1.5f) rb.velocity = rb.velocity.normalized * speed * 1.5f;
            if (rb.velocity.magnitude <= 0.05f) targ = transform.position + new Vector3(Random.Range(0f, maxrange), 1f, Random.Range(0f, maxrange));
            disttotarg = Vector3.Distance(transform.position, targ);
            if (disttotarg <= mindist) targ = new Vector3(Random.Range(0f, maxrange), 1f, Random.Range(0f, maxrange));
        }
        else
        {

        }
    }
    private void FixedUpdate()
    {
        rb.AddForce((targ - transform.position).normalized * speed);

        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out rayHit, sightLine * 1.5f))
        {
            if (rayHit.collider.transform.tag == "Player")
            {
                Debug.Log("I See You");
                seenPlayer = true;

            }
            else if (rayHit.collider.transform.tag == "LevelPart")
            {
                Debug.Log("That's a wall");
            }
            else
            {
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
        Debug.Log("died");
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<StatsSystem>() != null) collision.gameObject.GetComponent<StatsSystem>().TakeDamage(25f * Time.deltaTime);
    }
}
