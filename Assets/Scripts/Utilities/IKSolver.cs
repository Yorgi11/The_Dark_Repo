using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSolver : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform[] between;
    [SerializeField] private Transform tip;

    [SerializeField] private Transform target;

    private float[] Lengths;

    private Transform[] bones;

    private float distRootTip;
    void Start()
    {
        bones = new Transform[between.Length + 2];
        for (int i=0; i<between.Length+2; i++)
        {
            if (i == 0) bones[i] = root;
            else if (i == between.Length + 1) bones[i] = tip;
            else bones[i] = between[i-1];
        }
        Lengths = new float[bones.Length-1];
        for (int i=0; i<bones.Length-1; i++)
        {
            Lengths[i] = (bones[i].position - bones[i + 1].position).magnitude;
        }
        for (int i=0;i<Lengths.Length;i++)
        {
            distRootTip += Lengths[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsReachable())
        {
            for (int i=bones.Length-1;i>=0;i--)
            {
                if (i == bones.Length-1) bones[i].position = target.position;
                else
                {
                    if (i + 1 < bones.Length - 1) bones[i].position = Vector3.down;// (bones[i + 1].position - bones[i - 1].position).normalized * Lengths[i-1];
                }
            }
        }
        else root.forward = (target.position - root.position).normalized;
    }

    private bool IsReachable()
    {
        if (distRootTip >= (root.position - target.position).magnitude) return true;
        else return false;
    }
}
