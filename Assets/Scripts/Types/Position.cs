using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : System.IEquatable<Position>
{
    public int Column { get; private set; }
    public int Row { get; private set; }

    private static float columnVectorAdjustment = 4;
    private static float rowVectorAdjustment = 2;

    public Vector3 ToVector3(float y = 0)
    {
        return new Vector3(Column - columnVectorAdjustment, y, Row - rowVectorAdjustment);
    }

    public Position(Vector3 planeCoordinates)
    {
        this.Column = Mathf.RoundToInt(planeCoordinates.x + columnVectorAdjustment);
        this.Row = Mathf.RoundToInt(planeCoordinates.z + rowVectorAdjustment);
    }

    public Position(int column, int row)
    {
        this.Column = column;
        this.Row = row;
    }

    public bool IsInArea(int left, int top, int right, int bottom)
    {
        return Column >= left && Column <= right && Row >= bottom && Row <= top;
    }

    public Position GetShorterPosition(int amount = 1)
    {
        int row = Column;
        if (row > 0)
        {
            row -= amount;
        }
        else if (row < 0)
        {
            row += amount;
        }
        int column = Row;
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
        return this.Row == other.Row && this.Column == other.Column;
    }

    public static Position operator -(Position b, Position a) => new Position(b.Column - a.Column, b.Row - a.Row);
    public static Position operator +(Position a, Position delta) => new Position(a.Column + delta.Column, a.Row + delta.Row);
}
