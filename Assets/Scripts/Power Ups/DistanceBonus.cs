using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBonus : Countdown
{
    int m_bonus = 2;

    public int Bonus { get => m_bonus; }

    Piece m_piece;

    public static DistanceBonus Create(Piece pieceToAssignTo)
    {
        return new DistanceBonus(pieceToAssignTo);
    }

    private DistanceBonus(Piece pieceToAssignTo)
    {
        m_piece = pieceToAssignTo;

        m_piece.DistanceBonus.Add(this);

        m_piece.OnFinishedMove.AddListener(DoCountdown);

        this.OnZeroCountdown.AddListener(() =>
        {
            m_piece.OnFinishedMove.RemoveListener(DoCountdown);
            m_piece.DistanceBonus.Remove(this);
        });
    }
}
