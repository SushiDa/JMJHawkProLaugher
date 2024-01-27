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

    float InputMovement;

    internal Rigidbody rigidbody { get; private set; }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(!MovementLocked)
            rigidbody.velocity = Vector3.right * InputMovement * Speed;
    }

    private void OnMove(InputValue value)
    {
        InputMovement = value.Get<float>();
    }
}
