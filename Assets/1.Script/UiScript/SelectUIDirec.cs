using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUIDirec : MonoBehaviour
{
    public Image Option;
    public Image Directors;

    


    public void OptionButtonClick()
    {

        AudioManager.Instance.PlaySoundSfx("ButtonClick");
        Option.gameObject.SetActive(true);
        
    }


    public void OptionButtonExit()
    {

        AudioManager.Instance.PlaySoundSfx("ButtonClick");
        Option.gameObject.SetActive(false);
        
    }


    public void OptionButtonDirector()
    {

        AudioManager.Instance.PlaySoundSfx("ButtonClick");
        Directors.gameObject.SetActive(true);
    }

    public void OptionButtonDirectorDown()
    {

        AudioManager.Instance.PlaySoundSfx("ButtonClick");
        Directors.gameObject.SetActive(false);
    }
}
