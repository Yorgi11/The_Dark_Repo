using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoading : MonoBehaviour
{
    [SerializeField] GameObject[] areas;

    private GameObject currentArea;
    void Start()
    {
        currentArea = areas[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentArea(GameObject n)
    {
        currentArea = n;
    }
}
