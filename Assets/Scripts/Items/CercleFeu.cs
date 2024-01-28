using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CercleFeu : AbstractItem
{
    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return true;
    }

    protected override void InteractImpl()
    {
        if (InteractingPlayer != null)
        {
            //DAB
            InteractingPlayer.animator.CrossFadeNicely("Dab", 0);
            Trick trick = new Trick
            {
                Direction = InteractingPlayer.InputHub.ReadPlayerDirection(),
                ItemSource = ItemCategory,
                IsSuperTrick = false
            };
            GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, trick);
        }
    }
}
