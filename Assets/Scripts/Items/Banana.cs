using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : AbstractItem
{
    [SerializeField] private float SlideSpeed;
    [SerializeField] private float SlideDuration;
    [SerializeField] private Vector2 SlidePerfectTiming;
    [SerializeField] private GameObject NormalBanana;
    [SerializeField] private GameObject SqaushedBanana;

    private float currentSlideTimer;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialParent = transform.parent;
    }
    internal override bool CanInteractImpl(PlayerGameController player)
    {
        PlayerDirection direction = player.InputHub.ReadPlayerDirection();
        return direction == PlayerDirection.HEAD || direction == PlayerDirection.FOOT;
    }
    protected override void InteractImpl()
    {
        if(InteractingPlayer != null)
        {
            transform.parent = InteractingPlayer.FootTransform;
            transform.localPosition = Vector3.zero;
            
            InteractingPlayer.InputHub.CanMove = false;
            InteractingPlayer.InputHub.CanRotate = false;
            InteractingPlayer.InputHub.CanJump = true;

            NormalBanana.SetActive(false);
            SqaushedBanana.SetActive(true);
            // Le perso glisse en avant
            InteractingPlayer.rigidbody.velocity = InteractingPlayer.FixedOrientator.right * SlideSpeed * Mathf.Sign(InteractingPlayer.GetComponent<Rigidbody>().velocity.x);
            InteractingPlayer.rigidbody.MoveRotation(Quaternion.identity);
            // ChangeAnimSlide
            switch(InteractingPlayer.InputHub.ReadPlayerDirection())
            {
                case PlayerDirection.FOOT:
                    // Normal stuff
                    Trick trick = new Trick
                    {
                        Direction = PlayerDirection.FOOT,
                        ItemSource = ItemCategory,
                        IsSuperTrick = false
                    };
                    GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, trick);
                    break;
                case PlayerDirection.HEAD:
                    // RAD stuff
                    Trick supertrick = new Trick
                    {
                        Direction = PlayerDirection.HEAD,
                        ItemSource = ItemCategory,
                        IsSuperTrick = true
                    };
                    GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, supertrick);
                    break;
                default:

                    break;
            }


        }
    }

    private void Update()
    {
        if(IsActive && currentSlideTimer < SlideDuration)
        {
            currentSlideTimer += Time.deltaTime;
            if(currentSlideTimer >= SlideDuration)
            {
                CancelUse();
            }
        }
    }

    protected override void CancelImpl()
    {
        NormalBanana.SetActive(false);
        SqaushedBanana.SetActive(false);

        if (InteractingPlayer != null)
        {
            InteractingPlayer.InputHub.CanMove = true;
            InteractingPlayer.InputHub.CanRotate = true;
            InteractingPlayer.InputHub.CanJump = true;
        }
    }

    protected override void ResetImpl()
    {
        NormalBanana.SetActive(true);
        SqaushedBanana.SetActive(false);
        currentSlideTimer = 0;
    }
}
