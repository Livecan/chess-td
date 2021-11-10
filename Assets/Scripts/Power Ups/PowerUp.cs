using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extend this class when creating a custom PowerUp script - contains logic for positioning the 3D object
public abstract class PowerUp : PositionedObject
{
    // Custom functionality for then the PowerUp collides with another object - supposedly a Piece
    protected virtual void OnTriggerEnter(Collider other) {
        other.gameObject.GetComponent<Piece>()?.OnPowerUpCollected?.Invoke();
    }
}
