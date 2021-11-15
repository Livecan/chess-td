using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// POLYMORPHISM
public interface IController
{
    public System.Action<Turn> TurnCallback { set; }

    public enum Direction { Left, Right };
    public Direction AttackDirection { set; }

    public void GetTurn(IEnumerable<Piece> myPieces, IEnumerable<Piece> opponentPieces);
}
