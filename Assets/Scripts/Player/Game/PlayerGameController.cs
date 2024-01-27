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
    private float fallMinVelocity = 0f;
    [SerializeField]
    private float fallForce = 1.5f;

    private bool isFalling = true;




    void FixedUpdate()
    {
        FallCorrection();
        JumpFixed();
        RotateFixed();
        MoveFixed();
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

        // Override Vertical Velocity + Apply Jump Force
        OverrideVerticalVelocity(0f);
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




    private void OverrideVerticalVelocity(float value)
    {
        // Override Velocity + Apply fall correction
        Vector3 velocity = rigidbody.velocity;
        velocity.y = value;
        rigidbody.velocity = velocity;
        FallCorrection();
    }

    private void FallCorrection()
    {
        // Verify and apply additional gravity
        bool isGrounded = inputHub.ReadIsGrounded();
        isFalling = rigidbody.velocity.y < fallMinVelocity;
        if (isFalling && !isGrounded) {
            Vector3 gravityForce = -fixedOrientator.up * fallForce;
            rigidbody.AddForce(gravityForce, ForceMode.Force);
        }
    }
}