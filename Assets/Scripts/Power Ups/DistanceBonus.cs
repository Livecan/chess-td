using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DistanceBonus : Countdown
{
    int m_bonus;

    public int Bonus { get => m_bonus; }

    Piece m_piece;

    private void DoConditionalCountdown(bool isPlayer)
    {
        if (isPlayer == m_piece.IsPlayer)
        {
            DoCountdown();
        }
    }

    public static DistanceBonus Create(Piece pieceToAssignTo)
    {
        return new DistanceBonus(pieceToAssignTo);
    }

    private DistanceBonus(Piece pieceToAssignTo, int bonus = 2)
    {
        m_bonus = bonus;

        m_piece = pieceToAssignTo;

        m_piece.DistanceBonus = this;

        GameManager.Manager.OnFinishedTurn.AddListener(DoConditionalCountdown);

        m_piece.OnAttacked.AddListener(
            (isDestroyed) =>
            {
                if (isDestroyed)
                {
                    Destroy();
                }
            }
        );

        this.OnZeroCountdown.AddListener(this.Destroy);
    }

    public void Destroy()
    {
        GameManager.Manager.OnFinishedTurn.RemoveListener(DoConditionalCountdown);
        m_piece.DistanceBonus = null;
    }
}
