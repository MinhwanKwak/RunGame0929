using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDirector : MonoBehaviour
{
    public Button[] LevelButtons;

    public Text[] ClearTexts;

    private void Start()
    {
        int levelReadched = PlayerPrefs.GetInt("levelReached", 1);
        for(int i = 0; i  < LevelButtons.Length; ++i)
        {

            if (i + 1 > levelReadched)
            {
                LevelButtons[i].transform.GetChild(2).gameObject.SetActive(true);
                
                LevelButtons[i].interactable = false;
            }
            else
            {
  
                LevelButtons[i].transform.GetChild(3).gameObject.SetActive(true);

                int StageNumber = i + 1;
                string GetTimeKeyCode = "Stage" + StageNumber;
                string temp = PlayerPrefs.GetString(GetTimeKeyCode);
                ClearTexts[i].text = temp;
            }
        }
    }

    public void StageSceneMove(string sceneName)
    {
        LoadingManager.instance.beforeSceneSetup(sceneName);
    }


    public void BeforeScene()
    {
        SceneManager.LoadScene("SelectScene");
        
    }
}
