using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomSpawnManager : MonoBehaviour, ISpawnManager
{
    [SerializeField] List<Piece> m_piecesPrefabs;
    [SerializeField] List<int> m_spawnChances;
    IList<Position> m_spawnPositions;

    public void Initialize(IEnumerable<Position> spawnPositions)
    {
        this.m_spawnPositions = spawnPositions.ToList();
    }

    public void Spawn()
    {
        Piece[] allPieces = FindObjectsOfType<Piece>();

        // Out of spawn positions it needs to extract positions that are not occupied
        IList<Position> availablePositions = m_spawnPositions.Where(availablePosition => !allPieces.Any(piece => piece.Position.Equals(availablePosition))).ToList();

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
