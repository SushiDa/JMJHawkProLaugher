using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : AbstractItem
{
    [SerializeField] private Vector2 MinMaxUpVelocity;
    [SerializeField] private float UpVelocityMultiplier;

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        bool isFalling = player.rigidbody.velocity.y < 0;

        return isFalling;

    }
    protected override void InteractImpl()
    {
        if (InteractingPlayer != null)
        {
            // PlayerCanDab = true;

            // PlayAnimation
            PlayerDirection direction = InteractingPlayer.InputHub.ReadPlayerDirection();
            Vector3 velocity = InteractingPlayer.rigidbody.velocity;
            velocity.y = Mathf.Clamp(Mathf.Abs(velocity.y) * UpVelocityMultiplier, MinMaxUpVelocity.x, MinMaxUpVelocity.y);
            InteractingPlayer.rigidbody.velocity = velocity;

            
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
