using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected abstract void OnTriggerEnter(Collider other);
}
