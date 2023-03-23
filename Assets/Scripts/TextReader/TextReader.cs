using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class TextReader : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Color colour;
    public GameObject textMeshContainer;
    public List<string> textToDIsplay;
    private int currentText = 0;
    public int displaySize;
    public int delay;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        textMesh.fontSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            closeText();
        }
    }

    public async void delayedText()
    {
        textMesh.text = textToDIsplay[currentText];
        for(int i = 0; i < displaySize; i++)
        {
            counter++;
            await Task.Delay((int)(delay * 60 * Time.deltaTime));
            textMesh.fontSize = counter;
        }
        currentText++;
    }

    public void displayText()
    {
        textMeshContainer.SetActive(true);
        delayedText();
    }

    public void closeText()
    {
        textMeshContainer.SetActive(false);
        textMesh.fontSize = 0;
        counter = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "TextChange")
        {
            Destroy(collision.gameObject);
            displayText();
        }
    }
}
