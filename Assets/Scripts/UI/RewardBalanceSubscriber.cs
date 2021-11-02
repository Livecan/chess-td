using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardBalanceSubscriber : MonoBehaviour
{
    public RewardManager rewardManager;
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        rewardManager.OnBalanceChanged.AddListener((int balance) => text.text = "Rewards balance: " + balance);   
    }


}
