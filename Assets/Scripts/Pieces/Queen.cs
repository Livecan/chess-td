using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Queen : Piece
{
    private int moveLength = 3;

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();

        int actualMoveLength = moveLength + DistanceBonus.Sum(distanceBonus => distanceBonus.Bonus);

        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, 0), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, 1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(0, 1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, 1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, 0), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(-1, -1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(0, -1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, new Position(1, -1), actualMoveLength));

        return availablePositions;
    }
}
