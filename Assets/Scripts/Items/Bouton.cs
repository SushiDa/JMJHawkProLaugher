using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouton : AbstractItem
{ 


    /*private void Awake()
    {
        initialPosition = transform.localPosition;
        initialParent = transform.parent;
    }*/

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return true;
    }

    protected override void InteractImpl()
    {
        if (InteractingPlayer != null && LinkedItem != null)
        {
            LinkedItem.Interact(InteractingPlayer);
        }
    }
    void Update()
    {
        
    }
}
