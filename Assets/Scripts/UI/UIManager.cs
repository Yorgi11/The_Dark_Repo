using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform HitParent;
    [SerializeField] private GameObject hitMarker;
    public void SpawnHitmarker()
    {
        GameObject marker = GameObject.Instantiate(hitMarker, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, HitParent);
        Destroy(marker, 0.125f);
    }
}
