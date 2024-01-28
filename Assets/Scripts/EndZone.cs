using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerGameController player))
        {
            GameEvents.WaveFinished?.Invoke(true);

        }
    }
}
