using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<Piece> m_piecesPrefabs;
    [SerializeField] List<int> m_spawnChances;
    IList<Position> m_spawnPositions;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Initialize(IEnumerable<Position> m_spawnPositions)
    {
        this.m_spawnPositions = m_spawnPositions.ToList();
    }

    public void Spawn()
    {
        Piece[] allPieces = FindObjectsOfType<Piece>();

        // Out of spawn positions it needs to extract positions that are not occupied
        IList<Position> availablePositions = m_spawnPositions.Where(availablePosition => !allPieces.Any(piece => piece.CurrentPosition.Equals(availablePosition))).ToList();

        //Each prefab is being spawned at random rate - m_spawnChances - and at random position if there is space for spawning
        for (int i = 0; i < m_piecesPrefabs.Count && availablePositions.Count > 0; i++)
        {
            if (Random.Range(0, 100) < m_spawnChances[i])
            {
                int positionIndex = Random.Range(0, availablePositions.Count);

                Position spawnPosition = availablePositions[positionIndex];

                m_piecesPrefabs[i].GetCopy(spawnPosition);

                availablePositions.Remove(spawnPosition);
            }
        }
    }
}
