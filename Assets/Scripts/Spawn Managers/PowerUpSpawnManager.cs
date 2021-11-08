using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnManager : MonoBehaviour, ISpawnManager
{
    [SerializeField] List<GameObject> powerUpPrefabs;
    [SerializeField] List<int> spawnChances;
    IList<Position> spawnPositions;

    public void Initialize(IEnumerable<Position> spawnPositions)
    {
        this.spawnPositions = spawnPositions.ToList();
    }

    public void Spawn()
    {
        List<Position> occupiedPositions = FindObjectsOfType<Piece>().Select(piece => piece.Position).ToList();
        occupiedPositions.AddRange(
            FindObjectsOfType<PowerUp>().Select(powerUp => powerUp.Position)
        );

        IList<Position> availablePositions = spawnPositions.Where(availablePosition => !occupiedPositions.Any(occupiedPosition => occupiedPosition.Equals(availablePosition))).ToList();
        for (int i = 0; i < Mathf.Min(powerUpPrefabs.Count, spawnChances.Count); i++)
        {
            if (availablePositions.Count > 0 && Random.Range(0, 100) < spawnChances[i])
            {
                Position randomAvailablePosition = availablePositions[Random.Range(0, availablePositions.Count)];
                GameObject powerUp = Instantiate(powerUpPrefabs[i]);
                powerUp.GetComponent<PowerUp>().Position = randomAvailablePosition;

                availablePositions.Remove(randomAvailablePosition);
            }
        }
    }
}
