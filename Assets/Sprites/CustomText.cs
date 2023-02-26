using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomText : MonoBehaviour
{
    [SerializeField] private string t;
    [SerializeField] private GameObject[] c;
    [SerializeField] private string asciiText;
    [SerializeField] private float spacing = 24f;
    [SerializeField] private float scale = 1f;

    private string lastT = "";
    private float lastSpacing = 0f;
    private float lastScale = 0f;

    private float currentSpace = 0f;

    private char[] text_;
    private char[] ascii;
    private void Start()
    {
        ascii = asciiText.ToLower().ToCharArray();
        t = t.ToLower();
        text_ = t.ToCharArray();
    }
    private void Update()
    {
        ascii = asciiText.ToLower().ToCharArray();
        t = t.ToLower();
        text_ = t.ToCharArray();
        if (!t.Equals(lastT) || spacing != lastSpacing || scale != lastScale)
        {
            lastT = t;
            lastSpacing = spacing;
            lastScale = scale;
            for (int i=0;i< GetComponentsInChildren<Transform>().Length; i++)
            {
                GameObject.DestroyImmediate(GetComponentsInChildren<Transform>()[0].gameObject);
            }
            // loop through each desired character
            for (int i = 0; i < text_.Length; i++)
            {
                if (text_[i] != ' ')
                {
                    // loop through all the available ascii characters and find the coresponding indexes
                    for (int j = 0; j < ascii.Length; j++)
                    {
                        GameObject temp;
                        if (text_[i] == ascii[j])
                        {
                            temp = Instantiate(c[j], transform.position + new Vector3(currentSpace, 0f, 0f), Quaternion.identity, transform);
                            temp.transform.localScale = Vector3.one * scale;
                            currentSpace += spacing;
                        }
                    }
                }
                else currentSpace += spacing;
            }
        }
    }
}
