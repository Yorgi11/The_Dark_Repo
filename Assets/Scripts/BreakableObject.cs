using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] GameObject SolidMesh;
    [SerializeField] Rigidbody[] bodys;
    [SerializeField] private int maxHits;
    [SerializeField] bool interactable;

    private int hits = 0;
    private void Update()
    {
        if (hits >= maxHits) Break();
    }
    private void Break()
    {
        SolidMesh.SetActive(false);
        for (int i=0;i< bodys.Length;i++)
        {
            bodys[i].isKinematic = false;
            bodys[i].gameObject.GetComponent<MeshRenderer>().enabled = true;
            if (!interactable) bodys[i].gameObject.layer = 3;
        }
    }
    public void AddHit()
    {
        hits++;
    }
}
