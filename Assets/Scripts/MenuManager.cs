using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioBridge.PlayMusic("Music/JMJ");
    }

    public void ButtonSound()
    {
        AudioBridge.PlaySFX("sfx/pop");

    }
}
