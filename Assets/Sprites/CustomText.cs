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

    private float currentSpace = 0f;

    private GameObject currentC;

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
        if (!t.Equals(lastT))
        {
            lastT = t;
            currentSpace = 0f;
            ascii = asciiText.ToLower().ToCharArray();
            t = t.ToLower();
            text_ = t.ToCharArray();
            for (int i = this.transform.childCount; i > 0; --i) DestroyImmediate(this.transform.GetChild(0).gameObject);
            for (int i = 0; i < text_.Length; i++)
            {
                if (text_[i] != ' ')
                {
                    for (int j = 0; j < ascii.Length; j++)
                    {
                        if (ascii[j] == text_[i])
                        {
                            currentC = c[j];
                            break;
                        }
                    }
                    GameObject temp = Instantiate(currentC, transform.position + new Vector3(currentSpace, 0f, 0f), Quaternion.identity, this.gameObject.transform);
                    temp.transform.localScale = Vector3.one * scale;
                    currentSpace += spacing + (scale * 0.1f * spacing);
                }else currentSpace += spacing + (scale * 0.1f * spacing);
            }
        }
    }
}
