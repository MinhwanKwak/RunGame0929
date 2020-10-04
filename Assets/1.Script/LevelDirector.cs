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
                //아직 클리어 못한경우 


                LevelButtons[i].transform.GetChild(7).gameObject.SetActive(true);
                LevelButtons[i].transform.GetChild(8).gameObject.SetActive(true);

                LevelButtons[i].transform.GetChild(2).gameObject.SetActive(false);
                LevelButtons[i].transform.GetChild(3).gameObject.SetActive(false);
                LevelButtons[i].transform.GetChild(4).gameObject.SetActive(false);
                LevelButtons[i].transform.GetChild(5).gameObject.SetActive(false);
                LevelButtons[i].transform.GetChild(6).gameObject.SetActive(false);


                LevelButtons[i].interactable = false;
            }
            else
            {
                //클리어시 
                int CurrenntStartGamekey = levelReadched -1;

                if (CurrenntStartGamekey != i)
                {
                    LevelButtons[i].transform.GetChild(2).gameObject.SetActive(true);
                    LevelButtons[i].transform.GetChild(3).gameObject.SetActive(true);
                    LevelButtons[i].transform.GetChild(5).gameObject.SetActive(true);


                    LevelButtons[i].transform.GetChild(4).gameObject.SetActive(false);
                    LevelButtons[i].transform.GetChild(6).gameObject.SetActive(false);
                    LevelButtons[i].transform.GetChild(7).gameObject.SetActive(false);
                    LevelButtons[i].transform.GetChild(8).gameObject.SetActive(false);



                    LevelButtons[i].transform.GetChild(7).gameObject.SetActive(false);
                  
                }
                else
                {
                   
                    //UnLock 인 경우 
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(4).gameObject.SetActive(true);
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(6).gameObject.SetActive(true);


                    LevelButtons[CurrenntStartGamekey].transform.GetChild(2).gameObject.SetActive(false);
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(3).gameObject.SetActive(false);
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(5).gameObject.SetActive(false);
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(7).gameObject.SetActive(false);
                    LevelButtons[CurrenntStartGamekey].transform.GetChild(8).gameObject.SetActive(false);

                }


                int StageNumber = i + 1;
                string GetTimeKeyCode = "Stage" + StageNumber;
                string temp = PlayerPrefs.GetString(GetTimeKeyCode);



                //임시 초기화 코드 
                PlayerPrefs.DeleteAll();

                ClearTexts[i].text = temp;
                
            }
        }
    }

    public void StageSceneMove(string Stagename)
    {
      
        LoadingManager.instance.beforeSceneSetup(Stagename);
    }


    public void BeforeScene()
    {
        SceneManager.LoadScene("SelectScene");
        
    }

    
    
}
