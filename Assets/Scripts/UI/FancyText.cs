using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FancyText : MonoBehaviour
{
    [SerializeField] Material mat;

    private float r=0f, g=0f, b = 0f, t = 0f;
    private float tempt = 1f;
    void Start()
    {
        
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.R))
            {
                r++;
            }
            else if (Input.GetKey(KeyCode.G))
            {
                g++;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                b++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.R))
            {
                r--;
            }
            else if (Input.GetKey(KeyCode.G))
            {
                g--;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                b--;
            }
        }*/
        
        t += tempt * Time.deltaTime;
        if (t > 1) tempt = -1;
        if (t < 0) tempt = 1;
        /*r = Mathf.PingPong(t*r, 10);
        g = Mathf.PingPong(t*g, 5);
        b = Mathf.PingPong(t*b, 7);*/
        Color c = new Color(r,g,b);
        c = Color.Lerp(Color.Lerp(Color.red, Color.blue, t), Color.Lerp(Color.green, Color.blue, t), t) + Color.Lerp(Color.Lerp(Color.red, Color.green, t), Color.Lerp(Color.green, Color.blue, t), t) + Color.Lerp(Color.Lerp(Color.red, Color.blue, t), Color.blue, t);
        SetTextColour(c);
    }

    public void SetTextColour(Color color)
    {
        mat.color = color;
    }
}
