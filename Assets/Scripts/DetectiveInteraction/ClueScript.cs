using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    [SerializeField] private GameObject journalScreen;
    public List<GameObject> clueSource;
    public  List<GameObject> puzzleSource;
    private List<bool> activeSourcePuzzle;
    private List<bool> activeSource;
    private string[] names;
    private string[] pnames;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < clueSource.Count; i++)
        {
            names[i] = clueSource[i].transform.name;
            activeSource[i] = false;
        }
        for (int i = 0; i < puzzleSource.Count; i++)
        {
            pnames[i] = puzzleSource[i].transform.name;
            activeSourcePuzzle[i] = false;
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
