using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleUserInput : MonoBehaviour
{
    private GameManager gameManager;
    private Piece m_selectedPiece;
    // TODO: make a parent object from Piece and Position?

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        if (m_selectedPiece != null)
        {
            // TODO: consider GameManager subscribing to events here instead of calling GameManager.AddTurn method.
            //      in which case I can have a Controller Interface that the Player and the Opponent will both use.
            gameManager.AddTurn(m_selectedPiece, selectedPosition);
        }
    }

    void SelectPiece(Piece selectedPiece)
    {
        Debug.Log(selectedPiece);
        if (selectedPiece.gameObject.CompareTag("Player"))
        {
            m_selectedPiece = selectedPiece;
        }
        else if (m_selectedPiece != null && selectedPiece.gameObject.CompareTag("Opponent"))
        {
            gameManager.AddTurn(m_selectedPiece, selectedPiece);
        }
    }

    private bool GetPositionHasPiece(Position position, out Piece piece)
    {
        piece = FindObjectsOfType<Piece>().FirstOrDefault(piece => piece.CurrentPosition.Row == position.Row && piece.CurrentPosition.Column == position.Column);
        return piece != null;
    }
}
