using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private int horzin;
    private int vertin;

    private bool isGrounded;
    private bool jump;

    private Rigidbody rb;
    private Move move;
    void Start()
    {
        move = GetComponent<Move>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        move.SpeedLimit3D(speed * 1.5f, rb);

        horzin = (int)Input.GetAxisRaw("Horizontal");
        vertin = (int)Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space)) jump = true;
        else jump = false;
    }
    private void FixedUpdate()
    {
        move.Move3DForce(speed, rb, transform.right * horzin, transform.forward * vertin);
        if (jump && isGrounded)
        {
            isGrounded = false;
            move.Jump3D(jumpForce, rb, transform.up);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6) isGrounded = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6) isGrounded = true;
    }
}
