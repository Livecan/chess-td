using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extend this class when creating a custom PowerUp script - contains logic for positioning the 3D object
public abstract class PowerUp : MonoBehaviour
{
    private Position m_position;
    public virtual Position Position
    {
        get => m_position;
        set
        {
            m_position = value;
            transform.position = m_position.ToVector3(transform.position.y);
        }
    }

    // Custom functionality for then the PowerUp collides with another object - supposedly a Piece
    protected abstract void OnTriggerEnter(Collider other);
}
