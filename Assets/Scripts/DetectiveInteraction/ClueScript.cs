using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    [SerializeField] private GameObject journalScreen;
    public List<GameObject> clueSource;
    public List<GameObject> puzzleSource;
    public List<bool> activeSourcePuzzle;
    public List<bool> activeSource;
    [SerializeField] private GameObject[] n;
    [SerializeField] private GameObject[] p;
    private string[] names;
    private string[] pnames;
    // Start is called before the first frame update
    void Start()
    {
        names = new string[n.Length];
        pnames = new string[n.Length];
        for (int i=0; i<n.Length; i++)
        {
            names[i] = n[i].name;
            pnames[i] = p[i].name;
        }
        for(int b = 0; activeSource.Count < b; b++)
        {
            activeSource[b] = false;
        }

        for (int b = 0; activeSourcePuzzle.Count < b; b++)
        {
            activeSourcePuzzle[b] = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            journalScreen.SetActive(true);
            for (int i = 0; clueSource.Count > i; i++)
            {
                clueSource[i].SetActive(activeSource[i]);
            }

            for (int i = 0; puzzleSource.Count > i; i++)
            {
                puzzleSource[i].SetActive(activeSourcePuzzle[i]);
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            journalScreen.SetActive(false);
        }
    }

    private void pageturn()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Clue")
        {
            for (int i = 0; names.Length > i; i++)
            {
                if(collision.transform.name == names[i])
                {
                    activeSource[i] = true;
                    Destroy(collision.gameObject);
                }
            }
        }
        if(collision.transform.tag == "Puzzle")
        {
            for (int i = 0; pnames.Length > i; i++)
            {
                if (collision.transform.name == pnames[i])
                {
                    activeSourcePuzzle[i] = true;
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
