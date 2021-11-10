using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBonus : Countdown
{
    int m_bonus;

    public int Bonus { get => m_bonus; }

    Piece m_piece;

    public static DistanceBonus Create(Piece pieceToAssignTo)
    {
        return new DistanceBonus(pieceToAssignTo);
    }

    private DistanceBonus(Piece pieceToAssignTo, int bonus = 2)
    {
        m_bonus = bonus;

        m_piece = pieceToAssignTo;

        m_piece.DistanceBonus = this;

        m_piece.OnFinishedMove.AddListener(DoCountdown);

        this.OnZeroCountdown.AddListener(this.Destroy);
    }

    public void Destroy()
    {
        m_piece.OnFinishedMove.RemoveListener(DoCountdown);
        m_piece.DistanceBonus = null;
    }
}
