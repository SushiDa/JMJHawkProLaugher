using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerGameController : MonoBehaviour
{
    [Header("Settings")]
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

    [SerializeField] private Transform footTransform;
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform headTransform;

    internal Transform FootTransform => footTransform;
    internal Transform HandTransform => handTransform;
    internal Transform HeadTransform => headTransform;

    internal new Rigidbody rigidbody { get; private set; }
    //Deso moi je code pas aussi beau
    public Animator animator;
    public Transform armature;
    private Quaternion eulerRotation;

    private PlayerInputHub inputHub;
    public PlayerInputHub InputHub { 
        get {
            if (inputHub == null) inputHub = FindObjectOfType<PlayerInputHub>();
            return inputHub;
        }
    }

    private Transform fixedOrientator;
    public Transform FixedOrientator { 
        get {
            if (fixedOrientator == null) {
                GameObject gameobject = GameObject.FindGameObjectWithTag(nameof(FixedOrientator));
                fixedOrientator = gameobject.transform;
            }
            return fixedOrientator;
        }
    }




    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (InputHub == null) return;
        if (FixedOrientator == null) return;
        FallCorrection();
        JumpFixed();
        RotateFixed();
        MoveFixed();
    }




    private void JumpFixed()
    {
        // Check if hub allow Jumping
        bool canJump = InputHub.ReadCanJump();
        if (!canJump) return;

        // Check if Hub says player is grounded
        bool isGrounded = InputHub.ReadIsGrounded(purge: true);
        if (!isGrounded) return;

        // Verify wanted to jump
        bool isJump = InputHub.ReadJump();
        if (!isJump) return;

        // Override Vertical Velocity + Apply Jump Force
        OverrideVerticalVelocity(0f);
        rigidbody.AddForce(FixedOrientator.up * jumpforce, ForceMode.Impulse);
        GameEvents.PlayerJump?.Invoke();
        animator.SetInteger("JumpingState", 1);
    }




    private void RotateFixed()
    {
        // Check if Hub allow Rotation
        bool canRotate = InputHub.ReadCanRotate();
        if (!canRotate) return;

        // Calculate + Apply rotation
        float inputRotation = InputHub.ReadRotation();
        float speededRotation = inputRotation * rotationSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.AngleAxis(speededRotation, FixedOrientator.forward);
        rigidbody.MoveRotation(rigidbody.rotation * rotation);
    }




    private void MoveFixed()
    {
        // Check if Hub allow Moving
        bool canMove = InputHub.ReadCanMove();
        if (!canMove) return;

        // Calculate + Apply movement
        float inputMove = InputHub.ReadMovement();
        float speededMove = inputMove * moveSpeed;
        Vector3 orientedVelocity = FixedOrientator.worldToLocalMatrix.MultiplyVector(rigidbody.velocity);
        orientedVelocity.x = speededMove;
        rigidbody.velocity = FixedOrientator.localToWorldMatrix.MultiplyVector(orientedVelocity);
        if (inputMove != 0)
        {
            animator.SetBool("isMoving", true);
            if (inputMove < 0) { 
                eulerRotation = Quaternion.Euler(0, 180, 0);
                transform.rotation = eulerRotation;
            }
            else if (inputMove > 0){
                eulerRotation = Quaternion.Euler(0, 0, 0);
                transform.rotation = eulerRotation;
            }
        }
        else
            animator.SetBool("isMoving", false);
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
        bool isGrounded = InputHub.ReadIsGrounded();
        bool isFalling = rigidbody.velocity.y < fallMinVelocity;
        if (!isGrounded && isFalling) {
            Vector3 gravityForce = -FixedOrientator.up * fallForce;
            rigidbody.AddForce(gravityForce, ForceMode.Force);
        }
        else
        {
            animator.SetInteger("JumpingState", 0);
        }
    }

}