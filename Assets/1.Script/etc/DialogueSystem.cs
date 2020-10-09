using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public Sprite cg;
    public string name;
    [TextArea]
    public string sentences;
    public Color color;
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private Image StandingCG;
    [SerializeField]
    private Text SentenceTx;
    [SerializeField]
    private Text NameTx;
    [SerializeField]
    private GameObject PressShiftKeyTx;
    [SerializeField]
    private Dialogue[] dialogues;
    [SerializeField]
    private Animator Fade_in;
    [SerializeField]
    private GameObject Fade_out;

    private Queue<string> Sentences = new Queue<string>();

    private bool isDialogue;
    private int count;

    private void Awake()
    {
        Fade_out.SetActive(true);
        count = 0;
        isDialogue = false;
        NextDialogue();
    }

    private void Update()
    {
        if (isDialogue)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
              //  if (GameManager.Instance.uiController.isPause) return; // 일시정지가 켜져있으면 키를 동작할 수 없게함
              if(count < dialogues.Length)
                {
                    if(count == 1)
                    {
                        NameTx.gameObject.transform.parent.gameObject.SetActive(true);
                        StandingCG.gameObject.SetActive(true);
                    }

                    NextDialogue();
                    PressShiftKeyTx.SetActive(false);
                    isDialogue = false;
                    //SentenceTx.color = dialogues[count].color;
                }
                else
                {
                    Skip();
                }
            }
        }
    }

    private void NextDialogue()
    {
        StopAllCoroutines();
        if(dialogues[count].name != null)
            NameTx.text = dialogues[count].name; // 이름
        if(dialogues[count].cg != null)
        {
            StandingCG.sprite = dialogues[count].cg; // 캐릭터 이미지
            StandingCG.SetNativeSize();
        }
        string Des = dialogues[count].sentences;
        StartCoroutine(ShowSentence());
        IEnumerator ShowSentence()
        {
            print("코르틴");
            print(Des);
            if (SentenceTx != null)
            {
                SentenceTx.text = "";
                for (int i = 0; i <= Des.Length; i++)
                {
                    SentenceTx.text = Des.Substring(0, i);
                    yield return new WaitForSecondsRealtime(0.07f);
                }
            }
            PressShiftKeyTx.SetActive(true);
            Invoke("showPressShiftKey", 0.5f);
        }
    }

    public void showPressShiftKey()
    {
        print("아무버튼");
        isDialogue = true;
        count++;
    }

    public void Skip()
    {
        Fade_in.enabled = true;
    }
}
