using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : PowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
#nullable enable
        Piece? otherPiece = other.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            AssignPieceStrengthBonus(otherPiece);
            Destroy(this.gameObject);
        }
#nullable restore
    }

    private void AssignPieceStrengthBonus(Piece piece)
    {
        StrengthBonus.Create(piece);
    }
}
