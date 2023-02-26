using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool canInteract = false;
    [SerializeField] private GameObject Prompt;
    void Update()
    {
        if (canInteract)
        {
            hideObject(false, Prompt, 4f);
            if (Input.GetKeyDown(KeyCode.E)) Debug.Log("Interacted");
        }
        else hideObject(true, Prompt, 4f);
    }
    private void hideObject(bool state, GameObject obj, float rate)
    {
        Vector3 temp;
        if (state) temp = new Vector3(0f, 0f, 0f);
        else temp = new Vector3(0.35f, 0.35f, 0.35f);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, temp, Time.deltaTime * rate);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7) canInteract = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7) canInteract = false;
    }
}
