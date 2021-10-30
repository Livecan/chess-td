using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public Piece Piece;
    public Position Position;
    public Piece AttackedPiece;

    public Turn(Piece piece, Position position)
    {
        this.Piece = piece;
        this.Position = position;
    }

    public Turn(Piece piece, Piece attackedPiece)
    {
        this.Piece = piece;
        this.AttackedPiece = attackedPiece;
    }
}