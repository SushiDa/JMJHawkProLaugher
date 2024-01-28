using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMananger : MonoBehaviour
{
    public Animator jmj;

    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneAfterAnimation(index));
    }

    private IEnumerator LoadSceneAfterAnimation(int index)
    {
        jmj.CrossFadeNicely("Dab", 0);
        yield return new WaitForSeconds(1f); // Attend la fin de l'animation
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
