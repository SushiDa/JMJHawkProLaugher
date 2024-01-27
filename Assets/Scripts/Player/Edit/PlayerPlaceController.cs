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
    private float placeMoveSpeed = 1f;
    [SerializeField]
    private float placeRotationSpeed = 1f;
    
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





    void Awake()
    {
        editController = GetComponent<PlayerEditController>();
    }

    void Update()
    {

    }





    private void MoveCursor()
    {
        Vector2 cursorMovementInput = InputHub.ReadCursorMovement();

        Camera camera = Camera.main;
        // TODO
    }

    private void RotateObject()
    {
        float rotationInput = InputHub.ReadRotation();

    }

    private void ConfirmPlacement()
    {
        if (placeVisual == null) return;
        placeVisual.transform.SetParent(placeKeeper, true);
        placeVisual = null;
    }

    private void CancelPlacement()
    {

    }





    public void Place(GameObject objectPrefab)
    {
        if (placeVisual != null) Destroy(placeVisual);
        if (objectPrefab != null) placeVisual = Instantiate(objectPrefab, placeCursor);
    }

    public void Exit()
    {
        editController.Choose();
    }
}