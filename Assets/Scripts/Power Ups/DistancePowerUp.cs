using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePowerUp : PowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
#nullable enable
        Piece? otherPiece = other.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            DistanceBonus.Create(otherPiece);
            Destroy(this.gameObject);
        }
#nullable restore
    }
}
