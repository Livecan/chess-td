using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private System.Action<Turn> m_turnCallback;
    public System.Action<Turn> TurnCallback
    {
        set
        {
            if (m_turnCallback != null)
            {
                throw new System.InvalidOperationException("Turn callback already initialized");
            }
            m_turnCallback = value;
        }
    }

    private IController.Direction? m_attackDirection;
    public IController.Direction AttackDirection
    {
        set
        {
            if (m_attackDirection != null)
            {
                throw new System.InvalidOperationException("Attack direction already initialized");
            }
            m_attackDirection = value;
        }
        get
        {
            if (m_attackDirection == null)
            {
                throw new System.InvalidOperationException("Attack direction not yet initialized");
            }
            return (IController.Direction)m_attackDirection;
        }
    }

    public void GetTurn(List<Piece> myPieces, List<Piece> opponentPieces)
    {
        Debug.Log("Get AI Turn");
        StartCoroutine(ProcessNextTurn(myPieces, opponentPieces));
    }

    private IEnumerator ProcessNextTurn(List<Piece> myPieces, List<Piece> opponentPieces)
    {
        yield return null;

        Turn nextTurn = null;

        int maxScore = int.MinValue;
        foreach (Piece myPiece in myPieces)
        {
            foreach (Position availablePosition in myPiece.GetAvailablePositions())
            {
                int score = 0;
                score -= GetPieceScore(myPiece, AttackDirection);
                score += GetPieceScore(myPiece, AttackDirection, availablePosition);

                Piece attackedOpponent = opponentPieces.Find(piece => piece.CurrentPosition.Equals(availablePosition));
                
                if (attackedOpponent != null)
                {
                    score += GetPieceScore(attackedOpponent, m_attackDirection != IController.Direction.Left ? IController.Direction.Left : IController.Direction.Right);
                    score -= GetPieceScore(
                        attackedOpponent,
                        m_attackDirection != IController.Direction.Left ? IController.Direction.Left : IController.Direction.Right,
                        null,
                        attackedOpponent.HealthPoints - myPiece.Strength
                    );
                }
                Debug.Log("Score: " + availablePosition.Column + ", " + availablePosition.Row + ": " + score);
                if (score > maxScore || (score == maxScore && Random.Range(0, 1f) < 0.5f))
                {
                    maxScore = score;
                    if (attackedOpponent != null) {
                        nextTurn = new Turn(myPiece, attackedOpponent);
                    }
                    else
                    {
                        nextTurn = new Turn(myPiece, availablePosition);
                    }
                }
            }
        }

        m_turnCallback(nextTurn);
    }

    private int GetPieceScore(Piece piece, IController.Direction attackDirection, Position position = null, int health = int.MinValue)
    {
        if (health != int.MinValue && health <= 0)
        {
            return 0;
        }

        if (position == null)
        {
            position = piece.CurrentPosition;
        }
        if (health == int.MinValue)
        {
            health = piece.HealthPoints;
        }

        int score = 0;

        if (typeof(Queen).IsInstanceOfType(piece))
        {
            score += 5;
        }
        if (typeof(Rook).IsInstanceOfType(piece))
        {
            score += 3;
        }
        
        score += health;

        if (attackDirection == IController.Direction.Right)
        {
            score += position.Column;
        }
        else
        {
            score -= position.Column;
        }

        return score;
    }
}
