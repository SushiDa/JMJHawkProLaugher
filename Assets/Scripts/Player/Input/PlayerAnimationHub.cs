using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAnimationHub : MonoBehaviour
{
    private Animator animator;
    private Animator Animator { 
        get { 
            if (animator == null) {
                PlayerAnimationLink link = FindObjectOfType<PlayerAnimationLink>(includeInactive: false);
                if (link != null) animator = link.Animator;
            }
            return animator;
        }
    }


    public void Play(AnimationClip clip) => Play(clip.name);
    public void Play(string stateName)
    {
        if (Animator == null) return;
        Animator.CrossFadeNicely(stateName, 0.1f);
    }
}