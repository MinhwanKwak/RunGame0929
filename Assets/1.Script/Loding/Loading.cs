using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField, Header("로딩바")]
    private Image LoadingBar;
    [SerializeField, Header("아이콘")]
    private Transform Icon;
    private float prexx = -785; // 아이콘 제어용

    private void Start()
    {
       // Cursor.visible = false;
        StartCoroutine(loadAsynSystem());
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    private IEnumerator loadAsynSystem()
    {
        // 씬 데이터를 불러오는데, 비동기로 처리
        var operation = SceneManager.LoadSceneAsync(LoadingManager.instance.moveScene);

        // 데이터를 모두 가져왔으나, true로 전환 할 때까지 이동X
        operation.allowSceneActivation = false;

        // isDone은 모든 처리가 끝나면 호출
        while(!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / 0.9f); // progress는 가져온 데이터 량을 표현. 모두 다 가져온 수치는 0.9

            LoadingBar.fillAmount = Mathf.MoveTowards(LoadingBar.fillAmount, 0.8f, Time.deltaTime*0.5f);

            // 아이콘 처리
            float rexx = Mathf.Lerp(-785, 785, LoadingBar.fillAmount);
            prexx = Mathf.Lerp(prexx, rexx, Time.deltaTime * 16f);
            Icon.localPosition = new Vector3(prexx, -354, 0);

            if (progress >= 0.8f &&LoadingBar.fillAmount >= 0.8f)
            {
                LoadingBar.fillAmount = Mathf.MoveTowards(LoadingBar.fillAmount, 1f, progress);
                Icon.localPosition = new Vector3(785, -354, 0);
            }

            if (progress >=1 && LoadingBar.fillAmount >= 1f)
                operation.allowSceneActivation = true;

            yield return null;
        }
    }
}
