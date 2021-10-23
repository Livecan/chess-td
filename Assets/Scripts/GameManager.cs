using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Piece> playerPiecesPrefabs;
    [SerializeField] private List<Piece> opponentPiecesPrefabs;

    private List<Piece> m_playerActivePieces = new List<Piece>();
    private List<Piece> m_opponentActivePieces = new List<Piece>();

    private bool playerTurn = true;
    private List<Turn> turns = new List<Turn>();
    private int turnIndex = 0;

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

        turns.AddRange(new Turn[]
        {
            new Turn(m_playerActivePieces[0], new Position(1, 1)),
            new Turn(m_opponentActivePieces[1], new Position(5, 2)),
            new Turn(m_playerActivePieces[1], new Position(3, 2)),
            new Turn(m_opponentActivePieces[2], new Position(6, 1)),
            new Turn(m_playerActivePieces[1], m_opponentActivePieces[1]),
            new Turn(m_opponentActivePieces[2], new Position(3, 1)),
            new Turn(m_playerActivePieces[0], new Position(2, 1)),
            new Turn(m_opponentActivePieces[0], new Position(7, 1)),
            new Turn(m_playerActivePieces[2], new Position(3, 3)),
        });

        NextTurn();
    }

    public void AddTurn(Piece piece, Piece target)
    {
        turns.Add(new Turn(piece, target));
        NextTurn();

    }

    public void AddTurn(Piece piece, Position target)
    {
        turns.Add(new Turn(piece, target));
        NextTurn();
    }

    void NextTurn()
    {
        if (turnIndex < turns.Count)
        {
            if (turns[turnIndex].AttackedPiece == null)
            {
                turns[turnIndex].Piece.GoTo(turns[turnIndex].Position);
            }
            else
            {
                turns[turnIndex].Piece.GoTo(turns[turnIndex].AttackedPiece);
            }
            turnIndex++;
        }
    }

    public class Turn
    {
        public Piece Piece;
        public Position Position;
        public Piece AttackedPiece;

        public Turn(Piece piece, Position position)
        {
            this.Piece = piece;
            this.Position = position;
        }

        public Turn(Piece piece, Piece attackedPiece)
        {
            this.Piece = piece;
            this.AttackedPiece = attackedPiece;
        }
    }
}
