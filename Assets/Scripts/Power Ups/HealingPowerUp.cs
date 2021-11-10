using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPowerUp : PowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
#nullable enable
        Piece? otherPiece = other.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            otherPiece.HealthPoints = otherPiece.MaxHitPoints;
            Destroy(this.gameObject);
        }
#nullable restore
    }
}
