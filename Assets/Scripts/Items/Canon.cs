using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : AbstractItem
{

    private float shotDelayTimer;
    private bool isFiring = false;

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return false;
    }


    protected override void InteractImpl()
    {
        isFiring = true;
    }

    void Update()
    {
        
        if (isFiring && shotDelayTimer < 2) 
        {
            shotDelayTimer += Time.deltaTime;
            if (shotDelayTimer >= 2) 
            {
                isFiring = false;
                shotDelayTimer = 0;
                AudioBridge.PlaySFX("Canon");
                //Animation du tir
                //Create a TARTE
            }
            
        }
    }
}
