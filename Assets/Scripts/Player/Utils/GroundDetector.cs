using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class GroundDetector : MonoBehaviour
{
    [SerializeField, Tooltip("The tag of the surface wanted to collide with")]
    private string[] surfaceTags = new string[] { "Untagged" };

    private int triggerCount = 0;
    public bool IsGrounded {
        get { return triggerCount > 0; }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (TryGetComponent(out Collider col)) {
            col.isTrigger = true;
        }
    }
#endif

    void OnTriggerEnter(Collider other)
    {
        if (surfaceTags.Any((string s) => other.CompareTag(s))) {
            ++triggerCount;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (surfaceTags.Any((string s) => other.CompareTag(s))) {
            --triggerCount;
        }
    }
}