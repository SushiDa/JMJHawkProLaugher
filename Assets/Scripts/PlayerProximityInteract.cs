using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProximityInteract : MonoBehaviour
{
    private List<AbstractItem> _interactables;

    AbstractItem currentHover;
    private void Awake()
    {
        _interactables = new List<AbstractItem>();
    }

    private void Update()
    {
        float closestDistance = Mathf.Infinity;
        AbstractItem hover = null;

        foreach(var interact in _interactables)
        {
            float distance = Vector3.Distance(transform.position, interact.transform.position);
            if (distance < closestDistance && !interact.triggered) hover = interact;
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
        if (value.isPressed)
        {
            currentHover?.Interact(gameObject);
        }
    }
}
