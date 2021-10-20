using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Piece> playerPieces;
    [SerializeField] private List<Piece> opponentPieces;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            playerPieces[0].startPosition = new Piece.Position(0, i + 1);
            opponentPieces[0].startPosition = new Piece.Position(8, i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
