using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Piece
{
    // Simplifies initializing of deltaForwardPosition, to use in Unity editor, needs to be compatible type - bool, not Position;
    // DeltaForwardPosition is set accordingly in Start().
    public bool isForwardLeft;

    Position deltaForwardPosition;
    public Position DeltaForwardPosition
    {
        get
        {
            return deltaForwardPosition;
        }
    }

    void Start()
    {
        deltaForwardPosition = isForwardLeft ? new Position(-1, 0) : new Position(1, 0);
    }

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();
        Position forwardPosition = CurrentPosition + DeltaForwardPosition;
        if (!forwardPosition.IsInArea(0, GameManager.Manager.FieldRows - 1, GameManager.Manager.FieldColumns - 1, 0))
        {
            return availablePositions;
        }

        if (!allPieces.Any(piece => piece.CurrentPosition.Equals(forwardPosition)))
        {
            availablePositions.Add(forwardPosition);
        }

        List<Position> attackPositions = new Position[] { forwardPosition + new Position(0, 1), forwardPosition + new Position(0, -1) }.ToList();

        List<Piece> opponentPieces = allPieces.Where(opponentPiece => !opponentPiece.gameObject.CompareTag(this.gameObject.tag)).ToList();

        availablePositions.AddRange(
            opponentPieces.Where(piece => attackPositions.Contains(piece.CurrentPosition)).Select(piece => piece.CurrentPosition)
        );

        return availablePositions;
    }
}
