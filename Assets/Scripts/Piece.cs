using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Piece : MonoBehaviour
{
    private Position startPosition;
    public Position StartPosition
    {
        set
        {
            if (startPosition == null)
            {
                startPosition = value;
                m_currentPosition = value;
                transform.position = value.ToVector3(transform.position.y);
            }
            else
            {
                throw new System.InvalidOperationException("Start position already assigned!");
            }
        }
    }

    private Position m_currentPosition;
    private Position m_targetPosition;
    public Position CurrentPosition { get => m_currentPosition; }

    private Piece m_attackedPiece;

    public UnityEvent OnFinishedMove { get; private set; } = new UnityEvent();

    [SerializeField] int m_healthPoints = 1;
    public int HealthPoints { get => m_healthPoints; protected set => m_healthPoints = value; }
    [SerializeField] int m_strength = 1;
    public int Strength { get => m_strength; }

    private float m_movementSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        // The piece moves as long as it has a target to move to
        if (m_targetPosition != null)
        {
            Vector3 direction = (m_targetPosition.ToVector3() - m_currentPosition.ToVector3()).normalized;
            float moveDistance = m_movementSpeed * Time.deltaTime;
            transform.position += new Vector3(direction.x, direction.y, direction.z) * moveDistance;

            Vector3 targetPosition = m_targetPosition.ToVector3(transform.position.y);

            // when the piece is closer to the target position than the last move distance, it's reached the position
            if ((targetPosition - transform.position).magnitude < moveDistance)
            {
                transform.position = targetPosition;

                m_currentPosition = m_targetPosition;
                m_targetPosition = null;

                // if planned attack, do it before the end of the turn
                if (m_attackedPiece)
                {
                    Attack();
                }
                else
                {
                    OnFinishedMove.Invoke();
                }
            }

        }
    }
    public void GoTo(Position position)
    {
        m_targetPosition = position;
        m_attackedPiece = null;
    }
    public void GoTo(Piece opponent)
    {
        m_targetPosition = GetPositionBeforeAttack(m_currentPosition, opponent.CurrentPosition);
        m_attackedPiece = opponent;
    }

    // when not managing to destroy a piece in the attack, knowing the last position before attack is necessary
    protected virtual Position GetPositionBeforeAttack(Position initialPosition, Position targetPosition)
    {
        Position deltaPosition = targetPosition - initialPosition;

        Position positionBeforeAttack = initialPosition + deltaPosition.GetShorterPosition();

        return positionBeforeAttack;
    }
    public abstract List<Position> GetAvailablePositions();

    void Attack()
    {
        Position targetPosition = m_attackedPiece.CurrentPosition;
        // if the opponent gets destroyed, move to his position, if not, finish turn here
        if (m_attackedPiece.TakeDamage(m_strength))
        {
            GoTo(targetPosition);
        }
        else
        {
            OnFinishedMove.Invoke();
        }
    }

    bool TakeDamage(int attack)
    {
        m_healthPoints -= attack;
        // if no HP left, destroy
        if (m_healthPoints <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }

    // A helper that return up to `amount` positions in the given `deltaPosition` direction or until a Position with an opponent (included)
    protected static List<Position> GetAvailablePositions(GameManager gameManager, Piece currentPiece, List<Piece> allPieces, Position deltaPosition, int amount)
    {
        List<Position> availablePositions = new List<Position>();

        Position candidatePosition = currentPiece.CurrentPosition;

        for (int i = 1; i <= amount; i++)
        {
            candidatePosition += deltaPosition;

            if (candidatePosition.Column < 0 || candidatePosition.Column >= gameManager.m_fieldColumns || candidatePosition.Row < 0 || candidatePosition.Row >= gameManager.m_fieldRows
                || allPieces.Exists(piece => piece.CurrentPosition.Equals(candidatePosition) && piece.tag == currentPiece.tag))
            {
                break;
            }

            availablePositions.Add(candidatePosition);

            if (allPieces.Find(piece => piece.CurrentPosition.Equals(candidatePosition)))
            {
                break;
            }
        }
        return availablePositions;
    }
}
