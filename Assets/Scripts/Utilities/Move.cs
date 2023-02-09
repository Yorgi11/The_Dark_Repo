using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //2D && TOP DOWN 3D

    // 2D movement without a ridgidbody
    public void Move2D(float horzIn, float vertIn, float speed, float factor)
    {

    }
    // 2D movement with force
    public void Move2DForce(float horzIn, float vertIn, float speed, Rigidbody2D rb)
    {
        rb.velocity += new Vector2(horzIn * speed, vertIn * speed) * 0.5f;
    }
    // 2D movement with velocity
    public void Move2DVelocity(float horzIn, float vertIn, float speed, Rigidbody2D rb)
    {
        rb.velocity = new Vector2(horzIn * speed, vertIn * speed);
    }
    // 2D friction simulaton
    public void Slow2D(Rigidbody2D rb, float slowRate)
    {
        if (rb.velocity.magnitude <= 0.1f) rb.velocity = Vector2.zero;
        else rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude - slowRate * Time.deltaTime);
    }
    // 2D movement along the XZ plane (3D world)
    public void Move3DXZ(float horzIn, float vertIn, float speed, Rigidbody rb)
    {
        rb.velocity += new Vector3(horzIn * speed * 0.25f, 0f, vertIn * speed * 0.25f);
    }
    // 2D movement along the XZ plane (3D world) (using a direction vector instead of horizontal and vertical inputs)
    public void Move3DXZ(Vector3 dir, float speed, Rigidbody rb)
    {
        rb.velocity += new Vector3(dir.x * speed * 0.25f, 0f, dir.z * speed * 0.25f);
    }
    // 2D movement speed limiter
    public void SpeedLimit2D(float speed, Rigidbody2D rb)
    {
        if (rb.velocity.x > speed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else if (rb.velocity.x < -speed)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        if (rb.velocity.y > speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
        else if (rb.velocity.y < -speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
        }
    }
    // 2D movement speed limiter (3D world)
    public void SpeedLimit3DXZ(float speed, Rigidbody rb)
    {
        if (rb.velocity.x > speed)
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, rb.velocity.z);
        }
        else if (rb.velocity.x < -speed)
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.z > speed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        }
        else if (rb.velocity.z < -speed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -speed);
        }
    }
    // 2D movement stops all velocity
    public void Stop2D(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
    }
    // 2D(3D world) or 3D movement stops all velocity

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 3D
    //
    public void Move3DForce(float speed, Rigidbody rb, Vector3 sidewaysDir, Vector3 forwardDir)
    {
        rb.velocity += speed * 0.5f * (sidewaysDir + forwardDir);
    }
    public void Move3DVelocity(float speed, Rigidbody rb, Vector3 sidewaysDir, Vector3 forwardDir)
    {
        rb.velocity = (sidewaysDir + forwardDir) * speed;
    }
    public void Move3DVelocityDir(float speed, Rigidbody rb, Vector3 forwardDir)
    {
        rb.velocity = forwardDir * speed;
    }
    public void SpeedLimit3D(float speed, Rigidbody rb)
    {
        Vector2 temp = new Vector2(rb.velocity.x, rb.velocity.z);
        if (temp.magnitude > speed) temp = temp.normalized * speed;
        rb.velocity = new Vector3(temp.x, rb.velocity.y, temp.y);
    }
    public void Jump3D(float jumpForce, Rigidbody rb, Vector3 upDir)
    {
        rb.AddForce(upDir * jumpForce, ForceMode.Impulse);
    }
    public void Stop3D(Rigidbody rb)
    {
        rb.velocity = Vector3.zero;
    }
}
