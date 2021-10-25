using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Piece
{
    Position deltaForwardPosition;
    public Position DeltaForwardPosition
    {
        set
        {
            if (deltaForwardPosition != null)
            {
                throw new System.InvalidOperationException("Delta forward position already initialized");
            }
            deltaForwardPosition = value;
        }
        get
        {
            return deltaForwardPosition;
        }
    }

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();
        Position forwardPosition = CurrentPosition + DeltaForwardPosition;
        if (!allPieces.Any(piece => piece.CurrentPosition == forwardPosition))
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
