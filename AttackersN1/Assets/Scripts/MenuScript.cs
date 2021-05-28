using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void ChangeScene(string ceneName)
    {
        SceneManager.LoadScene(ceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
