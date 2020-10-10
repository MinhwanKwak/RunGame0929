using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPosRevision : MonoBehaviour
{
    bool zPosRevision;
    Transform playerTransform;
    float time = 0;

    void Update()
    {
        if (zPosRevision)
        {
            time += Time.deltaTime;
            if(GameManager.Instance.player.transform.position.z != 0)
            {
                var playerPos = GameManager.Instance.player.transform.position;
                var posZ  = Mathf.Lerp(playerTransform.position.z, 0, Time.deltaTime * 3);

                GameManager.Instance.player.transform.position
                    = new Vector3(playerPos.x, playerPos.y, posZ);
            }

            if (time > 5f)
            {
                zPosRevision = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = GameManager.Instance.player.transform;
            zPosRevision = true;
        }
    }
}
