using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractItem : MonoBehaviour
{
    [SerializeField] List<ItemCancelTypes> AvailableCancelActions;

    [SerializeField] internal int MultiplierBonus;
    [SerializeField] internal int PerfectMultiplierBonus;
    [SerializeField] internal int PointBonus;
    [SerializeField] internal int PerfectPointBonus;

    internal Action HoverOnEvent;
    internal Action HoverOffEvent;
    internal Action ResetEvent;
    internal Action cancelEvent;

    internal bool Triggered;
    internal bool IsActive;

    protected Vector2 initialPosition;
    protected Transform initialParent;

    protected SushiTestPlayer InteractingPlayer;

    

    internal bool Interact(SushiTestPlayer player)
    {
        if (!Triggered)
        {
            Triggered = true;
            IsActive = true;
            InteractingPlayer = player;
            bool interacting = InteractImpl();
            if (!interacting) CancelUse(); // if simple interaction, immediately cancel
            return interacting;
        }
        return false;
    }

    internal void Hover(bool hovering)
    {
        if (!Triggered)
        {
            if (hovering) HoverOnEvent?.Invoke();
            else HoverOffEvent?.Invoke();
            HoverImpl(hovering);
        }
    }

    internal void CancelUse()
    {
        IsActive = false;
        cancelEvent?.Invoke();
        CancelImpl();
        ResetPositionAndParent();
        InteractingPlayer = null;
    }
    internal void Reset()
    {
        IsActive = false;
        ResetEvent?.Invoke();
        Triggered = false;
        ResetPositionAndParent();
        InteractingPlayer = null;
    }

    protected abstract bool InteractImpl();

    protected virtual void HoverImpl(bool hovering) { }
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
    JUMP = 0,
    INTERACT = 1
}