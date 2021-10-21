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
    public Position Pos { get => m_targetPosition; }

    private Piece m_attackPiece;

    public UnityEvent OnFinishedMove { get; private set; } = new UnityEvent();

    private float m_movementSpeed = 1;

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

            Vector3 targetPosition = m_targetPosition.ToVector3();
            targetPosition.y = transform.position.y;

            // when the piece is closer to the target position than the last move distance, it's reached the position
            if ((targetPosition - transform.position).magnitude < moveDistance)
            {
                m_currentPosition = m_targetPosition;
                m_targetPosition = null;

                // if planned attack, do it before the end of the turn
                if (m_attackPiece)
                {
                    throw new System.NotImplementedException();
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
        m_targetPosition = GetPositionBeforeAttack(m_targetPosition, opponent.Pos);
        m_attackPiece = opponent;
    }

    Position GetPositionBeforeAttack(Position initialPosition, Position targetPosition)
    {
        Position positionBeforeAttack;// TODO: do the magic
        throw new System.NotImplementedException();
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
    }
}
