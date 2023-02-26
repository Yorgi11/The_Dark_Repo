using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    private Transform pos = null;
    void Update()
    {
        if (pos != null)
        {
            transform.position = pos.position;
            transform.rotation = Quaternion.LookRotation(pos.transform.forward);
        }
    }
    public void SetPos(Transform p)
    {
        pos = p;
    }
}
