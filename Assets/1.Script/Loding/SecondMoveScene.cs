using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondMoveScene : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startScene());

    }

    IEnumerator startScene()
    {
     yield return new  WaitForSeconds(7f);


        SceneManager.LoadScene("SelectScene");
    }
}
