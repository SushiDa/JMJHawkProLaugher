using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAnimationLink : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public Animator Animator => animator;
}