using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] string PuzzleName;
    [SerializeField] GameObject[] PuzPi;
    [SerializeField] Interactable[] puzzlePieces;
    [SerializeField] Interactable startingPiece;
    [SerializeField] GameObject[] objForRewards;
    [SerializeField] string[] CorrectInputs;
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector3 EndPos;

    private float t = 0f;

    private bool activatedPieces = false;
    private MainHub hub;
    private void Start()
    {
        hub = FindObjectOfType<MainHub>();
    }
    void Update()
    {
        if (startingPiece.GetPresses() > 0 && !activatedPieces)
        {
            if (puzzlePieces.Length > 0 && puzzlePieces[0] != null)
            {
                for (int i = 0; i < puzzlePieces.Length; i++)
                {
                    puzzlePieces[i].SwapActiveFeature();
                }
            }
            if (PuzPi.Length > 0 && PuzPi[0] != null)
            {
                for (int i = 0; i < PuzPi.Length; i++)
                {
                    if (PuzPi[i] != null) PuzPi[i].SetActive(!PuzPi[i].activeInHierarchy);
                }
            }
            startingPiece.SwapActiveFeature();
            activatedPieces = true;
        }
        if (t <= 1f && hub.EnemiesKilled >= 4 && objForRewards.Length > 0)
        {
            t += Time.deltaTime * 6f;
            objForRewards[0].transform.position = Vector3.Lerp(StartPos, EndPos, t);
            objForRewards[1].transform.position = Vector3.Lerp(StartPos, EndPos, t);
            objForRewards[2].transform.position = Vector3.Lerp(StartPos, EndPos, t);
            //objForRewards[3].transform.position = Vector3.Lerp(StartPos, EndPos, t);
        }
        //if (startingPiece.GetInputs() == CorrectInputs) 
    }
}
