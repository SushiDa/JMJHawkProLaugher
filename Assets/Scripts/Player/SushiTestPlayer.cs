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

    internal Rigidbody rb { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        AudioBridge.PlayMusic("Music/MusicLoop", 150, 32);
    }

    private void Update()
    {
        if(!MovementLocked)
            rb.velocity = Vector3.right * InputMovement * Speed;
    }

    private void OnMove(InputValue value)
    {
        InputMovement = value.Get<float>();
    }

    internal void ChangeAnimation(string animation)
    {

    }

    internal PlayerDirection GetCurrentDirection()
    {
        return PlayerDirection.FOOT;
    }
}
