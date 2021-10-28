using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    List<Piece> myPrefabs;

    int balance = 0;

    public void AddKill(Piece piece)
    {
        balance += piece.Strength;
        Debug.Log("Reward balance: " + balance);
    }
}
