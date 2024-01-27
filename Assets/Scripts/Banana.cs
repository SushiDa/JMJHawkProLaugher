using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : AbstractItem
{
    [SerializeField] private float SlideSpeed;
    [SerializeField] private float SlideDuration;
    [SerializeField] private Vector2 SlidePerfectTiming;

    private float currentSlideTimer;
    private bool sliding;

    private GameObject CurrentInteracting;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialParent = transform.parent;
    }

    internal override bool Interact(GameObject source)
    {
        if(source != null && !triggered)
        {
            triggered = true;
            CurrentInteracting = source;
            GetComponent<MeshRenderer>().material.color = Color.red;
            var player = source.GetComponent<SushiTestPlayer>();
            transform.parent = player.FootTransform;
            transform.localPosition = Vector3.zero;
            
            player.MovementLocked = true;
            player.InteractLocked = true;
            player.JumpLocked = false;
            // Le perso glisse en avant
            player.GetComponent<Rigidbody>().velocity = Vector3.right * SlideSpeed * Mathf.Sign(player.GetComponent<Rigidbody>().velocity.x);

            sliding = true;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if(sliding && currentSlideTimer < SlideDuration)
        {
            currentSlideTimer += Time.deltaTime;
            if(currentSlideTimer >= SlideDuration)
            {
                Cancel();
            }
        }
    }

    private void Cancel()
    {
        sliding = false;
        GetComponent<MeshRenderer>().enabled = false;

        if (CurrentInteracting != null)
        {
            var player = CurrentInteracting.GetComponent<SushiTestPlayer>();
            player.MovementLocked = false;
            player.InteractLocked = false;
            player.JumpLocked = false;
        }

        CurrentInteracting = null;
    }

    internal override void ResetImpl()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        GetComponent<MeshRenderer>().enabled = true;
        CurrentInteracting = null;
        sliding = false;
        currentSlideTimer = 0;
    }
    internal override void HoverImpl(bool hovering)
    {
        if(!triggered) GetComponent<MeshRenderer>().material.color = hovering ? Color.yellow : Color.white;
    }

    private void OnTriggerExit(Collider other)
    {
        // Le perso tombe
    }
}
