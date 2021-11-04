using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPowerUp : PowerUp
{
    private Position m_position;
    public override Position Position {
        get => m_position;
        set
        {
            m_position = value;
            transform.position = m_position.ToVector3(transform.position.y);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Healing trigger");
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
