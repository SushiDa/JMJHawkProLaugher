using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SushiTestPlayer : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] internal Transform FootTransform;
    [SerializeField] internal Transform HandTransform;
    [SerializeField] internal Transform HeadTransform;

    internal bool MovementLocked;
    internal bool JumpLocked;
    internal bool InteractLocked;

    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        if (!MovementLocked)
        {
            float movement = value.Get<float>();
            _rb.velocity = Vector3.right * movement * Speed;
        }
    }
}
