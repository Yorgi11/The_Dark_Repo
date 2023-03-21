using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool opensMenu;
    [SerializeField] private bool isButton;
    [SerializeField] private bool doTrans;
    [SerializeField] private bool doActivate;
    [Header("Menu first == 0,   Button first == 1")]
    [SerializeField] private int menuVbutton = 0;
    [SerializeField] private int pressesToFlip = 1;
    [SerializeField] private GameObject Prompt;

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject objToTrans;

    [SerializeField] private Vector3 StartPos;
    [SerializeField] private Vector3 EndPos;
    [SerializeField] private float speed;

    [SerializeField] private GameObject[] Features;
    [SerializeField] private TMP_InputField[] inputFields;

    private AudioSource audio;

    private string[] inputs;

    private float t = 0f;

    private int presses = 0;

    private bool canInteract = false;
    private bool but = false;
    public bool audioPresent;

    // MainHub
    private MainHub hub;
    private void Start()
    {
        inputs = new string[inputFields.Length];
        hub = FindObjectOfType<MainHub>();
        if (audioPresent == true)
        {
            audio = GetComponent<AudioSource>();
        }
;
    }
    void Update()
    {
        if (canInteract)
        {
            HideObject(false, Prompt, 4f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (audioPresent == true)
                {
                    audio.Play();
                }

                if (opensMenu && isButton )
                {
                    if (menuVbutton == 0 && presses < pressesToFlip) OpenMenu();
                    else if (menuVbutton == 0 && presses >= pressesToFlip) but = true;
                    if (menuVbutton == 1 && presses < pressesToFlip) but = true;
                    else if (menuVbutton == 1 && presses >= pressesToFlip) OpenMenu();
                }
                else if (opensMenu) OpenMenu();
                else if (isButton) but = true;
                presses++;
            }
        }
        else HideObject(true, Prompt, 4f);

        if (but && objToTrans != null)
        {
            t += Time.deltaTime * speed;
            objToTrans.transform.position = Vector3.Lerp(StartPos, EndPos, t);
            if (t > 1f) t = 0f;
            if (objToTrans.transform.position == EndPos)
            {
                but = false;
                Vector3 temp = StartPos;
                StartPos = EndPos;
                EndPos = temp;
            }
        }
    }
    public void HideObject(bool state, GameObject obj, float rate)
    {
        Vector3 temp;
        if (state) temp = new Vector3(0f, 0f, 0f);
        else temp = new Vector3(0.35f, 0.35f, 0.35f);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, temp, Time.deltaTime * rate);
    }
    private void OpenMenu()
    {
        hub.DisableMouse = true;
        if (Menu != null) Menu.SetActive(true);
    }

    public void SwapActiveFeature()
    {
        if (Features != null)
        {
            for (int i=0;i<Features.Length;i++)
            {
                Features[i].SetActive(!Features[i].activeInHierarchy);
            }
        }
    }
    public void SetInputs()
    {
        for (int i=0;i<inputFields.Length;i++)
        {
            inputs[i] = inputFields[i].text;
        }
    }
    public string[] GetInputs()
    {
        return inputs;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7) canInteract = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7) canInteract = false;
    }

    public int GetPresses()
    {
        return presses;
    }
}
