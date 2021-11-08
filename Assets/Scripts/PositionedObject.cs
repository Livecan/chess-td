using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionedObject : MonoBehaviour
{
    protected Position m_position;
    public virtual Position Position
    {
        get => m_position;
        set
        {
            m_position = value;
            transform.position = m_position.ToVector3(transform.position.y);
        }
    }
}
