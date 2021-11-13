using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
    [SerializeField] ParticleSystem destroyFxPrefab;
    [SerializeField] ParticleSystem attackedFxPrefab;
    [SerializeField] ParticleSystem powerUpCollectedFxPrefab;

    private void OnAttacked(bool destroyed)
    {
        ParticleSystem attackedFx = Instantiate(destroyed ? destroyFxPrefab : attackedFxPrefab, transform.position, transform.rotation);
        attackedFx.Play();
        StartCoroutine(DestroyParticleEffect(attackedFx));
    }

    private void OnPowerUpCollected()
    {
        ParticleSystem powerUpCollectedFx = Instantiate(powerUpCollectedFxPrefab, transform.position, transform.rotation);
        powerUpCollectedFx.Play();
        StartCoroutine(DestroyParticleEffect(powerUpCollectedFx));
    }

    private IEnumerator DestroyParticleEffect(ParticleSystem particleSystem)
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(particleSystem.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Piece myPiece = gameObject.GetComponentInParent<Piece>();
        myPiece.OnAttacked.AddListener(OnAttacked);
        myPiece.OnPowerUpCollected.AddListener(OnPowerUpCollected);
    }
}
