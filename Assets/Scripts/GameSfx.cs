using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSfx : MonoBehaviour
{
    [SerializeField] AudioClip startGameClip;

    private void Start()
    {
        GameManager.Manager.OnStartGame.AddListener(PlayOnStartGame);
    }

    private void PlayOnStartGame()
    {
        GetComponent<AudioSource>().PlayOneShot(startGameClip);
    }
}
