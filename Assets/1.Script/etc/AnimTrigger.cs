using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimTrigger : MonoBehaviour
{
    [SerializeField, Header("애니메이션을 활성화 시킬 오브젝트")]
    private GameObject act;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거를 밟을 시
        if (other.CompareTag("Player"))
        {
            print("트리거");
            act.GetComponent<Animator>().enabled = true;
            if (act.gameObject.layer == 10)
            {
                AudioManager.Instance.PlaySoundSfx("BrokenRock");
            }
            else if (act.gameObject.layer == 11)
            {

                AudioManager.Instance.PlaySoundSfx("BrokenHumanRock");
            }
            else if(act.gameObject.layer == 14)
            {
                AudioManager.Instance.PlaySoundSfx("GroundTrigger");
            }
        }
    }
}
