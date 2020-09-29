using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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


    public void GameStart()
    {
        SceneManager.LoadScene("SelectScene");
    }

    public void StartNewScene()
    {
        LoadingManager.instance.beforeSceneSetup("Stage1");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
