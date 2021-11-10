using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraTurnPowerUp : PowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
#nullable enable
        Piece? otherPiece = other.gameObject.GetComponent<Piece>();
        if (otherPiece != null)
        {
            GameManager.Manager.HasExtraTurn = true;
            Destroy(this.gameObject);
        }
#nullable restore
    }
}
