using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Scene loader that has public methods for loading the next scene in the build index, loading the first scene in the build index, quiting the application, and reloading the current active scene in the build index
/// </summary>
public class SceneLoader : MonoBehaviour
{

    public static SceneLoader sceneLoader;

    private void Awake()
    {
        if(sceneLoader == null)
        {
            sceneLoader = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(sceneLoader);
            return;
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void LoadSceneByName(string name) // call this method  from scripting when you want to get a scene by its name
    {
        SceneManager.LoadScene(name);
    }
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
