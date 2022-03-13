using UnityEditor;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
    }

    public void LoadToPage(string scene_name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);
        Time.timeScale = 1f;
    }

    public void GameQuitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}