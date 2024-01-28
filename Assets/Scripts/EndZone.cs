using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerGameController player))
        {
            ps.Play();
            GameEvents.WaveFinished?.Invoke(true);
        }
    }
}
