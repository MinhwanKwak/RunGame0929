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
        AudioManager.Instance.PlaySoundSfx("ButtonClick");
        LoadingManager.instance.beforeSceneSetup("Standing");
    }

    public void LoadScene()
    {
        
        AudioManager.Instance.PlaySoundSfx("ButtonClick");
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
