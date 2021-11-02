using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour, ISpawnManager
{
    [SerializeField] List<Piece> myPrefabs;
    [SerializeField] List<int> prices;

    List<Position> m_spawnPositions;

    IList<Piece> spawnQueue = new List<Piece>();

    public UnityEvent<int> OnBalanceChanged { get; private set; } = new UnityEvent<int>();

    int balance = 0;
    int Balance
    {
        get => balance;
        set
        {
            balance = value;
            OnBalanceChanged.Invoke(balance);
        }
    }

    public void AddKill(Piece piece)
    {
        Balance += piece.Strength;
        Debug.Log("Reward balance: " + Balance);
    }

    public void PurchaseUnit(Piece chosenPrefab)
    {
        int prefabIndex = myPrefabs.IndexOf(chosenPrefab);
        if (Balance >= prices[prefabIndex]) {
            Balance -= prices[prefabIndex];
            spawnQueue.Add(chosenPrefab);
        }
    }

    public void Initialize(IEnumerable<Position> spawnPositions)
    {
        this.m_spawnPositions = spawnPositions.ToList();
    }

    public void Spawn()
    {
        Piece[] allPieces = FindObjectsOfType<Piece>();

        // TODO: copied from SpawnManager - move into interface static method
        // Out of spawn positions it needs to extract positions that are not occupied
        IList<Position> availablePositions = m_spawnPositions.Where(availablePosition => !allPieces.Any(piece => piece.CurrentPosition.Equals(availablePosition))).ToList();

        // TODO include any free space in condition
        while (spawnQueue.Count > 0 && availablePositions.Count > 0)
        {
            Position spawnPosition = availablePositions[Random.Range(0, availablePositions.Count - 1)];

            spawnQueue.First().GetCopy(spawnPosition);

            availablePositions.Remove(spawnPosition);
            spawnQueue.RemoveAt(0);
        }
    }
}
