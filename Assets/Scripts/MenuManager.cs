using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        AudioBridge.PlayMusic("Music/JMJ_Title");
    }

    public void ButtonSound()
    {
        AudioBridge.PlaySFX("sfx/pop");
    }
}