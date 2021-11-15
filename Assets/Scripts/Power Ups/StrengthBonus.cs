using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrengthBonus : Countdown
{
    int m_bonus = 2;

    public int Bonus { get => m_bonus; }

    private Piece m_piece;

    private void DoConditionalCountdown(bool isPlayer)
    {
        if (isPlayer == m_piece.IsPlayer)
        {
            DoCountdown();
        }
    }

    public static StrengthBonus Create(Piece pieceToAssignTo)
    {
        return new StrengthBonus(pieceToAssignTo);
    }

    private StrengthBonus(Piece pieceToAssignTo)
    {
        m_piece = pieceToAssignTo;

        m_piece.AddStrengthBonus(this);

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

        this.OnZeroCountdown.AddListener(Destroy);
    }

    private void Destroy()
    {

        GameManager.Manager.OnFinishedTurn.RemoveListener(DoConditionalCountdown);
        m_piece.RemoveStrengthBonus(this);
    }
}
