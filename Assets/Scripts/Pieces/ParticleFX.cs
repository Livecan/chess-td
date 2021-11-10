using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
    [SerializeField] ParticleSystem destroyFxPrefab;

    [SerializeField] ParticleSystem attackedFxPrefab;

    private void OnAttacked(bool destroyed)
    {
        Debug.Log("AttackedParticle");
        ParticleSystem attackedFx = Instantiate(destroyed ? destroyFxPrefab : attackedFxPrefab, transform.position, transform.rotation);
        /*var main = attackedFx.main;
        main.simulationSpeed = 0.1f;*/
        attackedFx.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Init FX");
        gameObject.GetComponentInParent<Piece>().OnAttacked.AddListener(OnAttacked);
    }
}
