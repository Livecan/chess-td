using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrengthBonus
{
    int m_countdown = 3;
    int m_bonus = 2;

    public int Bonus { get => m_bonus; }

    private Piece m_piece;

    public static StrengthBonus Create(Piece pieceToAssignTo)
    {
        return new StrengthBonus(pieceToAssignTo);
    }

    private StrengthBonus(Piece pieceToAssignTo)
    {
        m_piece = pieceToAssignTo;

        m_piece.AddStrengthBonus(this);

        m_piece.OnFinishedMove.AddListener(Countdown);
    }

    public void Countdown()
    {
        m_countdown--;
        if (m_countdown <= 0)
        {
            m_piece.OnFinishedMove.RemoveListener(Countdown);
            m_piece.RemoveStrengthBonus(this);
        }
    }
}
