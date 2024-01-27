using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGameController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private PlayerInputHub inputHub;
    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private Transform fixedOrientator;

    [Header("Values")]
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float jumpforce = 1.5f;
    [SerializeField]
    private float fallMassMultiplier = 1.5f;

    private bool isFalling = true;




    void Update()
    {
        Interact();
    }

    void FixedUpdate()
    {
        FallGravityCorrection();
        JumpFixed();
        RotateFixed();
        MoveFixed();
    }




    private void Interact()
    {
        bool isInteract = inputHub.ReadInteract();
        // TODO
    }




    private void JumpFixed()
    {
        // Check if hub allow Jumping
        bool canJump = inputHub.ReadCanJump();
        if (!canJump) return;

        // Check if Hub says player is grounded
        bool isGrounded = inputHub.ReadIsGrounded(purge: true);
        if (!isGrounded) return;

        // Verify wanted to jump
        bool isJump = inputHub.ReadJump();
        if (!isJump) return;

        // Apply player gravity correction
        Vector3 velocity = rigidbody.velocity;
        velocity.y = 0f;
        rigidbody.velocity = velocity;
        FallGravityCorrection();

        // Apply Jump Force
        rigidbody.AddForce(fixedOrientator.up * jumpforce, ForceMode.Impulse);
    }




    private void RotateFixed()
    {
        // Check if Hub allow Rotation
        bool canRotate = inputHub.ReadCanRotate();
        if (!canRotate) return;

        // Calculate + Apply rotation
        float inputRotation = inputHub.ReadRotation();
        float speededRotation = inputRotation * rotationSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.AngleAxis(speededRotation, fixedOrientator.forward);
        rigidbody.MoveRotation(rigidbody.rotation * rotation);
    }




    private void MoveFixed()
    {
        // Check if Hub allow Moving
        bool canMove = inputHub.ReadCanMove();
        if (!canMove) return;

        // Calculate + Apply movement
        float inputMove = inputHub.ReadMovement();
        float speededMove = inputMove * moveSpeed * Time.fixedDeltaTime;
        Vector3 movement = fixedOrientator.right * speededMove;
        rigidbody.MovePosition(rigidbody.position + movement);
    }




    private void FallGravityCorrection()
    {
        bool wasFalling = isFalling;
        isFalling = rigidbody.velocity.y < 0f;
        if (!wasFalling && isFalling) rigidbody.mass *= fallMassMultiplier;
        else if (wasFalling && !isFalling) rigidbody.mass /= fallMassMultiplier;
    }
}