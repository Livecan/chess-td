using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rook : Piece
{
    private int moveLength = 3;
    private GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();

        availablePositions.AddRange(GetAvailablePositions(allPieces, new Position(1, 0), moveLength));
        availablePositions.AddRange(GetAvailablePositions(allPieces, new Position(0, 1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(allPieces, new Position(-1, 0), moveLength));
        availablePositions.AddRange(GetAvailablePositions(allPieces, new Position(0, -1), moveLength));

        return availablePositions;
    }

    private List<Position> GetAvailablePositions(List<Piece> allPieces, Position deltaPosition, int amount)
    {
        List<Position> availablePositions = new List<Position>();

        Position candidatePosition = CurrentPosition;

        for (int i = 1; i <= amount; i++)
        {
            candidatePosition += deltaPosition;

            if (candidatePosition.Row < 0 || candidatePosition.Row >= gameManager.fieldRows || candidatePosition.Column < 0 || candidatePosition.Column >= gameManager.fieldColumns)
            {
                break;
            }

            availablePositions.Add(candidatePosition);

            if (allPieces.Find(piece => piece.CurrentPosition.Row == candidatePosition.Row && piece.CurrentPosition.Column == candidatePosition.Column)) {
                break;
            }
        }
        return availablePositions;
    }
}
