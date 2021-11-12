using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSfx : MonoBehaviour
{
    [SerializeField] AudioClip collectPowerUpClip;
    static AudioSource globalAudioSource;

    private void Start()
    {
        if (globalAudioSource == null)
        {
            globalAudioSource = GameObject.Find("Audio Player").GetComponent<AudioSource>();
        }

        PowerUp powerUp = GetComponentInParent<PowerUp>();
        powerUp.OnDestroyEvent.AddListener(PlayOnCollected);
    }

    private void PlayOnCollected()
    {
        globalAudioSource.PlayOneShot(collectPowerUpClip);
    }
}
