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

    public override PieceType Type => PieceType.Pawn;

    protected override void Start()
    {
        base.Start();
        deltaForwardPosition = isForwardLeft ? Position.getPosition(-1, 0) : Position.getPosition(1, 0);
    }

    public override List<Position> GetAvailablePositions()
    {
        List<Piece> allPieces = new List<Piece>(FindObjectsOfType<Piece>());
        List<Position> availablePositions = new List<Position>();
        Position forwardPosition = Position + DeltaForwardPosition;

        // Check that pawn's move would not be outside of the field
        if (!forwardPosition.IsInArea(0, GameManager.Manager.FieldRows - 1, GameManager.Manager.FieldColumns - 1, 0))
        {
            return availablePositions;
        }

        // If the pawn caught Distance Power Up, it can move forward more fields
        int actualMoveLength = 1 + GetDistanceBonus();
        availablePositions.AddRange(
            Piece.GetAvailablePositions(GameManager.Manager, this, allPieces, DeltaForwardPosition, actualMoveLength)
                // The pawn can't attack moving forward
                .Where(emptyPosition => !allPieces.Any(piece => piece.Position.Equals(emptyPosition)))
        );

        List<Position> attackPositions = new Position[] { forwardPosition + Position.getPosition(0, 1), forwardPosition + Position.getPosition(0, -1) }.ToList();

        List<Piece> opponentPieces = allPieces.Where(opponentPiece => !opponentPiece.gameObject.CompareTag(this.gameObject.tag)).ToList();

        // If there are any pieces in the pawn's attacking space, add these options in available positions
        availablePositions.AddRange(
            opponentPieces.Where(piece => attackPositions.Contains(piece.Position)).Select(piece => piece.Position)
        );

        return availablePositions;
    }
}
