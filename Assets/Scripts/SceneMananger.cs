using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMananger : MonoBehaviour
{

    public void LoadScene(int idex)
    {
        SceneManager.LoadScene(idex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
