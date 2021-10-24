using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Piece> playerPiecesPrefabs;
    [SerializeField] private List<Piece> opponentPiecesPrefabs;

    private List<Piece> m_playerActivePieces = new List<Piece>();
    private List<Piece> m_opponentActivePieces = new List<Piece>();

    private int turnIndex = 1;

    private IController playerController;
    private IController opponentController;

    public readonly int fieldRows = 9, fieldColumns = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(NextTurn));
        opponentPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(NextTurn));

        for (int i = 0; i < 3; i++)
        {
            Piece playerPiece = Instantiate(playerPiecesPrefabs[i]);
            playerPiece.StartPosition = new Position(0, i + 1);
            m_playerActivePieces.Add(playerPiece);
            m_playerActivePieces[i].OnFinishedMove.AddListener(NextTurn);

            Piece opponentPiece = Instantiate(opponentPiecesPrefabs[i]);
            opponentPiece.StartPosition = new Position(8, i + 1);
            m_opponentActivePieces.Add(opponentPiece);
            m_opponentActivePieces[i].OnFinishedMove.AddListener(NextTurn);
        }


        SetupController(playerController = FindObjectOfType<UserController>());
        opponentController = playerController;  // TODO: temporary until AI

        NextTurn();
    }

    private void SetupController(IController playerController)
    {
        playerController.TurnCallback = NextTurn;
        playerController.GetTurn(m_playerActivePieces, m_opponentActivePieces);
    }

    void NextTurn(Turn turn)
    {
        if (turn.AttackedPiece == null)
        {
            turn.Piece.GoTo(turn.Position);
        }
        else
        {
            turn.Piece.GoTo(turn.AttackedPiece);
        }

        m_playerActivePieces = FindObjectsOfType<Piece>().Where(piece => piece.gameObject.CompareTag("Player")).ToList();
        m_opponentActivePieces = FindObjectsOfType<Piece>().Where(piece => piece.gameObject.CompareTag("Opponent")).ToList();
    }

    void NextTurn()
    {
        turnIndex = (turnIndex + 1) % 2;

        Debug.Log(turnIndex);

        if (turnIndex == 0)
        {
            playerController.GetTurn(m_playerActivePieces.ToList(), m_opponentActivePieces.ToList());
        }
        else
        {
            opponentController.GetTurn(m_opponentActivePieces.ToList(), m_playerActivePieces.ToList());
        }

    }
}
