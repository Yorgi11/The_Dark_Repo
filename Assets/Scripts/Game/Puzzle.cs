using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] string PuzzleName;
    [SerializeField] Interactable[] puzzlePieces;
    [SerializeField] Interactable startingPiece;
    [SerializeField] string[] CorrectInputs;

    private bool activatedPieces = false;
    void Update()
    {
        if (startingPiece.GetPresses() > 0 && !activatedPieces)
        {
            if (puzzlePieces[0] != null)
            {
                for (int i = 0; i < puzzlePieces.Length; i++)
                {
                    puzzlePieces[i].SwapActiveFeature();
                }
            }
            startingPiece.SwapActiveFeature();
            activatedPieces = true;
        }
        //if (startingPiece.GetInputs() == CorrectInputs) 
    }
}
