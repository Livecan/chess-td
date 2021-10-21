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

    private Piece m_attackPiece;

    public UnityEvent OnFinishedMove { get; private set; } = new UnityEvent();

    private float m_movementSpeed = 5;

    private void Start()
    {
        m_currentPosition = startPosition;

        //float y = transform.position.y;
        // @todo: figure out how to do this - maybe spawn with the GameManager instead???
        //transform.position.Set(startPosition.ToVector3().x, transform.position.y, startPosition.ToVector3().z); // TODO: dont do double call to ToVector3
    }

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
                if (m_attackPiece)
                {
                    throw new System.NotImplementedException("Attack action not yet implemented.");
                    // Attack();
                }
                Debug.Log("Finished move.");
                OnFinishedMove.Invoke();
            }

        }
    }

    public void GoTo(Position position)
    {
        m_targetPosition = position;
        m_attackPiece = null;
    }

    public void GoTo(Piece opponent)
    {
        Debug.Log(m_currentPosition);
        Debug.Log(opponent.CurrentPosition);
        m_targetPosition = GetPositionBeforeAttack(m_currentPosition, opponent.CurrentPosition);
        m_attackPiece = opponent;
    }

    Position GetPositionBeforeAttack(Position initialPosition, Position targetPosition)
    {
        Position deltaPosition = targetPosition - initialPosition;

        Position positionBeforeAttack = initialPosition + deltaPosition.GetShorterPosition();

        return positionBeforeAttack;
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
