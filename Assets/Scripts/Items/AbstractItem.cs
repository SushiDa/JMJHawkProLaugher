using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractItem : MonoBehaviour
{
    [SerializeField] internal bool CancelPreviousItems;
    [SerializeField] internal bool CancellableLongInteraction;
    [SerializeField] protected List<ItemCancelTypes> AvailableCancelActions;
    [SerializeField] protected string ItemCategory;
    [SerializeField] protected string SFXOnInteract;

    [SerializeField] internal float InteractTimeBonus;
    [SerializeField] internal int MultiplierBonus;
    [SerializeField] internal int PerfectMultiplierBonus;
    [SerializeField] internal int PointBonus;
    [SerializeField] internal int PerfectPointBonus;

    internal bool Triggered;
    internal bool IsActive;

    protected Vector2 initialPosition;
    protected Transform initialParent;
    protected PlayerGameController InteractingPlayer;

    internal bool CanInteract(PlayerGameController player) { 
        return !Triggered && CanInteractImpl(player); 
    }
    internal virtual bool CanInteractImpl(PlayerGameController player) { return false; }
    internal void Interact(PlayerGameController player)
    {
        if (!Triggered)
        {
            Triggered = true;
            IsActive = true;
            InteractingPlayer = player;
            InteractImpl();
            AudioBridge.PlaySFX(SFXOnInteract);
        }
    }
    internal void CancelUse()
    {
        IsActive = false;
        CancelImpl();
        ResetPositionAndParent();
        InteractingPlayer?.GetComponent<PlayerProximityInteract>()?.RemoveActiveItem(this);
        InteractingPlayer = null;
    }
    internal void Reset()
    {
        IsActive = false;
        Triggered = false;
        ResetPositionAndParent();
        InteractingPlayer?.GetComponent<PlayerProximityInteract>()?.RemoveActiveItem(this);
        InteractingPlayer = null;
    }
    protected virtual void InteractImpl() { }
    protected virtual void ResetImpl() { }
    protected virtual void CancelImpl() { }

    protected void ResetPositionAndParent()
    {
        transform.parent = initialParent;
        transform.localPosition = initialPosition;
    }
    internal bool IsCancellable(ItemCancelTypes cancelType)
    {
        return AvailableCancelActions.Contains(cancelType);
    }
}

public enum ItemCancelTypes
{
    NONE = 0,
    JUMP = 1,
    INTERACT = 2,
    ITEM = 3,
}