using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tarte : AbstractItem
{
    private Public foule = null;

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return true;
    }

    protected override void InteractImpl()
    {
        if (InteractingPlayer != null)
        {
            // Animation de tarte dans tête

            InteractingPlayer.animator.CrossFadeNicely("Armature|tarteTaguele", 0);
            foule = GameObject.FindGameObjectWithTag("Public").GetComponent<Public>();
            if (foule != null)
                foule.publicEnDelire();
            else
                Debug.Log("Pas De Foule");
            //Si le joueur est au sol, trick
            if (InteractingPlayer.InputHub.ReadIsGrounded())
            {
                Trick trick = new Trick
                {
                    Direction = InteractingPlayer.InputHub.ReadPlayerDirection(),
                    ItemSource = ItemCategory,
                    IsSuperTrick = false
                };
                GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, trick);
            }
            else //Sinon supertrick
            {
                Trick supertrick = new Trick
                {
                    Direction = InteractingPlayer.InputHub.ReadPlayerDirection(),
                    ItemSource = ItemCategory,
                    IsSuperTrick = true
                };
                GameEvents.ScoreBonus?.Invoke(PerfectPointBonus, PerfectMultiplierBonus, InteractTimeBonus, supertrick);
            }
            Destroy(gameObject, 0.3f);
        }
    }

}
