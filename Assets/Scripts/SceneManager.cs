using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    
    public void LoadToPage(string scene_name){
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_name);
        Time.timeScale = 1f;
    }

    public void GameQuitter(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}