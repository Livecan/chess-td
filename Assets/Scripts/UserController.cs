using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserController : MonoBehaviour, IController
{
    private GameManager gameManager;
    private Piece m_selectedPiece;

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
                throw new System.InvalidOperationException("Turn callback already initialized");
            }
            m_attackDirection = value;
        }
    }

    private List<Piece> myPieces;
    private List<Piece> opponentPieces;

    private bool isMyTurn = false;

    // TODO: make a parent object from Piece and Position?

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Plane")))
            {
                Position selectedPosition = new Position(hit.point);
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
        Debug.Log("Selected position");
        if (m_selectedPiece != null && m_selectedPiece.GetAvailablePositions().Contains(selectedPosition))
        {
            SubmitTurn(new Turn(m_selectedPiece, selectedPosition));
        }
    }

    void SelectPiece(Piece selectedPiece)
    {
        Debug.Log(selectedPiece);
        // the player picks his piece to play
        if (myPieces.Contains(selectedPiece))
        {
            m_selectedPiece = selectedPiece;
        }
        // the player must have chosen his piece to play and the selectedPiece is the opponent, the opponent's position must be allowed
        else if (m_selectedPiece != null && m_selectedPiece.GetAvailablePositions().Contains(selectedPiece.CurrentPosition))
        {
            SubmitTurn(new Turn(m_selectedPiece, selectedPiece));
        }
    }

    private bool GetPositionHasPiece(Position position, out Piece piece)
    {
        piece = FindObjectsOfType<Piece>().FirstOrDefault(piece => piece.CurrentPosition.Column == position.Column && piece.CurrentPosition.Row == position.Row);
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
        Debug.Log("SubmitTurn");
        isMyTurn = false;
        m_selectedPiece = null;
        m_turnCallback.Invoke(turn);
    }
}
