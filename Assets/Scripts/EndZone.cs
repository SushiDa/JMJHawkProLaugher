using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out SushiTestPlayer player))
        {
            GameEvents.WaveFinished?.Invoke(true);
        }
    }
}
