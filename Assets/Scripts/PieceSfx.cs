using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSfx : MonoBehaviour
{
    [SerializeField] AudioClip destroyClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip walkingClip;
    AudioSource audioSource;

    Piece piece;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); //GameObject.Find("Audio Player").GetComponent<AudioSource>();
        }

        piece = GetComponentInParent<Piece>();
        piece.OnAttacked.AddListener(PlayOnAttacked);
        piece.OnStartMove.AddListener(PlayOnWalking);
        piece.OnFinishedMove.AddListener(StopPlayback);
    }

    private void PlayOnAttacked(bool destroyed)
    {
        if (destroyed)
        {
            GameManager.Manager.GetComponentInChildren<AudioSource>().PlayOneShot(destroyClip);
        }
        else
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    private void PlayOnWalking()
    {
        audioSource.clip = walkingClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void StopPlayback()
    {
        audioSource.Stop();
    }
}
