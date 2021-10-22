using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Piece : MonoBehaviour
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
                Debug.Log("Finished move.");
                OnFinishedMove.Invoke();
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
        Debug.Log(m_currentPosition);
        Debug.Log(opponent.CurrentPosition);
        m_targetPosition = GetPositionBeforeAttack(m_currentPosition, opponent.CurrentPosition);
        m_attackedPiece = opponent;
    }
    Position GetPositionBeforeAttack(Position initialPosition, Position targetPosition)
    {
        Position deltaPosition = targetPosition - initialPosition;

        Position positionBeforeAttack = initialPosition + deltaPosition.GetShorterPosition();

        return positionBeforeAttack;
    }
    void Attack()
    {
        Position targetPosition = m_attackedPiece.CurrentPosition;
        // if the opponent gets destroyed, move to his position
        if (m_attackedPiece.TakeDamage(m_strength))
        {
            GoTo(targetPosition);
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

    public class Position
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        private float rowVectorAdjustment = -4;
        private float columnVectorAdjustment = -2;

        public Vector3 ToVector3(float y = 0)
        {
            return new Vector3(Row + rowVectorAdjustment, y, Column + columnVectorAdjustment);
        }

        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public Position GetShorterPosition(int amount = 1)
        {
            int row = Row;
            if (row > 0)
            {
                row -= amount;
            }
            else if (row < 0)
            {
                row += amount;
            }
            int column = Column;
            if (column > 0)
            {
                column -= amount;
            }
            else if (column < 0)
            {
                column += amount;
            }

            return new Position(row, column);
        }

        public static Position operator -(Position b, Position a) => new Position(b.Row - a.Row, b.Column - a.Column);
        public static Position operator +(Position a, Position delta) => new Position(a.Row + delta.Row, a.Column + delta.Column);
    }
}
