using UnityEngine;

[DisallowMultipleComponent]
public class PlayerInputHub : MonoBehaviour
{
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



    private PlayerInputWrapper playerInputWrapper;
    public PlayerInputWrapper PlayerInputWrapper { 
        get {
            if (playerInputWrapper == null) playerInputWrapper = FindObjectOfType<PlayerInputWrapper>(includeInactive: true);
            return playerInputWrapper;
        }
    }




    private Vector2 cursorMovement = Vector2.zero;
    public Vector2 CursorMovement { 
        set { cursorMovement = value; }
        private get { return cursorMovement; }
    }

    public Vector2 ReadCursorMovement(bool purge = false)
    {
        Vector2 value = CursorMovement;
        if (purge) CursorMovement = Vector2.zero;
        return value;
    }




    private bool canMove = true;
    public bool CanMove { 
        set { canMove = value; }
        private get { return canMove; }
    }

    public bool ReadCanMove(bool purge = false)
    {
        bool value = CanMove;
        if (purge) CanMove = true;
        return value;
    }




    private float movement = 0f;
    public float Movement {
        set { movement = value; }
        private get { return movement; }
    }

    public float ReadMovement(bool purge = false)
    {
        float value = Movement;
        if (purge) Movement = 0f;
        return value;
    }




    private bool canRotate = false;
    public bool CanRotate { 
        set { canRotate = value; }
        private get { return canRotate; }
    }

    public bool ReadCanRotate(bool purge = false)
    {
        bool value = CanRotate;
        if (!value) return !ReadIsGrounded();
        if (purge) CanRotate = false;
        return value;
    }




    private float rotation = 0f;
    public float Rotation {
        set { rotation = value; }
        private get { return rotation; }
    }

    public float ReadRotation(bool purge = false)
    {
        float value = Rotation;
        if (purge) Rotation = 0f;
        return value;
    }




    private bool canJump = true;
    public bool CanJump { 
        set { canJump = value; }
        private get { return canJump; }
    }

    public bool ReadCanJump(bool purge = false)
    {
        bool value = CanJump;
        if (purge) CanJump = true;
        return value;
    }




    private bool isGrounded = false;
    public bool IsGrounded {
        set { isGrounded = value; }
        private get { return isGrounded; }
    }

    private GroundDetector GroundDetector => FindObjectOfType<GroundDetector>(includeInactive: true);
    public bool ReadIsGrounded(bool purge = false)
    {
        bool value = IsGrounded;
        if (!value) {
            GroundDetector groundDetector = GroundDetector;
            if (groundDetector == null) return false;
            return groundDetector.IsGrounded;
        }        
        if (purge) IsGrounded = false;
        return isGrounded;
    }




    private bool isJump = false;
    public bool IsJump {
        set { isJump = value; }
        private get { return isJump; }
    }

    public bool ReadJump(bool purge = true)
    {
        bool value = IsJump;
        if (purge) IsJump = false;
        return value;
    }




    private bool isInteract = false;
    public bool IsInteract {
        set { isInteract = value; }
        private get { return isInteract; }
    }

    public bool ReadInteract(bool purge = true)
    {
        bool value = IsInteract;
        if (purge) IsInteract = false;
        return value;
    }



    public PlayerDirection ReadPlayerDirection()
    {
        // Verify we have a ground detector
        GroundDetector groundDetector = GroundDetector;
        if (groundDetector == null) return PlayerDirection.UNKNOWN;

        // Calculate Rotations for angle comparaison
        Quaternion detectorUp = Quaternion.LookRotation(groundDetector.transform.up);
        Quaternion absoluteUp = Quaternion.LookRotation(-FixedOrientator.up);
        Quaternion absoluteRight = Quaternion.LookRotation(FixedOrientator.right);

        // Compare rotations angles
        float upDifference = Quaternion.Angle(detectorUp, absoluteUp);
        float rightDifference = Quaternion.Angle(detectorUp, absoluteRight);

        // Find wich direction correspond to our angle differences
        if (upDifference <= 45f) return PlayerDirection.HEAD; // UP
        else if (upDifference <= 135f && rightDifference <= 45f) return PlayerDirection.FRONT; // RIGHT
        else if (upDifference <= 135f && rightDifference > 45f) return PlayerDirection.BACK; // LEFT
        else return PlayerDirection.FOOT; // DOWN
    }
}