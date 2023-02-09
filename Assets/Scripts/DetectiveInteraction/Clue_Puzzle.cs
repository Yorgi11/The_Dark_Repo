using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue_Puzzle : MonoBehaviour
{
    [SerializeField] private GameObject journalScreen;
    [SerializeField] private GameObject[] clueIcons;
    [SerializeField] private GameObject[] puzzleIcons;
    [SerializeField] private GameObject clueContainer;
    [SerializeField] private GameObject puzzleContainer;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) journalScreen.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab)) journalScreen.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Clue"))
        {
            GameObject.Instantiate(clueIcons[int.Parse(collision.gameObject.name[^1..])], clueContainer.transform.position, Quaternion.identity, clueContainer.transform);
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("Puzzle"))
        {
            GameObject.Instantiate(puzzleIcons[int.Parse(collision.gameObject.name[^1..])], puzzleContainer.transform.position, Quaternion.identity, puzzleContainer.transform);
            Destroy(collision.gameObject);
        }
    }
}
