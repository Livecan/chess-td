using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSfx : MonoBehaviour
{
    [SerializeField] AudioClip destroyClip;
    [SerializeField] AudioClip hitClip;
    static AudioSource globalAudioSource;

    Piece piece;

    private void Start()
    {
        if (globalAudioSource == null)
        {
            globalAudioSource = GameObject.Find("Audio Player").GetComponent<AudioSource>();
        }

        piece = GetComponentInParent<Piece>();
        piece.OnAttacked.AddListener(PlayOnAttacked);
    }

    private void PlayOnAttacked(bool destroyed)
    {
        if (destroyed)
        {
            globalAudioSource.PlayOneShot(destroyClip);
        }
        else
        {
            globalAudioSource.PlayOneShot(hitClip);
        }
    }
}
