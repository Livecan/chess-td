using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Piece> playerPiecesPrefabs;
    [SerializeField] private List<Piece> opponentPiecesPrefabs;

    private List<Piece> m_playerActivePieces = new List<Piece>();
    private List<Piece> m_opponentActivePieces = new List<Piece>();

    private bool playerTurn = true;
    private Turn[] turns;
    private int turnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(NextTurn));
        opponentPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(NextTurn));

        for (int i = 0; i < 3; i++)
        {
            Piece playerPiece = Instantiate(playerPiecesPrefabs[i]);
            playerPiece.StartPosition = new Piece.Position(0, i + 1);
            m_playerActivePieces.Add(playerPiece);
            m_playerActivePieces[i].OnFinishedMove.AddListener(NextTurn);

            Piece opponentPiece = Instantiate(opponentPiecesPrefabs[i]);
            opponentPiece.StartPosition = new Piece.Position(8, i + 1);
            m_opponentActivePieces.Add(opponentPiece);
            m_opponentActivePieces[i].OnFinishedMove.AddListener(NextTurn);
        }

        turns = new Turn[]
        {
            new Turn(m_playerActivePieces[0], new Piece.Position(1, 1)),
            new Turn(m_opponentActivePieces[1], new Piece.Position(5, 2)),
            new Turn(m_playerActivePieces[1], new Piece.Position(3, 2)),
            new Turn(m_opponentActivePieces[2], new Piece.Position(6, 1)),
            new Turn(m_playerActivePieces[0], new Piece.Position(2, 1)),
            new Turn(m_opponentActivePieces[2], new Piece.Position(3, 1)),
            new Turn(m_playerActivePieces[2], new Piece.Position(3, 3)),
            new Turn(m_opponentActivePieces[0], new Piece.Position(7, 1)),
        };

        NextTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextTurn()
    {
        if (turnIndex < turns.Length)
        {
            turns[turnIndex].Piece.GoTo(turns[turnIndex].Position);
            turnIndex++;
        }
    }

    public class Turn
    {
        public Piece Piece;
        public Piece.Position Position;

        public Turn(Piece piece, Piece.Position position)
        {
            this.Piece = piece;
            this.Position = position;
        }
    }
}