using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    public void Next()
    {
        SceneManager.LoadScene(SceneName);
    }
}
