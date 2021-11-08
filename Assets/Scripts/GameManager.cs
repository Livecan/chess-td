using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private List<Piece> m_playerPiecesPrefabs;
    [SerializeField] private List<Piece> m_opponentPiecesPrefabs;

    [SerializeField] private GameObject playerController;
    [SerializeField] private GameObject opponentController;

    private ISpawnManager m_powerUpSpawnManager;

    private int m_turnIndex = 1;

    public bool HasExtraTurn { get; set; }

    public int FieldColumns { get; private set; } = 9;
    public int FieldRows { get; private set; } = 5;

    private static GameManager gameManager;

    public static GameManager Manager
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }
            return gameManager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int column = 0; column < FieldColumns; column++)
        {
            for (int row = 0; row < FieldRows; row++)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.GetComponent<PositionedObject>().Position = Position.getPosition(column, row);
            }
        }

        InitializeController(playerController.GetComponent<UserController>(), IController.Direction.Right);
        InitializeController(opponentController.GetComponent<AIController>(), IController.Direction.Left);

        // Initialize SpawnManager with spawning positions - the first column on opponent's side
        opponentController.GetComponent<ISpawnManager>().Initialize(
            new Position[] {
                Position.getPosition(FieldColumns - 1, 0),
                Position.getPosition(FieldColumns - 1, 1),
                Position.getPosition(FieldColumns - 1, 2),
                Position.getPosition(FieldColumns - 1, 3),
                Position.getPosition(FieldColumns - 1, 4),
            }
        );

        RewardManager playerRewardManager = playerController.GetComponent<RewardManager>();

        // Initialize PlayerRewardManager similarly to Opponent SpawnManager - the first column on player's side
        playerRewardManager.Initialize(
            new Position[] {
                Position.getPosition(0, 0),
                Position.getPosition(0, 1),
                Position.getPosition(0, 2),
                Position.getPosition(0, 3),
                Position.getPosition(0, 4),
            }
        );

        m_powerUpSpawnManager = FindObjectOfType<PowerUpSpawnManager>();
        m_powerUpSpawnManager.Initialize(
            new Position[] {
                Position.getPosition(3, 0),
                Position.getPosition(3, 1),
                Position.getPosition(3, 2),
                Position.getPosition(3, 3),
                Position.getPosition(3, 4),
                Position.getPosition(4, 0),
                Position.getPosition(4, 1),
                Position.getPosition(4, 2),
                Position.getPosition(4, 3),
                Position.getPosition(4, 4),
                Position.getPosition(5, 0),
                Position.getPosition(5, 1),
                Position.getPosition(5, 2),
                Position.getPosition(5, 3),
                Position.getPosition(5, 4),
            }
        );

        m_playerPiecesPrefabs.ForEach(piece => {
            piece.OnFinishedMove.AddListener(() => NextTurn());
            piece.OnKill.AddListener(playerRewardManager.AddKill);
        });

        m_opponentPiecesPrefabs.ForEach(piece => piece.OnFinishedMove.AddListener(() => NextTurn()));

        for (int i = 0; i < 3; i++)
        {
            m_playerPiecesPrefabs[i].GetCopy(Position.getPosition(0, i + 1));
            m_opponentPiecesPrefabs[i].GetCopy(Position.getPosition(8, i + 1));
        }

        NextTurn();
    }

    // Initializes controller's with necessary values
    private void InitializeController(IController controller, IController.Direction attackDirection)
    {
        controller.TurnCallback = NextTurn;
        controller.AttackDirection = attackDirection;

    }

    // Performs the given turn if it is valid
    void NextTurn(Turn turn)
    {
        if (!turn.Piece.GetAvailablePositions().Contains(turn.Position) && !turn.Piece.GetAvailablePositions().Contains(turn.AttackedPiece?.Position))
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
        if (nextPlayer && !HasExtraTurn)
        {
            m_turnIndex = (m_turnIndex + 1) % 2;
        }

        HasExtraTurn = false;

        Debug.Log("Player turn: " + m_turnIndex);

        if (m_turnIndex == 0)
        {
            opponentController.GetComponent<ISpawnManager>().Spawn();
            playerController.GetComponent<UserController>().GetTurn(m_playerActivePieces.ToList(), m_opponentActivePieces.ToList());
        }
        else
        {
            m_powerUpSpawnManager.Spawn();
            playerController.GetComponent<ISpawnManager>().Spawn();
            opponentController.GetComponent<AIController>().GetTurn(m_opponentActivePieces.ToList(), m_playerActivePieces.ToList());
        }

    }
}
