using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerUp : PowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
#nullable enable
        Piece? otherPiece = other.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            StrengthBonus.Create(otherPiece);
            Destroy(this.gameObject);
        }
#nullable restore
    }
}
