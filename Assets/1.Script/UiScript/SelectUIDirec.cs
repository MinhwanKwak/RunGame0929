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
        Option.gameObject.SetActive(true);
        
    }


    public void OptionButtonExit()
    {
        Option.gameObject.SetActive(false);
        
    }


    public void OptionButtonDirector()
    {
        Directors.gameObject.SetActive(true);
    }

    public void OptionButtonDirectorDown()
    {
        Directors.gameObject.SetActive(false);
    }
}
