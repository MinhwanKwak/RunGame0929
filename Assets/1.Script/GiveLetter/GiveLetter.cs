using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveLetter : MonoBehaviour
{

    public GameObject PostObj;

    private float dis;
    private float speed;
    private float waitTime;
    private float delspeed = 2f;

    private GameObject PostBox;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartSendMail();
    }


    //현재 충돌된 postboX를 가져온다.
    public void GetPostBox(GameObject GetBox)
    {
        PostBox = GetBox;
    }



    public void StartSendMail()
    {
        if (PostBox == null) return;
        dis = Vector3.Distance(transform.position, PostBox.transform.position);


        waitTime += Time.deltaTime;
        //1.5초 동안 천천히 forward 방향으로 전진합니다
        if (waitTime < 0.1f)
        {
            speed = Time.deltaTime * delspeed;
            transform.Translate(transform.forward * speed, Space.World);
        }
        else
        {
            // 1.5초 이후 타겟방향으로 lerp위치이동 합니다

            speed += Time.deltaTime;
            float t = speed / dis;

            transform.position = Vector3.LerpUnclamped(transform.position, PostBox.transform.position, t);

        }


        // 매프레임마다 타겟방향으로 포탄이 방향을바꿉니다
        //타겟위치 - 포탄위치 = 포탄이 타겟한테서의 방향
        Vector3 directionVec = PostBox.transform.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 2f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PostBox")
        {
            Destroy(gameObject);
        }
    }

}
