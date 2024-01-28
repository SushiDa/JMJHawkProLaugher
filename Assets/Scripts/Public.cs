using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Public : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    public void publicEnDelire()
    {
        // Parcourir tous les enfants de cet objet
        foreach (Transform child in transform)
        {
            // V�rifier si l'enfant a un composant Animator
            Animator childAnimator = child.GetComponent<Animator>();

            // Si un composant Animator est trouv�, lancer la transition
            if (childAnimator != null)
            {
                childAnimator.CrossFadeNicely("Armature|HeadBang", 0);
            }
        }
    }
}
