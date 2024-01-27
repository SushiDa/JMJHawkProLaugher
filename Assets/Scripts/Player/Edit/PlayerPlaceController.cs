using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerEditController))]
public class PlayerPlaceController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Transform placeKeeper;
    [SerializeField]
    private Transform placeCursor;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private bool isMultipleParts = false;
    private PlayerEditController editController;
    private GameObject placeVisual;

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

    private new Camera camera;
    public Camera Camera { 
        get {
            if (camera == null) camera = Camera.main;
            return camera;
        }
    }



#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying) return;
        enabled = false;
    }
#endif




    void Awake()
    {
        editController = GetComponent<PlayerEditController>();
    }

    void Update()
    {
        if (InputHub == null || FixedOrientator == null || Camera == null) return;
        MoveCursor();
        RotateCursor();
        CancelPlacement();
        ConfirmPlacement();
    }





    private void CancelPlacement()
    {
        // Check if Cancel + if needed apply + exit
        if (isMultipleParts) return;
        bool isCancel = InputHub.ReadInteract();
        if (isCancel) Exit();
    }

    private void ConfirmPlacement()
    {
        // Verify want & can confirm + do it
        bool isConfirm = InputHub.ReadJump();
        if (!isConfirm || placeVisual == null) return;
        placeVisual.transform.SetParent(placeKeeper, true);
        
        // Go to Next part if there is one
        if (placeVisual.TryGetComponent(out ObjectMultiPart multipart) && multipart.NextGraphicPrefab != null) {
            placeVisual = null;
            Place(multipart.NextGraphicPrefab);
            isMultipleParts = true;

            // Link Parts together
            if (!placeVisual.TryGetComponent(out ObjectMultiPart newMultipart)) newMultipart = placeVisual.AddComponent<ObjectMultiPart>();
            newMultipart.LinkedPrevious = multipart;
            multipart.LinkedNext = newMultipart;
            
        } else { // otherwise Exit
            placeVisual = null;
            isMultipleParts = false;
            editController.ConsumeEditing();
            Exit();
        }
    }

    private void MoveCursor()
    {
        // Check if Hub allow Moving
        bool canMove = InputHub.ReadCanMove();
        if (!canMove) return;

        // Calculate Screen relative movement
        Vector2 inputMove = InputHub.ReadCursorMovement();
        Vector2 speededMove = inputMove * moveSpeed * Time.deltaTime;

        // Calculate + Apply Movement from Screen relative to World
        Vector3 screenPosition = Camera.WorldToViewportPoint(placeCursor.transform.position);
        screenPosition += new Vector3(speededMove.x, speededMove.y, 0f);
        screenPosition.x = Mathf.Clamp01(screenPosition.x);
        screenPosition.y = Mathf.Clamp01(screenPosition.y);
        placeCursor.transform.position = Camera.ViewportToWorldPoint(screenPosition);
    }

    private void RotateCursor()
    {
        // Check if Hub allow Rotation
        bool canRotate = InputHub.ReadCanRotate();
        if (!canRotate) return;

        // Calculate + Apply rotation
        float inputRotation = InputHub.ReadRotation();
        float speededRotation = inputRotation * rotationSpeed * Time.deltaTime;
        placeCursor.rotation *= Quaternion.AngleAxis(speededRotation, FixedOrientator.forward);
    }





    public void Place(GameObject objectPrefab)
    {
        if (placeVisual != null) Destroy(placeVisual);
        if (objectPrefab != null) placeVisual = Instantiate(objectPrefab, placeCursor);
        else placeVisual = null;
    }

    public void Exit()
    {
        Place(null);
        editController.Choose();
    }
}