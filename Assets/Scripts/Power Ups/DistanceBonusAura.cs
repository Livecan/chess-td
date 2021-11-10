using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistanceBonusAura : MonoBehaviour
{
    GameObject aura;

    void Start()
    {
        GetComponent<Piece>().OnDistanceBonus.AddListener(ChangeAura);
    }

    void ChangeAura(DistanceBonus distanceBonus)
    {
        if (aura == null)
        {
            aura = GetComponentsInChildren<Transform>(true).First(transform => transform.CompareTag("DistanceBonusCircle")).gameObject;
        }
        aura.SetActive(distanceBonus != null);
    }
}
