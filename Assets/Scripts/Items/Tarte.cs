using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarte : AbstractItem
{


    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return true;
    }

    protected override void InteractImpl()
    {
        if (InteractingPlayer != null)
        {
            // Animation de tarte dans tête

        }
    }

}
