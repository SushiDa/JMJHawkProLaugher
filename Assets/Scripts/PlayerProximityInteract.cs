using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProximityInteract : MonoBehaviour
{
    private List<AbstractItem> _interactables = new List<AbstractItem>();
    private SushiTestPlayer _player;

    AbstractItem currentHover;
    AbstractItem activeItem;

    private void Awake()
    {
        _player = GetComponent<SushiTestPlayer>();
    }

    private void Update()
    {
        float closestDistance = Mathf.Infinity;
        AbstractItem hover = null;

        foreach(var interact in _interactables)
        {
            float distance = Vector3.Distance(transform.position, interact.transform.position);
            if (distance < closestDistance && !interact.Triggered) hover = interact;
        }

        if (hover != null && currentHover != hover)
        {
            currentHover = hover;
            foreach (var interact in _interactables)
                interact.Hover(interact == hover);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out AbstractItem interact))
        {
            _interactables.Add(interact);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out AbstractItem interact))
        {
            _interactables.Remove(interact);
            interact.Hover(false);
            if (interact == currentHover) currentHover = null;
        }
    }

    private void OnInteract(InputValue value)
    {
        if (value.isPressed && !_player.InteractLocked)
        {
            CancelActiveItem(false, ItemCancelTypes.INTERACT);

            if(currentHover != null)
            {
                CancelActiveItem(true, ItemCancelTypes.INTERACT);
                currentHover.Interact(_player);
                if(currentHover.IsActive)
                    activeItem = currentHover;
            }
        }
    }

    private void OnJump(InputValue value)
    {
        if(value.isPressed && !_player.JumpLocked)
        {
            CancelActiveItem(false, ItemCancelTypes.JUMP);
        }
    }

    private void CancelActiveItem(bool force, ItemCancelTypes cancelType)
    {
        if (activeItem != null && activeItem.IsActive && (force || activeItem.IsCancellable(cancelType)))
        {
            activeItem.CancelUse();
            activeItem = null;
        }
        else if(force)
        {
            activeItem = null;
        }
    }
}
