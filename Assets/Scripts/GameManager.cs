using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnManager m_opponentSpawnManager;
    [SerializeField] private List<Piece> m_playerPiecesPrefabs;
    [SerializeField] private List<Piece> m_opponentPiecesPrefabs;

    private int m_turnIndex = 1;

    private IController m_playerController;
    private IController m_opponentController;

    public readonly int m_fieldColumns = 9, m_fieldRows = 5;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize SpawnManager with spawning positions - the first column on opponent's side
        m_opponentSpawnManager.Initialize(
            new Position[] {
                new Position(m_fieldColumns - 1, 0),
                new Position(m_fieldColumns - 1, 1),
                new Position(m_fieldColumns - 1, 2),
                new Position(m_fieldColumns - 1, 3),
                new Position(m_fieldColumns - 1, 4),
            }
        );

        m_playerPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(() => NextTurn()));
        m_opponentPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(() => NextTurn()));

        for (int i = 0; i < 3; i++)
        {
            InitializePiece(m_playerPiecesPrefabs[i], new Position(0, i + 1));
            InitializePiece(m_opponentPiecesPrefabs[i], new Position(8, i + 1));
        }


        InitializeController(m_playerController = FindObjectOfType<UserController>(), IController.Direction.Right);
        InitializeController(m_opponentController = FindObjectOfType<AIController>(), IController.Direction.Left);

        NextTurn();
    }

    // Instantiates a Piece according to a prefab and assigns is the given position and the NextTurn listener
    public Piece InitializePiece(Piece piecePrefab, Position position)
    {
        Piece piece = Instantiate(piecePrefab);
        piece.StartPosition = position;
        piece.OnFinishedMove.AddListener(() => NextTurn());
        return piece;
    }

    // Initializes controller's with necessary values - especially important for the AI Controller
    private void InitializeController(IController controller, IController.Direction attackDirection)
    {
        controller.TurnCallback = NextTurn;
        controller.AttackDirection = attackDirection;

    }

    // Performs the given turn if it is valid
    void NextTurn(Turn turn)
    {
        if (!turn.Piece.GetAvailablePositions().Contains(turn.Position) && !turn.Piece.GetAvailablePositions().Contains(turn.AttackedPiece?.CurrentPosition))
        {
            NextTurn(false);
        }
        else
        {
            if (turn.AttackedPiece == null)
            {
                turn.Piece.GoTo(turn.Position);
            }
            else
            {
                turn.Piece.GoTo(turn.AttackedPiece);
            }
        }
    }

    // Starts a turn for the next player (or asks the current player for the correct turn)
    void NextTurn(bool nextPlayer = true)
    {
        List<Piece> m_playerActivePieces = FindObjectsOfType<Piece>().Where(piece => piece.gameObject.CompareTag("Player")).ToList();
        List<Piece> m_opponentActivePieces = FindObjectsOfType<Piece>().Where(piece => piece.gameObject.CompareTag("Opponent")).ToList();
        if (nextPlayer)
        {
            m_turnIndex = (m_turnIndex + 1) % 2;
        }

        Debug.Log("Player turn: " + m_turnIndex);

        if (m_turnIndex == 0)
        {
            m_opponentSpawnManager.Spawn();
            m_playerController.GetTurn(m_playerActivePieces.ToList(), m_opponentActivePieces.ToList());
        }
        else
        {
            m_opponentController.GetTurn(m_opponentActivePieces.ToList(), m_playerActivePieces.ToList());
        }

    }
}
