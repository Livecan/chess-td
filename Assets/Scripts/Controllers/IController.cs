using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public System.Action<Turn> TurnCallback { set; }

    public enum Direction { Left, Right };
    public Direction AttackDirection { set; }

    public void GetTurn(List<Piece> myPieces, List<Piece> opponentPieces);
}