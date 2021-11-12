using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSfx : MonoBehaviour
{
    [SerializeField] AudioClip collectPowerUpClip;

    private void Start()
    {

        PowerUp powerUp = GetComponentInParent<PowerUp>();
        powerUp.OnDestroyEvent.AddListener(PlayOnCollected);
    }

    private void PlayOnCollected()
    {
        GetComponent<AudioSource>().PlayOneShot(collectPowerUpClip);
    }
}
