using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rook : Piece
{
    private int moveLength = 3;

    public override PieceType Type => PieceType.Rook;

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();

        int actualMoveLength = moveLength + GetDistanceBonus();

        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, Position.getPosition(1, 0), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, Position.getPosition(0, 1), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, Position.getPosition(-1, 0), actualMoveLength));
        availablePositions.AddRange(GetAvailablePositions(GameManager.Manager, this, allPieces, Position.getPosition(0, -1), actualMoveLength));

        return availablePositions;
    }
}
