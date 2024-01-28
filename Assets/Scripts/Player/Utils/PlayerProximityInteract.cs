using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProximityInteract : MonoBehaviour
{
    private PlayerGameController _player;

    private List<AbstractItem> activeItems = new List<AbstractItem>();

    private void Awake()
    {
        _player = GetComponent<PlayerGameController>();
        GameEvents.PlayerJump += OnJump;
    }

    private void OnDestroy()
    {
        GameEvents.PlayerJump -= OnJump;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out AbstractItem item))
        {
            if (item.CanInteract(_player))
            {
                if(item.CancelPreviousItems)
                {
                    CancelActiveItem(true, ItemCancelTypes.ITEM);
                }

                item.Interact(_player);
                if (item.CancellableLongInteraction && item.IsActive) activeItems.Add(item);
            }
        }
    }

    private void OnJump()
    {
        CancelActiveItem(false, ItemCancelTypes.JUMP);
    }
    internal void CancelActiveItem(bool force, ItemCancelTypes cancelType)
    {
        List<AbstractItem> itemsToCancel = new List<AbstractItem>();
        foreach (var activeItem in activeItems)
        {
            if (activeItem != null && activeItem.IsActive && (force || activeItem.IsCancellable(cancelType)))
            {
                itemsToCancel.Add(activeItem);
            }
        }
        
        foreach( var activeItem in itemsToCancel)
            activeItem.CancelUse();
    }

    internal void RemoveActiveItem(AbstractItem item)
    {
        activeItems.Remove(item);
    }
}
