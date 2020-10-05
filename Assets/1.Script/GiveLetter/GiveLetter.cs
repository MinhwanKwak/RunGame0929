using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveLetter : MonoBehaviour
{

   public GameObject PostObj;

   public float PostSpeed = 10f;
   private float CurrentSpeed = 0f;
   
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
        GameObject post = Instantiate(PostObj);
    }



    public void StartSendMail()
    {
        if(PostBox != null)
        {
            if(CurrentSpeed <= PostSpeed)
            {
                CurrentSpeed += PostSpeed * Time.deltaTime; 
            }

            transform.position += transform.up * CurrentSpeed * Time.deltaTime;
            


            Vector3 t_dir = (PostBox.transform.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PostBox")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

        }
    }

}
