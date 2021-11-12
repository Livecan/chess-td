using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Extend this class when creating a custom PowerUp script - contains logic for positioning the 3D object
public abstract class PowerUp : PositionedObject
{
    public UnityEvent OnDestroyEvent { get; } = new UnityEvent();

    protected virtual void OnDestroy()
    {
        OnDestroyEvent.Invoke();
    }

    // Custom functionality for then the PowerUp collides with another object - supposedly a Piece
    protected virtual void OnTriggerEnter(Collider other) {
        other.gameObject.GetComponent<Piece>()?.OnPowerUpCollected?.Invoke();
    }
}
