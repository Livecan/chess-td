using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrengthBonus : Countdown
{
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

        m_piece.OnFinishedMove.AddListener(DoCountdown);

        this.OnZeroCountdown.AddListener(() =>
        {
            m_piece.OnFinishedMove.RemoveListener(DoCountdown);
            m_piece.RemoveStrengthBonus(this);
        });
    }
}
