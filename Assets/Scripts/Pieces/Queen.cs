using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    private int moveLength = 3;

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();

        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, 0), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, 1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(0, 1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, 1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, 0), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, -1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(0, -1), moveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, -1), moveLength));

        return availablePositions;
    }
}
