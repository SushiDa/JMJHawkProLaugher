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
    private AnimatorClipInfo[] clipInfo;
    private float previousMovement = 0f;

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
        // Verify wanted to jump
        bool isJump = InputHub.ReadJump();
        if (!isJump) return;

        // Check if hub allow Jumping
        bool canJump = InputHub.ReadCanJump();
        if (!canJump) return;

        // Check if Hub says player is grounded
        bool isGrounded = InputHub.ReadIsGrounded(purge: true);
        if (!isGrounded) return;

        // Override Vertical Velocity + Apply Jump Force
        OverrideVerticalVelocity(0f);
        rigidbody.AddForce(FixedOrientator.up * jumpforce, ForceMode.Impulse);
        GameEvents.PlayerJump?.Invoke();
        //#plusRienAFoutre
        animator.SetInteger("JumpingState", 1);
        animator.CrossFadeNicely("Armature|Jumpascend 0", 0);
    }




    private void RotateFixed()
    {
        // Check if Hub allow Rotation
        bool canRotate = InputHub.ReadCanRotate();
        if (!canRotate) return;

        // Calculate + Apply rotation
        float inputRotation = InputHub.ReadRotation();
        float speededRotation = inputRotation * rotationSpeed * Time.fixedDeltaTime;
        transform.Rotate(FixedOrientator.forward * speededRotation, Space.World);
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

        // Calculate Animation / Rotation Things and apply them
        if (!Mathf.Approximately(speededMove, 0f)) {
            bool directionChanged = Mathf.Sign(previousMovement) != Mathf.Sign(speededMove);
            if (directionChanged) transform.Rotate(0f, 180f, 0f);
            previousMovement = speededMove;
            animator.SetBool("isMoving", true);
        } else animator.SetBool("isMoving", false);
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
        } else animator.SetInteger("JumpingState", 0);
    }

    public string GetCurrentClipName()
    {
        int layerIndex = 0;
        clipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex);
        return clipInfo[0].clip.name;
    }

}