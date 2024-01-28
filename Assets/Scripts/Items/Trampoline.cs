using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : AbstractItem
{
    [SerializeField] private Vector2 MinMaxUpVelocity;
    [SerializeField] private float UpVelocityMultiplier;
    //Qui m'empechera de faire des var public ????
    public Animator animator;

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        bool isFalling = player.rigidbody.velocity.y < 0;

        return isFalling;

    }
    protected override void InteractImpl()
    {
        if (InteractingPlayer != null)
        {
            animator.SetBool("Interact", true);
            //DeactivateAllChildren(gameObject);
            Destroy(gameObject, 0.3f);
            // PlayerCanDab = true;


            // PlayAnimation
            PlayerDirection direction = InteractingPlayer.InputHub.ReadPlayerDirection();
            Vector3 velocity = InteractingPlayer.rigidbody.velocity;
            velocity.y = Mathf.Clamp(Mathf.Abs(velocity.y) * UpVelocityMultiplier, MinMaxUpVelocity.x, MinMaxUpVelocity.y);
            InteractingPlayer.rigidbody.velocity = velocity;
            InteractingPlayer.animator.CrossFadeNicely("Armature|Jumpascend 0", 0);

            Trick trick = new Trick
            {
                Direction = InteractingPlayer.InputHub.ReadPlayerDirection(),
                ItemSource = ItemCategory,
                IsSuperTrick = false
            };
            GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, trick);
                    
        }
    }

    void DeactivateAllChildren(GameObject parent)
    {
        int childCount = parent.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
}
