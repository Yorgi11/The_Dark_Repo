using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool opensMenu;
    [SerializeField] private bool isButton;
    [Header("Menu first == 0,   Button first == 1")]
    [SerializeField] private int menuVbutton = 0;
    [SerializeField] private int pressesToFlip = 1;
    [SerializeField] private GameObject Prompt;

    [SerializeField] private GameObject Menu;
    [SerializeField] private ButtonActivate objToActivate;

    [SerializeField] private GameObject[] Features;
    [SerializeField] private TMP_InputField[] inputFields;

    private string[] inputs;

    private int presses = 0;

    private bool canInteract = false;

    // MainHub
    private MainHub hub;
    private void Start()
    {
        inputs = new string[inputFields.Length];
        hub = FindObjectOfType<MainHub>();
    }
    void Update()
    {
        if (canInteract)
        {
            HideObject(false, Prompt, 4f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (opensMenu && isButton)
                {
                    if (menuVbutton == 0 && presses < pressesToFlip) OpenMenu();
                    else if (menuVbutton == 0 && presses >= pressesToFlip) ButtonPressed();
                    if (menuVbutton == 1 && presses < pressesToFlip) ButtonPressed();
                    else if (menuVbutton == 1 && presses >= pressesToFlip) OpenMenu();
                }
                else if (opensMenu) OpenMenu();
                else if (isButton) ButtonPressed();
                presses++;
            }
        }
        else HideObject(true, Prompt, 4f);
    }
    private void HideObject(bool state, GameObject obj, float rate)
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
    private void ButtonPressed()
    {
        if (objToActivate != null) objToActivate.Activate();
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
