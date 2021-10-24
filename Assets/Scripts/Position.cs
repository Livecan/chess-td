using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : System.IEquatable<Position>
{
    public int Row { get; private set; }
    public int Column { get; private set; }

    private static float rowVectorAdjustment = 4;
    private static float columnVectorAdjustment = 2;

    public Vector3 ToVector3(float y = 0)
    {
        return new Vector3(Row - rowVectorAdjustment, y, Column - columnVectorAdjustment);
    }

    public Position(Vector3 planeCoordinates)
    {
        this.Row = Mathf.RoundToInt(planeCoordinates.x + rowVectorAdjustment);
        this.Column = Mathf.RoundToInt(planeCoordinates.z + columnVectorAdjustment);
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

    public bool Equals(Position other)
    {
        return this.Column == other.Column && this.Row == other.Row;
    }

    public static Position operator -(Position b, Position a) => new Position(b.Row - a.Row, b.Column - a.Column);
    public static Position operator +(Position a, Position delta) => new Position(a.Row + delta.Row, a.Column + delta.Column);
}
