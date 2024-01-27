using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : AbstractItem
{
    [SerializeField] private float SlideSpeed;
    [SerializeField] private float SlideDuration;
    [SerializeField] private Vector2 SlidePerfectTiming;

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
            GetComponent<MeshRenderer>().material.color = Color.red;
            transform.parent = InteractingPlayer.FootTransform;
            transform.localPosition = Vector3.zero;
            
            InteractingPlayer.MovementLocked = true;
            InteractingPlayer.InteractLocked = true;
            InteractingPlayer.JumpLocked = false;
            // Le perso glisse en avant
            InteractingPlayer.rb.velocity = Vector3.right * SlideSpeed * Mathf.Sign(InteractingPlayer.GetComponent<Rigidbody>().velocity.x);
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
        GetComponent<MeshRenderer>().enabled = false;

        if (InteractingPlayer != null)
        {
            InteractingPlayer.MovementLocked = false;
            InteractingPlayer.InteractLocked = false;
            InteractingPlayer.JumpLocked = false;
        }
    }

    protected override void ResetImpl()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        GetComponent<MeshRenderer>().enabled = true;
        currentSlideTimer = 0;
    }

    protected override void HoverImpl(bool hovering)
    {
        GetComponent<MeshRenderer>().material.color = hovering ? Color.yellow : Color.white;
    }
}
