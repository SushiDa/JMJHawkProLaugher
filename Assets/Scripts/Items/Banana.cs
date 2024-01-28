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

    protected override bool InteractImpl()
    {
        if(InteractingPlayer != null)
        {
            transform.parent = InteractingPlayer.FootTransform;
            transform.localPosition = Vector3.zero;
            
            InteractingPlayer.MovementLocked = true;
            InteractingPlayer.InteractLocked = true;
            InteractingPlayer.JumpLocked = false;

            NormalBanana.SetActive(false);
            SqaushedBanana.SetActive(true);
            // Le perso glisse en avant
            InteractingPlayer.rb.velocity = Vector3.right * SlideSpeed * Mathf.Sign(InteractingPlayer.GetComponent<Rigidbody>().velocity.x);

            // ChangeAnimSlide
            switch(InteractingPlayer.GetCurrentDirection())
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
                case PlayerDirection.HAND:
                    // RAD stuff
                    Trick supertrick = new Trick
                    {
                        Direction = PlayerDirection.HAND,
                        ItemSource = ItemCategory,
                        IsSuperTrick = true
                    };
                    GameEvents.ScoreBonus?.Invoke(PointBonus, MultiplierBonus, InteractTimeBonus, supertrick);
                    break;
                default:

                    break;
            }
            return true;
        }
        return false;
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
            InteractingPlayer.MovementLocked = false;
            InteractingPlayer.InteractLocked = false;
            InteractingPlayer.JumpLocked = false;
        }
    }

    protected override void ResetImpl()
    {
        NormalBanana.SetActive(true);
        SqaushedBanana.SetActive(false);
        currentSlideTimer = 0;
    }
}
