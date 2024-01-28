using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class PlayerGameRotationCorrection : MonoBehaviour
{
    [SerializeField]
    private string[] groundTags = new string[] { "Untagged" };
    [SerializeField]
    private GameObject failFxPrefab;

    private PlayerInputHub playerInputHub;
    public PlayerInputHub PlayerInputHub { 
        get {
            if (playerInputHub == null) playerInputHub = FindObjectOfType<PlayerInputHub>();
            return playerInputHub;
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




    void OnCollisionEnter(Collision collision)
    {
        // Check if collision is with ground
        if (PlayerInputHub == null) return;
        if (groundTags.Any((string tag) => collision.transform.CompareTag(tag))) {
            PlayerDirection direction = PlayerInputHub.ReadPlayerDirection();

            // Verify + do fail callback
            switch (direction) {
                case PlayerDirection.HEAD:
                case PlayerDirection.FRONT:
                case PlayerDirection.BACK: {
                    if (failFxPrefab != null) Instantiate(failFxPrefab, transform.position, Quaternion.identity);
                    GameEvents.playerFallFail?.Invoke(direction);
                    break;
                }
            }

            // Reset Player Rotation
            Vector3 rotation = FixedOrientator.worldToLocalMatrix.MultiplyVector(transform.rotation.eulerAngles);
            rotation.z = 0f;
            transform.rotation = Quaternion.Euler(FixedOrientator.localToWorldMatrix.MultiplyVector(rotation));
        }
    }
}