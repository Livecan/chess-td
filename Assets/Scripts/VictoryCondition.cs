using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCondition : MonoBehaviour
{
    public void CheckVictoryCondition(bool isPlayerTurn)
    {
        IEnumerable<Piece> opponentPieces = GameManager.Manager.OpponentPieces;

        IEnumerable<Piece> opponentPiecesInPlayerSpawn = opponentPieces.Where(piece => piece.Position.Column == 0);

        // the player loses if the opponent has any 3+ pieces on player's side
        if (opponentPiecesInPlayerSpawn.Count() > 2)
        {
            GameManager.Manager.LoseGame();
        }

        IEnumerable<Piece> playerPieces = GameManager.Manager.PlayerPieces;

        IEnumerable<Piece> playerPawnsInOpponentSpawn = playerPieces.Where(piece => piece.Position.Column == GameManager.Manager.FieldColumns - 1);

        // the player wins if he has pawn on the 5 furthest opponent tiles
        if (playerPawnsInOpponentSpawn.Count() == GameManager.Manager.FieldRows)
        {
            GameManager.Manager.WinGame();
        }

        if (isPlayerTurn)
        {
            // if the player can't move any piece, he lost the game
            if (playerPieces.Where(piece => piece.GetAvailablePositions().Count > 0).Count() == 0)
            {
                GameManager.Manager.LoseGame();
            }
        }
        else
        {
            // if the opponent can't move, the player loses as well, because the game can't continue and  the player can't fulfill the winning conditions
            if (opponentPieces.Where(piece => piece.GetAvailablePositions().Count > 0).Count() == 0)
            {
                GameManager.Manager.LoseGame();
            }
        }
    }
}
