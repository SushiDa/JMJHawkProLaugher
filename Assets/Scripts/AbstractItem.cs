using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractItem : MonoBehaviour
{
    internal Action HoverOnEvent;
    internal Action HoverOffEvent;
    internal Action ResetEvent;

    internal bool triggered;

    protected Vector2 initialPosition;
    protected Transform initialParent;

    internal void Hover(bool hovering)
    {
        if (hovering) HoverOnEvent?.Invoke();
        else HoverOffEvent?.Invoke();
        HoverImpl(hovering);
    }

    internal abstract bool Interact(GameObject source);

    internal virtual void HoverImpl(bool hovering){ }

    internal void Reset()
    {
        ResetEvent?.Invoke();
        triggered = false;
        ResetPositionAndParent();
    }

    internal virtual void ResetImpl() { }

    protected void ResetPositionAndParent()
    {
        transform.localPosition = initialPosition;
        transform.parent = initialParent;
    }
}
