using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CinemachineShake cinemachineShake; // 카메라 제어용 스크립트
    public Player player;
    public UiController uiController;
    public TutorialTrigger[] tutorialTrigger;
    

    public bool isPauseTutorial; // 튜토리얼 일시정지 체크
    public bool isGameOver = false;

    public int levelToUnlock = 2;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
       
        //씬이 변경되도 파괴하지 않는 기능 
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        // GameOver();
    }

    public void WinLevel(bool mission)
    {

        if (mission)
        {
            PlayerPrefs.SetInt("levelReached", levelToUnlock);
           // SceneManager.LoadScene("SelectScene");
        }
        else
        {
            Debug.Log("미션실패");
        }
    }

}
