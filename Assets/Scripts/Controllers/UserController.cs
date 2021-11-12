using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserController : MonoBehaviour, IController
{
    private GameManager gameManager;
    private Piece m_selectedPiece;

    [SerializeField] GameObject selectorAura;

    private System.Action<Turn> m_turnCallback;
    public System.Action<Turn> TurnCallback
    {
        set
        {
            if (m_turnCallback != null)
            {
                throw new System.InvalidOperationException("Turn callback already initialized");
            }
            m_turnCallback = value;
        }
    }

    private IController.Direction? m_attackDirection;
    public IController.Direction AttackDirection
    {
        set
        {
            if (m_attackDirection != null) {
                throw new System.InvalidOperationException("Attack direction already initialized");
            }
            m_attackDirection = value;
        }
    }

    private List<Piece> myPieces;
    private List<Piece> opponentPieces;

    private bool isMyTurn = false;

    private RewardManager rewardManager;

    // TODO: make a parent object from Piece and Position?

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rewardManager = gameObject.GetComponent<RewardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, LayerMask.GetMask("Plane")))
            {
                Position selectedPosition = hit.collider.GetComponent<PositionedObject>().Position;

                // TODO: check if valid position according to the rules - maybe render position objects for the player and only those would collide with Raycast?
                Piece selectedPiece;
                if (GetPositionHasPiece(selectedPosition, out selectedPiece))
                {
                    SelectPiece(selectedPiece);
                }
                else
                {
                    SelectPosition(selectedPosition);
                }
            }
        }
    }

    void SelectPosition(Position selectedPosition)
    {
        if (m_selectedPiece != null && m_selectedPiece.GetAvailablePositions().Contains(selectedPosition))
        {
            SubmitTurn(new Turn(m_selectedPiece, selectedPosition));
        }
    }

    void SelectPiece(Piece selectedPiece)
    {
        // the player picks his piece to play
        if (myPieces.Contains(selectedPiece))
        {
            m_selectedPiece = selectedPiece;
            selectorAura.transform.position = new Vector3(m_selectedPiece.transform.position.x, selectorAura.transform.position.y, m_selectedPiece.transform.position.z);
            selectorAura.SetActive(true);
        }
        // the player must have chosen his piece to play and the selectedPiece is the opponent, the opponent's position must be allowed
        else if (m_selectedPiece != null && m_selectedPiece.GetAvailablePositions().Contains(selectedPiece.Position))
        {
            SubmitTurn(new Turn(m_selectedPiece, selectedPiece));
        }
    }

    private bool GetPositionHasPiece(Position position, out Piece piece)
    {
        piece = FindObjectsOfType<Piece>().FirstOrDefault(piece => piece.Position.Column == position.Column && piece.Position.Row == position.Row);
        return piece != null;
    }

    public void GetTurn(List<Piece> myPieces, List<Piece> opponentPieces)
    {
        this.myPieces = myPieces;
        this.opponentPieces = opponentPieces;
        isMyTurn = true;
    }

    private void SubmitTurn(Turn turn)
    {
        selectorAura.SetActive(false);
        isMyTurn = false;
        m_selectedPiece = null;
        m_turnCallback(turn);
    }
}
