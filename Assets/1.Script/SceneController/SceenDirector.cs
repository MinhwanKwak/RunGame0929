using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class SceenDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //public void GameStart()
    //{
    //    SceneManager.LoadScene("SelectScene");
    //}

    public void StartNewScene()
    {
        LoadingManager.instance.beforeSceneSetup("Standing");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
        
}
